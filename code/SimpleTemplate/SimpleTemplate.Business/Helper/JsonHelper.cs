using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTemplate.Business.Helper
{
    public class JsonHelper
    {
        private const string REMOVE_PATTERN = @"^_\d+$";
        public static JToken Fetch(JObject data, string key)
        {
            return data.SelectToken("$." + key);
        }

        public static string DescribeJsonComparison(JToken jsonResult)
        {
            List<JsonComparisonResult> compResults = new List<JsonComparisonResult>();
            getChangesItems(jsonResult, compResults);

            string result = processChangesDescription(compResults);

            return result;
        }

        private static string processChangesDescription(List<JsonComparisonResult> compResults)
        {
            StringBuilder result = new StringBuilder();
            int counter = 1;

            for (int i = 0; i < compResults.Count; i++)
            {
                JsonComparisonResult compResult = compResults.ElementAt(i);

                if(compResult.Name == "_t")
                {
                    continue;
                }
                
                //capitalize the first letter
                compResult.Description = compResult.Description.First().ToString().ToUpper() + compResult.Description.Substring(1);

                result.AppendLine($"{counter++}. |{compResult.Path}| {compResult.Description}");
            }

            return result.ToString();
        }

        private static void getChangesItems(JToken jsonResult, List<JsonComparisonResult> compResults)
        {
            if (compResults == null)
            {
                compResults = new List<JsonComparisonResult>();
            }

            foreach (JToken item in jsonResult.Values())
            {
                string name = readName(item.Path);

                if (item.Type == JTokenType.Object && item.HasValues)
                {
                    getChangesItems(item, compResults);
                }
                else if (item.Type == JTokenType.Array && item.Values().Count() == 2)
                {
                    //array with 2 values, it is modified values from old value to new value
                    compResults.Add(new JsonComparisonResult()
                    {
                        Description = $"{makeItMoreNatural(name)} changed from {item.Values().First()} to {item.Values().Last()}",
                        Name = name,
                        Path = item.Path
                    });
                }
                else if (System.Text.RegularExpressions.Regex.IsMatch(name, REMOVE_PATTERN) && item.Type == JTokenType.Array && item.Values().Count() > 2 && item.Values().ElementAt(item.Values().Count() - 1).ToString() == "0" && item.Values().ElementAt(item.Values().Count() - 2).ToString() == "0")
                {
                    //array with more than 2 values, and the last two values are zeros. It old item removed

                    string parentName = readName(item.Parent.Path);
                    if(item.Parent != null && item.Parent.Parent != null)
                    {
                        parentName = readName(item.Parent.Parent.Path);
                    }

                    StringBuilder description = new StringBuilder();
                    description.Append($"{makeItMoreNatural(parentName)} removed with the criteria: [");

                    int propCount = item.Values().Count() - 2;

                    for (int i = 0; i < propCount; i++)
                    {
                        JToken propToken = item.Values().ElementAt(i);

                        if (propToken.Type == JTokenType.Property)
                        {
                            JProperty prop = propToken as JProperty;

                            if (!string.IsNullOrEmpty(prop.Value.ToString()))
                            {
                                description.Append($"{makeItMoreNatural(prop.Name)} is {prop.Value}");
                            }
                            else
                            {
                                description.Append($"no {makeItMoreNatural(prop.Name)}");
                            }
                        }

                        if(i < propCount - 1)
                        {
                            description.Append(", ");
                        }
                    }

                    description.Append("]");

                    compResults.Add(new JsonComparisonResult()
                    {
                        Description = description.ToString(),
                        Name = parentName,
                        Path = item.Path
                    });
                }
                else
                {
                    compResults.Add(new JsonComparisonResult()
                    {
                        Description = $"new {makeItMoreNatural(name)} added with {item.ToString()}",
                        Name = name,
                        Path = item.Path
                    });
                }
            }
        }

        private static string readName(string path)
        {
            int indexOfLastPeriod = path.LastIndexOf(".");

            string name = path.Substring(indexOfLastPeriod + 1, path.Length - indexOfLastPeriod - 1);

            return name;
        }

        private static string makeItMoreNatural(string input)
        {
            string op1 = String.Copy(input);

            string op2 = System.Text.RegularExpressions.Regex.Replace(op1, @"\W+",string.Empty);
            
            bool allCapital = System.Text.RegularExpressions.Regex.IsMatch(op2, @"^[A-Z0-9]+$");

            if (allCapital)
            {
                return op2;
            }

            string result = System.Text.RegularExpressions.Regex.Replace(op2, "(\\B[A-Z])", " $1");

            result = result.ToLower();
            string[] words = result.Split(" ".ToCharArray());
            List<string> words2 = new List<string>();

            string cached = string.Empty;

            foreach (var item in words)
            {
                if(item.Length == 1)
                {
                    cached += item;
                    continue;
                }

                if (!String.IsNullOrEmpty(cached))
                {
                    words2.Add(cached.ToUpper());
                    cached = string.Empty;
                }

                words2.Add(item);
            }

            if (!String.IsNullOrEmpty(cached))
            {
                words2.Add(cached.ToUpper());
                cached = string.Empty;
            }

            result = String.Join(" ", words2.ToArray());

            #region special words handling
            if(words.Contains("is") || words.Contains("has")|| words.Contains("in"))
            {
                result = "\"" + result + "\"";
            }
            #endregion

            return result;
        }
    }
}
