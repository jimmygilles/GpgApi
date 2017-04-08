{code:c#}
using System;
using System.Security;
using GpgApi;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[]() args)
        {
            GpgInterface.ExePath = "path_to_gpg.exe";

            String encryptedFileName = "path_to_the_encrypted_file";
            String fileName = "destination_file";

            GpgDecrypt decrypt = new GpgDecrypt(encryptedFileName, fileName);
            decrypt.AskPassphrase = GetPassword;

            {
                // The current thread is blocked until the decryption is finished.
                GpgInterfaceResult result = decrypt.Execute();
                Callback(result);
            }
            // or
            {
                decrypt.ExecuteAsync(Callback);
            }
        }

        public static SecureString GetPassword(AskPassphraseInfo info)
        {
            // Change this to return the password you want to use.
            return GpgInterface.GetSecureStringFromString("a");
        }

        public static void Callback(GpgInterfaceResult result)
        {
            if (result.Status == GpgInterfaceStatus.Success)
            {
                // ...
            }
            else
            {
                // ...
            }
        }
    }
}
{code:c#}
