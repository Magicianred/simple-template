using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace SimpleTest.UnitTests
{
    [TestClass]
    public class JsonFetchUnitTest
    {
        private JObject _Sample = JObject.Parse("{\"widget\": {    \"debug\": \"on\",    \"window\": {        \"title\": \"Sample onfabulator Widget\",        \"name\": \"main_window\",        \"width\": 500,        \"height\": 500    },    \"image\": {         \"src\": \"Images/Sun.png\",        \"name\": \"sun1\",        \"hOffset\": 250,        \"vOffset\": 250,        \"alignment\": \"center\"    },    \"text\": {        \"data\": \"Click Here\",        \"size\": 36,        \"style\": \"bold\",        \"name\": \"text1\",        \"hOffset\": 250,        \"vOffset\": 100,        \"alignment\": \"center\",        \"onMouseUp\": \"sun1.opacity = (sun1.opacity / 100) * 90;\"    }}}");

        [TestMethod]
        public void TestFetch1()
        {
            string key = "widget";
            string expected = "{\r\n  \"debug\": \"on\",\r\n  \"window\": {\r\n    \"title\": \"Sample onfabulator Widget\",\r\n    \"name\": \"main_window\",\r\n    \"width\": 500,\r\n    \"height\": 500\r\n  },\r\n  \"image\": {\r\n    \"src\": \"Images/Sun.png\",\r\n    \"name\": \"sun1\",\r\n    \"hOffset\": 250,\r\n    \"vOffset\": 250,\r\n    \"alignment\": \"center\"\r\n  },\r\n  \"text\": {\r\n    \"data\": \"Click Here\",\r\n    \"size\": 36,\r\n    \"style\": \"bold\",\r\n    \"name\": \"text1\",\r\n    \"hOffset\": 250,\r\n    \"vOffset\": 100,\r\n    \"alignment\": \"center\",\r\n    \"onMouseUp\": \"sun1.opacity = (sun1.opacity / 100) * 90;\"\r\n  }\r\n}";
            string actual = SimpleTemplate.Business.Helper.JsonFetch.Fetch(_Sample, key).ToString();
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void TestFetch2()
        {
            string key = "widget.debug";
            string expected = "on";
            string actual = SimpleTemplate.Business.Helper.JsonFetch.Fetch(_Sample, key).ToString();
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void TestFetch3()
        {
            string key = "widget.image.name";
            string expected = "sun1";
            string actual = SimpleTemplate.Business.Helper.JsonFetch.Fetch(_Sample, key).ToString();
            Assert.AreEqual(actual, expected);
        }
    }
}
