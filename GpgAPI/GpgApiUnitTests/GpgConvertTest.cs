using GpgApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GpgApiUnitTests
{
    [TestClass]
    public class GpgConvertTest
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestMethod]
        [DeploymentItem("GpgApi.dll")]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void ToCipherAlgorithmTest()
        {
            Assert.AreEqual(CipherAlgorithm.None, GpgConvert_Accessor.ToCipherAlgorithm(1));
            Assert.AreEqual(CipherAlgorithm.ThreeDes, GpgConvert_Accessor.ToCipherAlgorithm(2));
            Assert.AreEqual(CipherAlgorithm.Cast5, GpgConvert_Accessor.ToCipherAlgorithm(3));
            Assert.AreEqual(CipherAlgorithm.BlowFish, GpgConvert_Accessor.ToCipherAlgorithm(4));
            Assert.AreEqual(CipherAlgorithm.Aes, GpgConvert_Accessor.ToCipherAlgorithm(7));
            Assert.AreEqual(CipherAlgorithm.Aes192, GpgConvert_Accessor.ToCipherAlgorithm(8));
            Assert.AreEqual(CipherAlgorithm.Aes256, GpgConvert_Accessor.ToCipherAlgorithm(9));
            Assert.AreEqual(CipherAlgorithm.TwoFish, GpgConvert_Accessor.ToCipherAlgorithm(10));
            Assert.AreEqual(CipherAlgorithm.Camellia128, GpgConvert_Accessor.ToCipherAlgorithm(11));
            Assert.AreEqual(CipherAlgorithm.Camellia192, GpgConvert_Accessor.ToCipherAlgorithm(12));
            Assert.AreEqual(CipherAlgorithm.Camellia256, GpgConvert_Accessor.ToCipherAlgorithm(13));
        }

        [TestMethod]
        [DeploymentItem("GpgApi.dll")]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void ToDigestAlgorithmTest()
        {
            Assert.AreEqual(DigestAlgorithm.MD5, GpgConvert_Accessor.ToDigestAlgorithm(1));
            Assert.AreEqual(DigestAlgorithm.Sha1, GpgConvert_Accessor.ToDigestAlgorithm(2));
            Assert.AreEqual(DigestAlgorithm.Rmd160, GpgConvert_Accessor.ToDigestAlgorithm(3));
            Assert.AreEqual(DigestAlgorithm.Sha256, GpgConvert_Accessor.ToDigestAlgorithm(8));
            Assert.AreEqual(DigestAlgorithm.Sha384, GpgConvert_Accessor.ToDigestAlgorithm(9));
            Assert.AreEqual(DigestAlgorithm.Sha512, GpgConvert_Accessor.ToDigestAlgorithm(10));
            Assert.AreEqual(DigestAlgorithm.Sha224, GpgConvert_Accessor.ToDigestAlgorithm(11));
        }

        [TestMethod]
        [DeploymentItem("GpgApi.dll")]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void ToCompressionAlgorithmTest()
        {
            Assert.AreEqual(CompressionAlgorithm.Zip, GpgConvert_Accessor.ToCompressionAlgorithm(1));
            Assert.AreEqual(CompressionAlgorithm.ZLib, GpgConvert_Accessor.ToCompressionAlgorithm(2));
            Assert.AreEqual(CompressionAlgorithm.BZip2, GpgConvert_Accessor.ToCompressionAlgorithm(3));
        }

        [TestMethod]
        [DeploymentItem("GpgApi.dll")]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void ToKeyAlgorithmTest()
        {
            Assert.AreEqual(KeyAlgorithm.RsaRsa, GpgConvert_Accessor.ToKeyAlgorithm(1));
            Assert.AreEqual(KeyAlgorithm.RsaEncrypt, GpgConvert_Accessor.ToKeyAlgorithm(2));
            Assert.AreEqual(KeyAlgorithm.RsaSign, GpgConvert_Accessor.ToKeyAlgorithm(3));
            Assert.AreEqual(KeyAlgorithm.ELGamal, GpgConvert_Accessor.ToKeyAlgorithm(16));
            Assert.AreEqual(KeyAlgorithm.Dsa, GpgConvert_Accessor.ToKeyAlgorithm(17));
            Assert.AreEqual(KeyAlgorithm.DsaELGamal, GpgConvert_Accessor.ToKeyAlgorithm(20));
        }

        [TestMethod]
        [DeploymentItem("GpgApi.dll")]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void ToTrustTest()
        {
            Assert.AreEqual(KeyTrust.Unknown, GpgConvert_Accessor.ToTrust('o'));
            Assert.AreEqual(KeyTrust.Invalid, GpgConvert_Accessor.ToTrust('i'));
            Assert.AreEqual(KeyTrust.Revoked, GpgConvert_Accessor.ToTrust('r'));
            Assert.AreEqual(KeyTrust.Expired, GpgConvert_Accessor.ToTrust('e'));
            Assert.AreEqual(KeyTrust.Undefined, GpgConvert_Accessor.ToTrust('q'));
            Assert.AreEqual(KeyTrust.None, GpgConvert_Accessor.ToTrust('n'));
            Assert.AreEqual(KeyTrust.Marginal, GpgConvert_Accessor.ToTrust('m'));
            Assert.AreEqual(KeyTrust.Full, GpgConvert_Accessor.ToTrust('f'));
            Assert.AreEqual(KeyTrust.Ultimate, GpgConvert_Accessor.ToTrust('u'));
        }

        [TestMethod]
        [DeploymentItem("GpgApi.dll")]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void ToOwnerTrustTest()
        {
            Assert.AreEqual(KeyOwnerTrust.None, GpgConvert_Accessor.ToOwnerTrust('n'));
            Assert.AreEqual(KeyOwnerTrust.Marginal, GpgConvert_Accessor.ToOwnerTrust('m'));
            Assert.AreEqual(KeyOwnerTrust.Ultimate, GpgConvert_Accessor.ToOwnerTrust('u'));
            Assert.AreEqual(KeyOwnerTrust.Full, GpgConvert_Accessor.ToOwnerTrust('f'));
        }

        [TestMethod]
        [DeploymentItem("GpgApi.dll")]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void ToIdTest1()
        {
            Assert.AreEqual(1, GpgConvert_Accessor.ToId(KeyAlgorithm.RsaRsa));
            Assert.AreEqual(2, GpgConvert_Accessor.ToId(KeyAlgorithm.DsaELGamal));
            Assert.AreEqual(3, GpgConvert_Accessor.ToId(KeyAlgorithm.Dsa));
            Assert.AreEqual(4, GpgConvert_Accessor.ToId(KeyAlgorithm.RsaSign));
            Assert.AreEqual(5, GpgConvert_Accessor.ToId(KeyAlgorithm.ELGamal));
            Assert.AreEqual(6, GpgConvert_Accessor.ToId(KeyAlgorithm.RsaEncrypt));
        }

        [TestMethod]
        [DeploymentItem("GpgApi.dll")]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void ToIdTest2()
        {
            Assert.AreEqual(2, GpgConvert_Accessor.ToId(KeyOwnerTrust.None));
            Assert.AreEqual(3, GpgConvert_Accessor.ToId(KeyOwnerTrust.Marginal));
            Assert.AreEqual(4, GpgConvert_Accessor.ToId(KeyOwnerTrust.Full));
            Assert.AreEqual(5, GpgConvert_Accessor.ToId(KeyOwnerTrust.Ultimate));
        }

        [TestMethod]
        [DeploymentItem("GpgApi.dll")]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void ToNameTest()
        {
            Assert.AreEqual("3DES", GpgConvert_Accessor.ToName(CipherAlgorithm.ThreeDes));
            Assert.AreEqual("CAST5", GpgConvert_Accessor.ToName(CipherAlgorithm.Cast5));
            Assert.AreEqual("BLOWFISH", GpgConvert_Accessor.ToName(CipherAlgorithm.BlowFish));
            Assert.AreEqual("AES", GpgConvert_Accessor.ToName(CipherAlgorithm.Aes));
            Assert.AreEqual("AES192", GpgConvert_Accessor.ToName(CipherAlgorithm.Aes192));
            Assert.AreEqual("AES256", GpgConvert_Accessor.ToName(CipherAlgorithm.Aes256));
            Assert.AreEqual("TWOFISH", GpgConvert_Accessor.ToName(CipherAlgorithm.TwoFish));
            Assert.AreEqual("CAMELLIA128", GpgConvert_Accessor.ToName(CipherAlgorithm.Camellia128));
            Assert.AreEqual("CAMELLIA192", GpgConvert_Accessor.ToName(CipherAlgorithm.Camellia192));
            Assert.AreEqual("CAMELLIA256", GpgConvert_Accessor.ToName(CipherAlgorithm.Camellia256));
        }

        [TestMethod]
        [DeploymentItem("GpgApi.dll")]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void ToDateTest()
        {
            // TODO
        }

        [TestMethod]
        [DeploymentItem("GpgApi.dll")]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void ToDaysTest()
        {
            // TODO
        }
    }
}
