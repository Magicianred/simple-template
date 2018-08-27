using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SimpleTemplate.Model;

namespace SimpleTemplate.Business.Function
{
    internal class NoFunction : IFunction
    {
        public Variable InputVariable { get; set; }
        public JObject Data { get; set; }

        public NoFunction(Model.Variable inputVariable, JObject data)
        {
            this.Data = data;
            this.InputVariable = inputVariable;
        }

        public string CalculateValue()
        {
            return Helper.JsonHelper.Fetch(this.Data, this.InputVariable.Name).ToString();
        }
    }
}
