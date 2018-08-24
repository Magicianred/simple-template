using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTemplate.Business.Function
{
    internal class FunctionFactory
    {
        const string FUNCTION_MAP = "map";
        const string FUNCTION_COMPARE = "compare";

        public static IFunction GetFunction(Model.Variable inputVariable, JObject data, Hashtable mapping = null)
        {
            if (inputVariable.IsFunction)
            {
                switch (inputVariable.FunctionName.ToLower())
                {
                    case FUNCTION_COMPARE:
                        return new CompareFunction(inputVariable, data);

                    case FUNCTION_MAP:
                        IFunction fn = new MapFunction(inputVariable, data);
                        ((MapFunction)fn).Mapping = mapping;
                        return fn;
                }
            }

            return new NoFunction(inputVariable, data);
        }
    }
}
