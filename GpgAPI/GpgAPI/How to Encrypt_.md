{code:c#}
using System;
using System.Collections.Generic;
using System.Security;
using GpgApi;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[]() args)
        {
            GpgInterface.ExePath = "path_to_gpg.exe";

            String fileName = "Path_to_file_to_encrypt";
            String encryptedFileName = "destination_path";

            // Produces a base64 file (can be opened in notepad for instance)
            Boolean armored = true;

            // Used only with asymetric algorithms (RSA)
            Boolean hideUserId = false;

            // If you want to sign the document with a sign key.
            KeyId keySignatureId = null;

            // If you want to encrypt using asymetric key, this is the list
            // of the public keys of the persons you want to encrypt to.
            List<KeyId> recipients = null;

            // The algorithm you want to use.
            CipherAlgorithm algorithm = CipherAlgorithm.Aes256;

            GpgEncrypt encrypt = new GpgEncrypt(fileName, encryptedFileName, armored, hideUserId, keySignatureId, recipients, algorithm);
            encrypt.AskPassphrase = GetPassword;

            {
                // The current thread is blocked until the encryption is finished.
                GpgInterfaceResult result = encrypt.Execute();
                Callback(result);
            }
            // or
            {
                encrypt.ExecuteAsync(Callback);
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