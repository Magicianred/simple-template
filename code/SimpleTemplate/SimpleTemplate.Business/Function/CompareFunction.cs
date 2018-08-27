using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SimpleTemplate.Model;

namespace SimpleTemplate.Business.Function
{
    internal class CompareFunction : IFunction
    {
        public Model.Variable InputVariable { get; set; }
        public JObject Data { get; set; }

        public CompareFunction(Model.Variable inputVariable, JObject data)
        {
            this.Data = data;
            this.InputVariable = inputVariable;
        }

        public string CalculateValue()
        {
            if(this.InputVariable == null || this.InputVariable.FunctionArguments == null || this.InputVariable.FunctionArguments.Count < 2)
            {
                return string.Empty;
            }

            JsonDiffPatchDotNet.JsonDiffPatch jdp = new JsonDiffPatchDotNet.JsonDiffPatch();
            var left = Helper.JsonHelper.Fetch(this.Data, this.InputVariable.FunctionArguments[0]);
            var right = Helper.JsonHelper.Fetch(this.Data, this.InputVariable.FunctionArguments[1]);

            JToken patch = jdp.Diff(left, right);

            return Helper.JsonHelper.DescribeJsonComparison(patch);;
        }
    }
}
