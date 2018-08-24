using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTemplate.Business.Helper
{
    public class JsonFetch
    {
        public static JToken Fetch(JObject data, string key)
        {
            return data.SelectToken("$." + key);
        }
    }
}
