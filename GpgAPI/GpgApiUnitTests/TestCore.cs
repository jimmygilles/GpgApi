using System;
using GpgApi;

namespace GpgApiUnitTests
{
    public static class TestCore
    {
        public static String GpgPath
        {
            get { return @"C:\Program Files (x86)\GNU\GnuPG\gpg.exe"; }
        }

        public static String AskPassphrase(AskPassphraseInfo info)
        {
            return "a";
        }
    }
}
