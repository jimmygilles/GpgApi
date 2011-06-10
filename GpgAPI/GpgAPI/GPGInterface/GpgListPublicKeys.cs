﻿#region License
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
    /// Retrieves the list of all public keys in the user's keyring.
    /// </summary>
    public sealed class GpgListPublicKeys : GpgInterface
    {
        public IEnumerable<KeyId> Filters { get; private set; }
        public List<Key> Keys { get; private set; }

        private UInt32 _index = 1;
        private Key _lastKey = null;
        private AbstractKeySignable _lastKeyNode = null;

        public GpgListPublicKeys(IEnumerable<KeyId> filters = null)
        {
            Keys = new List<Key>();
            Filters = filters;
        }

        // internal AND protected
        internal override String Arguments()
        {
            String arguments = "--status-fd=2 --fixed-list-mode --with-colons --with-fingerprint --list-sigs";

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
                case "pub":
                {
                    Key key = new Key
                    {
                        Trust = GpgConvert.ToTrust(parts[1]),
                        Size = Convert.ToUInt32(parts[2]),
                        Algorithm = GpgConvert.ToKeyAlgorithm(parts[3]),
                        Id = new KeyId(parts[4]),
                        CreationDate = GpgConvert.ToDate(parts[5]),
                        ExpirationDate = new GpgDateTime(parts[6]),
                        OwnerTrust = GpgConvert.ToOwnerTrust(parts[8]),
                        Type = KeyType.Public
                    };

                    if (parts.Length > 11)
                    {
                        String p = parts[11];
                        key.IsDisabled = p.Contains("D");
                    }

                    Keys.Add(key);
                    _lastKey = key;
                    _index = 1;
                    break;
                }

                case "fpr":
                {
                    _lastKey.FingerPrint = new FingerPrint(parts[9]);
                    break;
                }
                    
                case "sub":
                {
                    KeySub sub = new KeySub
                    {
                        Trust = GpgConvert.ToTrust(parts[1]),
                        Size = Convert.ToUInt32(parts[2]),
                        Algorithm = GpgConvert.ToKeyAlgorithm(parts[3]),
                        Id = new KeyId(parts[4]),
                        CreationDate = GpgConvert.ToDate(parts[5]),
                        ExpirationDate = new GpgDateTime(parts[6])
                    };

                    if (parts.Length > 11)
                    {
                        String p = parts[11];
                        sub.IsDisabled = p.Contains("D");
                    }

                    _lastKey.SubKeys.Add(sub);
                    _lastKeyNode = sub;
                    break;
                }

                case "uat":
                {
                    if (parts.Length >= 6)
                    {
                        KeyPhoto photo = new KeyPhoto
                        {
                            Index = _index++
                        };

                        _lastKey.Photos.Add(photo);
                        _lastKeyNode = photo;
                    }
                    break;
                }

                case "uid":
                {
                    KeyUserInfo userid = new KeyUserInfo(Utils.UnescapeGpgString(parts[9]))
                    {
                        Index = _index++
                    };

                    _lastKey.UserInfos.Add(userid);
                    _lastKeyNode = userid;
                    break;
                }

                case "sig":
                case "rev":
                {
                    if (_lastKeyNode != null)
                        _lastKeyNode.Signatures.Add(new KeySignature(parts[4], GpgConvert.ToDate(parts[5]), parts[0] == "rev"));
                    break;
                }
            }

            return GpgInterfaceResult.Success;
        }
    }
}
