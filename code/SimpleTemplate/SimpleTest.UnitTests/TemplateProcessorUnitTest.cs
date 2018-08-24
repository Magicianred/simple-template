using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace SimpleTest.UnitTests
{
    [TestClass]
    public class TemplateProcessorUnitTest
    {
        private JObject _Data = JObject.Parse("{'ID':1,'Operation':'store changed','OldStore':{'Stores':['Lambton Quay','Willis Street'],'Manufacturers':[{'Name':'Acme Co','Products':[{'Name':'Anvil','Price':50}]},{'Name':'Contoso','Products':[{'Name':'Elbow Grease','Price':99.95},{'Name':'Headlight Fluid','Price':4},{'Name':'Milk','Price':2}]}]},'NewStore':{'Stores':['Lambton Quay','Willis Street','Shobra Street'],'Manufacturers':[{'Name':'Acme Company','Products':[{'Name':'Anvil','Price':50},{'Name':'Beans','Price':10,'Available':true}]},{'Name':'Contoso','Products':[{'Name':'Elbow Grease','Price':99.95}]}]}}");

        [TestMethod]
        public void TestNoFunction()
        {
            string template = "An event of {{Operation}} submitted.";
            
            string expected = "An event of store changed submitted.";

            SimpleTemplate.Business.TemplateProcesser processer = new SimpleTemplate.Business.TemplateProcesser(template, _Data);

            Assert.AreEqual(expected, processer.Result);
        }

        [TestMethod]
        public void TestMapFunction()
        {
            string template = "An event of {{$Map$Operation}} submitted.";
            Hashtable mapping = new Hashtable
            {
                { "store changed", "store modified" }
            };

            string expected = "An event of store modified submitted.";

            SimpleTemplate.Business.TemplateProcesser processer = new SimpleTemplate.Business.TemplateProcesser(template, _Data, mapping);

            Assert.AreEqual(expected, processer.Result);
        }

        [TestMethod]
        public void TestCompareFunction()
        {
            string template = "An event of {{Operation}} AKA {{$Map$Operation}} submitted. The changes are the following: \r\n {{$Compare$OldStore,NewStore}}";
            Hashtable mapping = new Hashtable
            {
                { "store changed", "store modified" }
            };

            string expected = "An event of store modified submitted.";

            SimpleTemplate.Business.TemplateProcesser processer = new SimpleTemplate.Business.TemplateProcesser(template, _Data, mapping);

            Assert.AreEqual(expected, processer.Result);
        }
    }
}
