using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleTest.UnitTests
{
    [TestClass]
    public class ModelUnitTest
    {
        [TestMethod]
        public void TestNormal()
        {
            SimpleTemplate.Business.Model.Variable v = new SimpleTemplate.Business.Model.Variable() { Input = "UserID" };

            Assert.AreEqual(v.IsFunction, false);
            Assert.AreEqual(v.FunctionName, string.Empty);
            Assert.AreEqual(v.FunctionArguments.Count, 0);
            Assert.AreEqual(v.Name, v.Input);
        }

        [TestMethod]
        public void TestFunction1()
        {
            SimpleTemplate.Business.Model.Variable v = new SimpleTemplate.Business.Model.Variable() { Input = "$Compare$OldDiscount,NewDiscount" };
            List<string> expectedArguments = new List<string>() { "OldDiscount", "NewDiscount" };

            Assert.AreEqual(v.IsFunction, true);
            Assert.AreEqual(v.FunctionName, "Compare");
            Assert.AreEqual(v.FunctionArguments.Count, 2);
            Assert.AreEqual(v.FunctionArguments.Except(expectedArguments).Count(), 0);
            Assert.AreEqual(expectedArguments.Except(v.FunctionArguments).Count(), 0);
            Assert.AreEqual(v.Name, string.Empty);
        }

        [TestMethod]
        public void TestFunction2()
        {
            SimpleTemplate.Business.Model.Variable v = new SimpleTemplate.Business.Model.Variable() { Input = "$Compare$" };

            Assert.AreEqual(v.IsFunction, true);
            Assert.AreEqual(v.FunctionName, "Compare");
            Assert.AreEqual(v.FunctionArguments.Count, 0);
            Assert.AreEqual(v.Name, string.Empty);
        }

        [TestMethod]
        public void TestFunction3()
        {
            SimpleTemplate.Business.Model.Variable v = new SimpleTemplate.Business.Model.Variable() { Input = "$Map$UserType" };
            List<string> expectedArguments = new List<string>() { "UserType" };

            Assert.AreEqual(v.IsFunction, true);
            Assert.AreEqual(v.FunctionName, "Map");
            Assert.AreEqual(v.FunctionArguments.Count, 1);
            Assert.AreEqual(v.FunctionArguments.Except(expectedArguments).Count(), 0);
            Assert.AreEqual(expectedArguments.Except(v.FunctionArguments).Count(), 0);
            Assert.AreEqual(v.Name, string.Empty);
        }
    }
}
