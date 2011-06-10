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
using System.IO;

namespace GpgApi
{
    /// <summary>
    /// Signs a file.
    /// </summary>
    public sealed class GpgSign : GpgInterface
    {
        public KeyId SignatureKeyId { get; private set; }
        public String Filename { get; private set; }
        public String SignedFilename { get; private set; }
        public Boolean Armored { get; private set; }

        public Boolean Signed { get; private set; }
        public DigestAlgorithm DigestAlgorithm { get; private set; }
        public KeyAlgorithm KeyAlgorithm { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="signatureKeyId"></param>
        /// <param name="filename"></param>
        /// <param name="signedFilename"></param>
        /// <param name="armored"></param>
        /// <exception cref="System.ArgumentNullException"/>
        public GpgSign(KeyId signatureKeyId, String filename, String signedFilename, Boolean armored)
        {
            if (signatureKeyId == null)
                throw new ArgumentNullException("signatureKeyId");

            SignatureKeyId = signatureKeyId;
            Filename = filename;
            SignedFilename = signedFilename;
            Armored = armored;
            Signed = false;
            KeyAlgorithm = KeyAlgorithm.None;
            DigestAlgorithm = DigestAlgorithm.None;
        }

        // internal AND protected
        internal override String Arguments()
        {
            String args = "";

            args += "--output " + Utils.EscapePath(SignedFilename) + " ";
            args += Armored ? "--clearsign " : "--sign ";
            args += "--local-user " + SignatureKeyId + " ";
            args += Utils.EscapePath(Filename);

            return args;
        }

        // internal AND protected
        internal override GpgInterfaceResult BeforeStartProcess()
        {
            if (!File.Exists(Filename))
                return new GpgInterfaceResult(GpgInterfaceStatus.Error, GpgInterfaceMessage.FileNotFound, Filename);

            if (!Utils.IsValidPath(SignedFilename))
                return new GpgInterfaceResult(GpgInterfaceStatus.Error, GpgInterfaceMessage.InvalidFilename, SignedFilename);

            return GpgInterfaceResult.Success;
        }

        // internal AND protected
        internal override GpgInterfaceResult ProcessLine(String line)
        {
            if (!GNUCheck(ref line))
                return GpgInterfaceResult.Success;

            switch (GetKeyword(ref line))
            {
                case GpgKeyword.BAD_PASSPHRASE:
                {
                    if (IsMaxTries())
                        return GpgInterfaceResult.BadPassphrase;
                    break;
                }

                case GpgKeyword.GET_HIDDEN:
                {
                    if (line == "passphrase.enter")
                    {
                        String password = InternalAskPassphrase(SignatureKeyId);
                        if (String.IsNullOrEmpty(password))
                            return GpgInterfaceResult.UserAbort;
                        WriteLine(password);
                    }
                    break;
                }

                case GpgKeyword.SIG_CREATED:
                {
                    String[] parts = line.Split(' ');
                    Signed = true;
                    KeyAlgorithm = GpgConvert.ToKeyAlgorithm(Int32.Parse(parts[1]));
                    DigestAlgorithm = GpgConvert.ToDigestAlgorithm(Int32.Parse(parts[2]));
                    break;
                }

                case GpgKeyword.GET_BOOL:
                {
                    if (line == "openfile.overwrite.okay")
                        WriteLine("YES");
                    break;
                }
            }

            return GpgInterfaceResult.Success;
        }
    }
}
