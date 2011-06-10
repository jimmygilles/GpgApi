using System;
using GpgApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GpgApiUnitTests
{
    [TestClass]
    public class UtilsTest
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
        public void IsValidPathTest()
        {
            Assert.IsFalse(Utils_Accessor.IsValidPath(null));
            Assert.IsFalse(Utils_Accessor.IsValidPath(String.Empty));
            Assert.IsTrue(Utils_Accessor.IsValidPath("c:\\test.txt"));
        }

        [TestMethod]
        [DeploymentItem("GpgApi.dll")]
        [Owner("Jimmy Gilles (jimmygilles@gmail.com)")]
        public void SplitUserInfoTest()
        {
            Name name = new Name("Unit Test");
            Email email = new Email("unittest@unittest.com");
            String comment = "Unit Test comment";

            String line = "Unit Test (Unit Test comment) <unittest@unittest.com>";

            Name line_name = null;
            Email line_email = null;
            String line_comment = null;

            Utils_Accessor.SplitUserInfo(line, out line_name, out line_email, out line_comment);

            Assert.AreEqual(name, line_name);
            Assert.AreEqual(email, line_email);
            Assert.AreEqual(comment, line_comment);
        }
    }
}
