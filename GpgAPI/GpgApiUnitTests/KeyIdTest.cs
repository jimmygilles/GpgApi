using System;
using GpgApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GpgApiUnitTests
{
    [TestClass]
    public class KeyIdTest
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void IsValidTest()
        {
            Assert.IsFalse(KeyId.IsValid(null));
            Assert.IsFalse(KeyId.IsValid(String.Empty));
            Assert.IsFalse(KeyId.IsValid("unit test"));
            Assert.IsTrue(KeyId.IsValid("0123456789ABCDEF"));
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        [ExpectedException(typeof(InvalidKeyIdException))]
        public void KeyIdConstructorTest1()
        {
            new KeyId(null);
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        [ExpectedException(typeof(InvalidKeyIdException))]
        public void KeyIdConstructorTest2()
        {
            new KeyId(String.Empty);
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        [ExpectedException(typeof(InvalidKeyIdException))]
        public void KeyIdConstructorTest3()
        {
            new KeyId("i am a test");
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void KeyIdConstructorTest4()
        {
            new KeyId("0123456789ABCDEF");
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void EqualsTest()
        {
            Assert.IsFalse(new KeyId("0123456789ABCDEF").Equals(null));
            Assert.IsFalse(new KeyId("A123456789ABCDEF").Equals(new KeyId("B123456789ABCDEF")));
            Assert.IsTrue(new KeyId("0123456789ABCDEF").Equals(new KeyId("0123456789ABCDEF")));
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void StaticEqualsTest()
        {
            Assert.IsTrue(KeyId.Equals(null, null));
            Assert.IsFalse(KeyId.Equals(new KeyId("0123456789ABCDEF"), null));
            Assert.IsFalse(KeyId.Equals(null, new KeyId("0123456789ABCDEF")));
            Assert.IsFalse(KeyId.Equals(new KeyId("A123456789ABCDEF"), new KeyId("B123456789ABCDEF")));
            Assert.IsTrue(KeyId.Equals(new KeyId("0123456789ABCDEF"), new KeyId("0123456789ABCDEF")));
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void ToStringTest()
        {
            Assert.AreEqual("0123456789ABCDEF", new KeyId("0123456789ABCDEF").ToString());
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void op_EqualityTest()
        {
            Assert.IsTrue(((KeyId)null) == ((KeyId)null));
            Assert.IsFalse((new KeyId("0123456789ABCDEF")) == ((KeyId)null));
            Assert.IsFalse(((KeyId)null) == (new KeyId("0123456789ABCDEF")));
            Assert.IsFalse((new KeyId("A123456789ABCDEF")) == (new KeyId("B123456789ABCDEF")));
            Assert.IsTrue((new KeyId("0123456789ABCDEF")) == (new KeyId("0123456789ABCDEF")));
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void op_InequalityTest()
        {
            Assert.IsFalse(((KeyId)null) != ((KeyId)null));
            Assert.IsTrue((new KeyId("0123456789ABCDEF")) != ((KeyId)null));
            Assert.IsTrue(((KeyId)null) != (new KeyId("0123456789ABCDEF")));
            Assert.IsTrue((new KeyId("A123456789ABCDEF")) != (new KeyId("B123456789ABCDEF")));
            Assert.IsFalse((new KeyId("0123456789ABCDEF")) != (new KeyId("0123456789ABCDEF")));
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void op_ImplicitTest()
        {
            Assert.AreEqual("0123456789ABCDEF", new KeyId("0123456789ABCDEF"));
        }
    }
}
