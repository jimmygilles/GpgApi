# How to use GpgAPI ?

* [How to Encrypt?](https://gpgapi.codeplex.com/wikipage?title=How%20to%20Encrypt%3f)
* [How to Decrypt?](https://gpgapi.codeplex.com/wikipage?title=How%20to%20Decrypt%3f)

Using GpgAPI is easy... really :)

First, you must set the path to gpg.exe:
{code:c#}
GpgInterface.ExePath = thepath;
{code:c#}

Then you can set a SynchronizationContext, this is usefull when you develop an Wpf/Xaml or winform application:
{code:c#}
GpgInterface.SynchronizationContext = SynchronizationContext.Current;
{code:c#}

Now, you can use GpgAPI.
For example, you want to retrieve all the keys managed by gpg:

{code:c#}
using System;
using GpgApi;

namespace Example
{
    public class gpgapi_example
    {
        public static void Main(String[]() args)
        {
            GpgInterface.ExePath = args[0](0);

            GpgListPublicKeys publicKeys = new GpgListPublicKeys();
            publicKeys.Execute();

            foreach (Key key in publicKeys.Keys)
            {
                Console.WriteLine(key.Id);
            }
        }
    }
}

{code:c#}

Sometimes, GpgAPI needs a password. Here is an example:

As you will see, we use the method "ExecuteAsync". ExecuteAsync is an asynchrone method. It starts a new thread.
We use the async method because the generation of a key may take minutes.
When the process is finished, the delegate method is called.

{code:c#}
using System;
using GpgApi;

namespace Example
{
    public class gpgapi_example
    {
        public static SecureString GetPassword(AskPassphraseInfo info)
        {
            // You can display a popup or anything else.
            // For the example, password "a" is returned.
            // You must return a SecureString instead of a String.

            // In you don't want to manage a SecureString,
            // you can use this convenience method :
            // GpgInterface.GetSecureStringFromString

            // The SecureString must be marked as read-only.

            return GpgInterface.GetSecureStringFromString("a");
        }

        public static void Main(String[]() args)
        {
            GpgInterface.ExePath = @"C:\Program Files (x86)\GNU\GnuPG\gpg.exe";

            Name name = new Name("My name");
            Email email = new Email("example@example.com");
            String comment = "A comment";
            KeyAlgorithm algorithm = KeyAlgorithm.RsaRsa;
            UInt32 size = 4096;
            DateTime expirationdate = DateTime.Now.AddYears(1);

            GpgGenerateKey generate = new GpgGenerateKey(name, email, comment, algorithm, size, expirationdate);
            generate.AskPassphrase = GetPassword;
            generate.ExecuteAsync(delegate(GpgInterfaceResult result)
            {
                // ...
                Console.WriteLine("Generated!");
                // ...
            });
        }
    }
}
{code:c#}
