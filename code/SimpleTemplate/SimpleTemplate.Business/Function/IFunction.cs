using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTemplate.Business.Function
{
    interface IFunction
    {
        Model.Variable InputVariable { get; set; }
        JObject Data { get; set; }

        string CalculateValue();
    }
}
