#region License
/*
    Copyright (c) 2011 Jimmy Gilles <jimmygilles@gmail.com>
 
    This file is part of GpgApi.

    GpgApi is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, version 3 of the License.

    GpgApi is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with GpgApi. If not, see <http://www.gnu.org/licenses/>.
*/
#endregion License

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace GpgApi
{
    public delegate void GpgInterfaceEventHandler(GpgInterface sender, GpgInterfaceResult result);

    public abstract class GpgInterface
    {
        private sealed class GpgOutput
        {
            //public Byte[] Raw { get; private set; }
            public String Str { get; private set; }

            public GpgOutput(Byte[] val)
            {
                //Raw = val;
                Str = Encoding.UTF8.GetString(val);
                Str = Str.Replace("\r\n", "").Replace("\n", "");
            }
        }

        /// <summary>
        /// Absolute path to the GPG executable.
        /// </summary>
        public static String ExePath = null;
        public static SynchronizationContext SynchronizationContext = null;

        public event GpgInterfaceEventHandler GpgInterfaceEvent;
        public Func<AskPassphraseInfo, String> AskPassphrase = null;

        private const Int32 PassphraseMaxTries = 3;
        private Process _process = null;
        private Boolean _aborted = false;
        private Boolean _processExited = false;
        private Boolean _alreadyUsed = false;
        private Object _alreadyUsedLock = new Object();
        private Int32 _tries = 0;
        private Thread _outputThread = null;
        private ConcurrentQueue<GpgOutput> _output = new ConcurrentQueue<GpgOutput>();

        private EventWaitHandle _outputEventWait = new ManualResetEvent(false);
        private EventWaitHandle _abortedEventWait = new ManualResetEvent(false);
        private EventWaitHandle _exitedEventWait = new ManualResetEvent(false);

        internal GpgInterface()
        {
        }

        // ----------------------------------------------------------------------------------------

        // internal AND protected
        internal void EmitEvent(GpgInterfaceResult result)
        {
            Log(result.Status.ToString());

            if (GpgInterfaceEvent == null)
                return;

            if (SynchronizationContext == null)
                GpgInterfaceEvent(this, result);
            else
                SynchronizationContext.Send(delegate { GpgInterfaceEvent(this, result); }, null);
        }

        // internal AND protected
        internal String InternalAskPassphrase(String userid, Boolean isnewpassphrase = false, Boolean issymmetric = false)
        {
            AskPassphraseInfo info = new AskPassphraseInfo(userid, isnewpassphrase, issymmetric, issymmetric ? 1 : PassphraseMaxTries - _tries);

            String result = null;

            if (SynchronizationContext == null)
                result = AskPassphrase(info);
            else
                SynchronizationContext.Send(delegate { result = AskPassphrase(info); }, null);

            _tries++;

            return result;
        }

        // ----------------------------------------------------------------------------------------

        public void ExecuteAsync(Action<GpgInterfaceResult> Completed = null)
        {
            Thread thread = new Thread(delegate()
            {
                GpgInterfaceResult result = Execute();

                if (Completed == null)
                    return;

                if (SynchronizationContext == null)
                    Completed(result);
                else
                    SynchronizationContext.Send(delegate { Completed(result); }, null);
            });

            thread.Name = ToString();
            thread.Start();
        }

        public GpgInterfaceResult Execute()
        {
            GpgInterfaceResult result = null;

            try
            {
                lock (_alreadyUsedLock)
                {
                    if (_alreadyUsed)
                        throw new GpgInterfaceAlreadyUsed();
                    _alreadyUsed = true;
                }

                if (!File.Exists(GpgInterface.ExePath))
                    throw new FileNotFoundException(null, GpgInterface.ExePath);

                Log("Execute " + this);

                result = GpgInterfaceResult.Started;
                EmitEvent(result);

                result = BeforeStartProcess();
                if (result.Status != GpgInterfaceStatus.Success)
                {
                    EmitEvent(result);
                    return result;
                }

                _process = GetProcess(Arguments());
                _process.Exited += delegate(Object sender, EventArgs args)
                {
                    _processExited = true;
                    _exitedEventWait.Set();
                };
                _process.EnableRaisingEvents = true;

                _process.Start();
                _process.BeginErrorReadLine();

                _outputThread = new Thread(OutputReader) { Name = ToString() + " - OutputReader" };
                _outputThread.Start();

                while (WaitHandle.WaitAny(new WaitHandle[] { _outputEventWait, _abortedEventWait, _exitedEventWait }) != -1)
                {
                    if (_aborted)
                        break;

                    if (_processExited)
                    {
                        if (_outputThread != null)
                            _outputThread.Join();
                        _outputThread = null;
                    }

                    _outputEventWait.Reset();

                    GpgOutput output;
                    while (!_aborted && _output.TryDequeue(out output))
                    {
                        Log(output.Str);
                        result = ProcessLine(output.Str);

                        if (result.Status == GpgInterfaceStatus.Success)
                            continue;

                        EmitEvent(result);

                        if (result.Status == GpgInterfaceStatus.Error)
                        {
                            _aborted = true;
                            break;
                        }
                    }

                    if (_processExited && _outputThread == null)
                        break;
                }

                if (!_aborted)
                {
                    result = GpgInterfaceResult.Success;
                    EmitEvent(result);
                }
                else if (_aborted && result.Status != GpgInterfaceStatus.Error)
                {
                    result = GpgInterfaceResult.UserAbort;
                    EmitEvent(result);
                }

                Log("Exit");
            }
            catch (Exception e)
            {
                result = new GpgInterfaceResult(GpgInterfaceStatus.Error, GpgInterfaceMessage.None, e);
                EmitEvent(result);
            }
            finally
            {
                CleanUp();
            }

            return result;
        }

        /// <summary>
        /// Aborts the current action.
        /// </summary>
        public void Abort()
        {
            _aborted = true;
            _abortedEventWait.Set();
        }

        private void CleanUp()
        {
            if (_outputThread != null)
            {
                _outputThread.Abort();
                _outputThread = null;
            }

            KillProcess();

            _output = null;

            if (_outputEventWait != null)
            {
                _outputEventWait.Dispose();
                _outputEventWait = null;
            }

            if (_abortedEventWait != null)
            {
                _abortedEventWait.Dispose();
                _abortedEventWait = null;
            }

            if (_exitedEventWait != null)
            {
                _exitedEventWait.Dispose();
                _exitedEventWait = null;
            }

            GC.SuppressFinalize(this);
        }

        private void KillProcess()
        {
            if (_process == null)
                return;

            try
            {
                if (_process.Id != 0 && !_process.HasExited)
                    Utils.KillByProcessId(_process.Id);
            }
            catch
            {
                // Do nothing
            }

            _process.Dispose();
            _process = null;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~GpgInterface()
        {
            CleanUp();
        }

        // internal AND protected
        internal void Log(String str, Boolean error = false)
        {
            //Console.WriteLine(str);
        }

        // internal AND protected
        internal void ResetTries()
        {
            _tries = 0;
        }

        // internal AND protected
        internal Boolean IsMaxTries()
        {
            return _tries >= PassphraseMaxTries;
        }

        // internal AND protected
        internal Boolean GNUCheck(ref String line)
        {
            if (line != null && line.StartsWith("[GNUPG:] "))
            {
                line = line.Substring(9);
                return true;
            }

            return false;
        }

        // internal AND protected
        internal GpgKeyword GetKeyword(ref String line)
        {
            String name = line;

            Int32 pos = name.IndexOf(' ');
            if (pos != -1)
                name = name.Substring(0, pos);

            GpgKeyword keyword;
            if (Enum.TryParse(name, out keyword))
            {
                line = pos == -1 ? String.Empty : line.Substring(pos + 1);
                return keyword;
            }

            return GpgKeyword.None;
        }

        // ----------------------------------------------------------------------------------------

        // internal AND protected
        internal virtual String Arguments()
        {
            return null;
        }

        // internal AND protected
        internal virtual GpgInterfaceResult BeforeStartProcess()
        {
            return GpgInterfaceResult.Success;
        }

        // internal AND protected
        internal virtual GpgInterfaceResult ProcessLine(String line)
        {
            return GpgInterfaceResult.Success;
        }

        // internal AND protected
        internal void WriteLine(Object str)
        {
            if (_process != null)
                _process.StandardInput.WriteLine(str);
        }

        // ----------------------------------------------------------------------------------------

        private Process GetProcess(String arguments)
        {
            arguments = (arguments.Contains("--status-fd=") ? "" : "--status-fd=1 ")
                      + "--command-fd=0 --no-verbose --no-greeting --no-secmem-warning --no-tty --display-charset utf8 "
                      + arguments;

            ProcessStartInfo info = new ProcessStartInfo
            {
                FileName = GpgInterface.ExePath,
                Arguments = arguments,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true
            };

            Process process = new Process { StartInfo = info };
            process.ErrorDataReceived += ErrorDataReceived;

            return process;
        }

        // internal AND protected
        internal String EncodeString(String str)
        {
            return Encoding.Default.GetString(Encoding.UTF8.GetBytes(str));
        }

        private void OutputReader()
        {
            Boolean isempty = false;
            MemoryStream stream = new MemoryStream();
            Byte[] bytes = new Byte[1024];
            Byte last = 0;

            while (!_processExited || !isempty)
            {
                Int32 count = _process.StandardOutput.BaseStream.Read(bytes, 0, bytes.Length);
                if (count > 0)
                {
                    for (Int32 i = 0; i < count; ++i)
                    {
                        isempty = false;

                        Byte b = bytes[i];

                        if (b == 10)
                            stream.WriteByte(b);

                        if (b == 10 || last == 13)
                        {
                            _output.Enqueue(new GpgOutput(MemoryStreamToBytesArray(stream)));
                            _outputEventWait.Set();
                            stream.Position = 0;
                            stream.SetLength(0);
                        }

                        if (b != 10)
                            stream.WriteByte(b);

                        last = b;
                    }
                }
                else
                {
                    isempty = true;

                    if (last == 10)
                    {
                        _output.Enqueue(new GpgOutput(MemoryStreamToBytesArray(stream)));
                        _outputEventWait.Set();
                        stream.Position = 0;
                        stream.SetLength(0);
                        last = 0;
                    }
                }
            }

            if (stream.Length > 0)
            {
                _output.Enqueue(new GpgOutput(MemoryStreamToBytesArray(stream)));
                _outputEventWait.Set();
            }
        }

        private static Byte[] MemoryStreamToBytesArray(MemoryStream stream)
        {
            stream.SetLength(stream.Position);
            return stream.ToArray();
        }

        private void ErrorDataReceived(Object sender, DataReceivedEventArgs e)
        {
            Log(e.Data, true);
        }
    }
}
