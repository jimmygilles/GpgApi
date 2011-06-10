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
using System.Collections.Generic;

namespace GpgApi
{
    /// <summary>
    /// Retrieves the list of all private keys in the user's keyring.
    /// </summary>
    public sealed class GpgListSecretKeys : GpgInterface
    {
        public IEnumerable<KeyId> Filters { get; private set; }
        public List<Key> Keys { get; private set; }

        public GpgListSecretKeys(IEnumerable<KeyId> filters)
        {
            Keys = new List<Key>();
            Filters = filters;
        }

        private Key _last = null;

        // internal AND protected
        internal override String Arguments()
        {
            String arguments = "--status-fd=2 --fixed-list-mode --with-colons --with-fingerprint --list-secret-keys";

            if (Filters != null)
                arguments += " " + String.Join(" ", Filters);

            return arguments;
        }

        // internal AND protected
        internal override GpgInterfaceResult ProcessLine(String line)
        {
            String[] parts = line.Split(':');

            switch (parts[0])
            {
                case "sec":
                {
                    Key key = new Key
                    {
                        Trust = GpgConvert.ToTrust(parts[1]),
                        Size = Convert.ToUInt32(parts[2]),
                        Algorithm = GpgConvert.ToKeyAlgorithm(parts[3]),
                        Id = new KeyId(parts[4]),
                        CreationDate = GpgConvert.ToDate(parts[5]),
                        ExpirationDate = new GpgDateTime(parts[6]),
                        Type = KeyType.Secret
                    };

                    Keys.Add(key);
                    _last = key;
                    break;
                }

                case "fpr":
                {
                    _last.FingerPrint = new FingerPrint(parts[9]);
                    break;
                }

                case "uid":
                {
                    _last.UserInfos.Add(new KeyUserInfo(Utils.UnescapeGpgString(parts[9])));
                    break;
                }
            }

            return GpgInterfaceResult.Success;
        }
    }
}
