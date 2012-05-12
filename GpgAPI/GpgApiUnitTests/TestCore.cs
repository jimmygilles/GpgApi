using System;
using GpgApi;
using System.Security;

namespace GpgApiUnitTests
{
    public static class TestCore
    {
        public static String GpgPath
        {
            get { return @"C:\Program Files (x86)\GNU\GnuPG\gpg.exe"; }
        }

        public static SecureString AskPassphrase(AskPassphraseInfo info)
        {
            return GpgInterface.GetSecureStringFromString("a");
        }
    }
}
