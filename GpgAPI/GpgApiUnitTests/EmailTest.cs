using System;
using GpgApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GpgApiUnitTests
{
    [TestClass]
    public class EmailTest
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
            Assert.IsFalse(Email.IsValid(null));
            Assert.IsFalse(Email.IsValid(String.Empty));
            Assert.IsFalse(Email.IsValid("unit test"));
            Assert.IsTrue(Email.IsValid("unittest@unittest.com"));
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        [ExpectedException(typeof(InvalidEmailAddressException))]
        public void EmailConstructorTest1()
        {
            new Email(null);
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        [ExpectedException(typeof(InvalidEmailAddressException))]
        public void EmailConstructorTest2()
        {
            new Email(String.Empty);
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        [ExpectedException(typeof(InvalidEmailAddressException))]
        public void EmailConstructorTest3()
        {
            new Email("i am a test");
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void EmailConstructorTest4()
        {
            new Email("unittest@unittest.com");
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void EqualsTest()
        {
            Assert.IsFalse(new Email("unittest@unittest.com").Equals(null));
            Assert.IsFalse(new Email("unittest1@unittest.com").Equals(new Email("unittest2@unittest.com")));
            Assert.IsTrue(new Email("unittest@unittest.com").Equals(new Email("unittest@unittest.com")));
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void StaticEqualsTest()
        {
            Assert.IsTrue(Email.Equals(null, null));
            Assert.IsFalse(Email.Equals(new Email("unittest@unittest.com"), null));
            Assert.IsFalse(Email.Equals(null, new Email("unittest@unittest.com")));
            Assert.IsFalse(Email.Equals(new Email("unittest1@unittest.com"), new Email("unittest2@unittest.com")));
            Assert.IsTrue(Email.Equals(new Email("unittest@unittest.com"), new Email("unittest@unittest.com")));
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void ToStringTest()
        {
            Assert.AreEqual("unittest@unittest.com", new Email("unittest@unittest.com").ToString());
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void op_EqualityTest()
        {
            Assert.IsTrue(((Email)null) == ((Email)null));
            Assert.IsFalse((new Email("unittest@unittest.com")) == ((Email)null));
            Assert.IsFalse(((Email)null) == (new Email("unittest@unittest.com")));
            Assert.IsFalse((new Email("unittest1@unittest.com")) == (new Email("unittest2@unittest.com")));
            Assert.IsTrue((new Email("unittest@unittest.com")) == (new Email("unittest@unittest.com")));
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void op_InequalityTest()
        {
            Assert.IsFalse(((Email)null) != ((Email)null));
            Assert.IsTrue((new Email("unittest@unittest.com")) != ((Email)null));
            Assert.IsTrue(((Email)null) != (new Email("unittest@unittest.com")));
            Assert.IsTrue((new Email("unittest1@unittest.com")) != (new Email("unittest2@unittest.com")));
            Assert.IsFalse((new Email("unittest@unittest.com")) != (new Email("unittest@unittest.com")));
        }

        [TestMethod]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void op_ImplicitTest()
        {
            Assert.AreEqual("unittest@unittest.com", new Email("unittest@unittest.com"));
        }
    }
}
