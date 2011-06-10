using System;
using GpgApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GpgApiUnitTests
{
    [TestClass]
    public class NameTest
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
            Assert.IsFalse(Name.IsValid(null));
            Assert.IsFalse(Name.IsValid(String.Empty));
            Assert.IsFalse(Name.IsValid("i am a <test>"));
            Assert.IsTrue(Name.IsValid("Unit Test"));
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        [ExpectedException(typeof(InvalidNameException))]
        public void NameConstructorTest1()
        {
            new Name(null);
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        [ExpectedException(typeof(InvalidNameException))]
        public void NameConstructorTest2()
        {
            new Name(String.Empty);
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        [ExpectedException(typeof(InvalidNameException))]
        public void NameConstructorTest3()
        {
            new Name("i am a <test>");
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void NameConstructorTest4()
        {
            new Name("Unit Test");
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void EqualsTest()
        {
            Assert.IsFalse(new Name("Unit Test").Equals(null));
            Assert.IsFalse(new Name("Unit Test 1").Equals(new Name("Unit Test 2")));
            Assert.IsTrue(new Name("Unit Test").Equals(new Name("Unit Test")));
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void StaticEqualsTest()
        {
            Assert.IsTrue(Name.Equals(null, null));
            Assert.IsFalse(Name.Equals(new Name("Unit Test"), null));
            Assert.IsFalse(Name.Equals(null, new Name("Unit Test")));
            Assert.IsFalse(Name.Equals(new Name("Unit Test 1"), new Name("Unit Test 2")));
            Assert.IsTrue(Name.Equals(new Name("Unit Test"), new Name("Unit Test")));
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void ToStringTest()
        {
            Assert.AreEqual("Unit Test", new Name("Unit Test").ToString());
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void op_EqualityTest()
        {
            Assert.IsTrue(((Name)null) == ((Name)null));
            Assert.IsFalse((new Name("Unit Test")) == ((Name)null));
            Assert.IsFalse(((Name)null) == (new Name("Unit Test")));
            Assert.IsFalse((new Name("Unit Test 1")) == (new Name("Unit Test 2")));
            Assert.IsTrue((new Name("Unit Test")) == (new Name("Unit Test")));
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void op_InequalityTest()
        {
            Assert.IsFalse(((Name)null) != ((Name)null));
            Assert.IsTrue((new Name("Unit Test")) != ((Name)null));
            Assert.IsTrue(((Name)null) != (new Name("Unit Test")));
            Assert.IsTrue((new Name("Unit Test 1")) != (new Name("Unit Test 2")));
            Assert.IsFalse((new Name("Unit Test")) != (new Name("Unit Test")));
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void op_ImplicitTest()
        {
            Assert.AreEqual("Unit Test", new Name("Unit Test"));
        }
    }
}
