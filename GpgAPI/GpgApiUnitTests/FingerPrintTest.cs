using System;
using GpgApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GpgApiUnitTests
{
    [TestClass]
    public class FingerPrintTest
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
            Assert.IsFalse(FingerPrint.IsValid(null));
            Assert.IsFalse(FingerPrint.IsValid(String.Empty));
            Assert.IsFalse(FingerPrint.IsValid("unit test"));
            Assert.IsTrue(FingerPrint.IsValid("123465ABCDE"));
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        [ExpectedException(typeof(InvalidFingerPrintException))]
        public void FingerPrintConstructorTest1()
        {
            new FingerPrint(null);
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        [ExpectedException(typeof(InvalidFingerPrintException))]
        public void FingerPrintConstructorTest2()
        {
            new FingerPrint(String.Empty);
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        [ExpectedException(typeof(InvalidFingerPrintException))]
        public void FingerPrintConstructorTest3()
        {
            new FingerPrint("unit test");
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void FingerPrintConstructorTest4()
        {
            new FingerPrint("123465ABCDE");
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void EqualsTest()
        {
            Assert.IsFalse(new FingerPrint("123465ABCDE").Equals(null));
            Assert.IsFalse(new FingerPrint("123465ABCDE1").Equals(new FingerPrint("123465ABCDE2")));
            Assert.IsTrue(new FingerPrint("123465ABCDE").Equals(new FingerPrint("123465ABCDE")));
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void StaticEqualsTest()
        {
            Assert.IsTrue(FingerPrint.Equals(null, null));
            Assert.IsFalse(FingerPrint.Equals(new FingerPrint("123465ABCDE"), null));
            Assert.IsFalse(FingerPrint.Equals(null, new FingerPrint("123465ABCDE")));
            Assert.IsFalse(FingerPrint.Equals(new FingerPrint("123465ABCDE1"), new FingerPrint("123465ABCDE2")));
            Assert.IsTrue(FingerPrint.Equals(new FingerPrint("123465ABCDE"), new FingerPrint("123465ABCDE")));
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void ToStringTest()
        {
            Assert.AreEqual("123465ABCDE", new FingerPrint("123465ABCDE").ToString());
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void op_EqualityTest()
        {
            Assert.IsTrue(((FingerPrint)null) == ((FingerPrint)null));
            Assert.IsFalse((new FingerPrint("123465ABCDE")) == ((FingerPrint)null));
            Assert.IsFalse(((FingerPrint)null) == (new FingerPrint("123465ABCDE")));
            Assert.IsFalse((new FingerPrint("123465ABCDE1")) == (new FingerPrint("123465ABCDE2")));
            Assert.IsTrue((new FingerPrint("123465ABCDE")) == (new FingerPrint("123465ABCDE")));
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void op_InequalityTest()
        {
            Assert.IsFalse(((FingerPrint)null) != ((FingerPrint)null));
            Assert.IsTrue((new FingerPrint("123465ABCDE")) != ((FingerPrint)null));
            Assert.IsTrue(((FingerPrint)null) != (new FingerPrint("123465ABCDE")));
            Assert.IsTrue((new FingerPrint("123465ABCDE1")) != (new FingerPrint("123465ABCDE2")));
            Assert.IsFalse((new FingerPrint("123465ABCDE")) != (new FingerPrint("123465ABCDE")));
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void op_ImplicitTest()
        {
            Assert.AreEqual("123465ABCDE", new FingerPrint("123465ABCDE"));
        }
    }
}
