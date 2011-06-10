using System;
using System.Collections.Generic;
using GpgApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GpgApiUnitTests
{
    [TestClass]
    public class GpgProcesses
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [ClassInitialize]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public static void MyClassInitialize(TestContext testContext)
        {
            GpgInterface.ExePath = TestCore.GpgPath;
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void Process1()
        {
            DateTime expiration_date = new DateTime(2050, 01, 01);
            Name name = new Name("Unit Test");
            Email email = new Email("unittest@unittest.com");
            String comment = "Unit Test Comment";

            GpgGenerateKey generatekey = new GpgGenerateKey(name, email, comment, KeyAlgorithm.RSA_RSA, 1024, expiration_date);
            generatekey.AskPassphrase = TestCore.AskPassphrase;
            generatekey.Execute();
            Assert.IsNotNull(generatekey.FingerPrint);

            GpgListPublicKeys keys = new GpgListPublicKeys();
            keys.Execute();

            Assert.IsNotNull(keys.Keys);
            Assert.AreNotEqual(0, keys.Keys.Count);

            Key key = null;
            foreach (Key item in keys.Keys)
            {
                if (item.FingerPrint == generatekey.FingerPrint)
                {
                    key = item;
                    break;
                }
            }

            Assert.IsNotNull(key);

            GpgDeleteKeys delete = new GpgDeleteKeys(new List<KeyId> { key.Id }, false);
            delete.Execute();

            DateTime ed = key.ExpirationDate.DateTime;
            Boolean isInRange = ed > expiration_date && ed < expiration_date.AddDays(1);

            Assert.AreEqual(KeyAlgorithm.RSA_RSA, key.Algorithm);
            Assert.AreEqual(true, isInRange);
            Assert.AreEqual(generatekey.FingerPrint, key.FingerPrint);
            Assert.IsFalse(key.IsDisabled);
            Assert.AreEqual(KeyOwnerTrust.Ultimate, key.OwnerTrust);
            Assert.AreEqual((UInt32)1024, key.Size);
            Assert.AreEqual(KeyType.Public, key.Type);
            Assert.IsNotNull(key.UserInfos);
            Assert.AreNotEqual(0, key.UserInfos.Count);
            Assert.AreEqual(name, key.UserInfos[0].Name);
            Assert.AreEqual(email, key.UserInfos[0].Email);
            Assert.AreEqual(comment, key.UserInfos[0].Comment);
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void Process2()
        {
            GpgVersion version = new GpgVersion();
            version.Execute();

            Version testVersion = new Version("1.4.10");
            Assert.IsTrue(testVersion.CompareTo(version.Version) <= 0);
        }
    }
}
