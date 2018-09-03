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
        private const string STANDARD_PATTERN = @"^\d+$";
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

            result.AppendLine();

            for (int i = 0; i < compResults.Count; i++)
            {
                JsonComparisonResult compResult = compResults.ElementAt(i);

                if(compResult.Name == "_t")
                {
                    continue;
                }
                
                //capitalize the first letter
                compResult.Description = compResult.Description.First().ToString().ToUpper() + compResult.Description.Substring(1);

                //result.AppendLine($"{counter++}. |{compResult.Path}| {compResult.Description}");
                result.AppendLine($"{counter++}. {compResult.Description}");
            }

            result.AppendLine();

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
                    bool isRemoveAddScenario = getChangesItemsB(item, compResults);

                    if (!isRemoveAddScenario)
                    {
                        getChangesItems(item, compResults);
                    }
                }
                else if (item.Type == JTokenType.Array && ((JArray)item).Count == 2)
                {
                    //array with 2 values, it is modified values from old value to new value

                    string description = string.Empty;
                    string parentName = readName(item.Parent.Path);
                    if (item.Parent.Parent != null)
                    {
                        parentName = readName(item.Parent.Parent.Path);
                    }

                    bool invalidParentName = System.Text.RegularExpressions.Regex.IsMatch(parentName, REMOVE_PATTERN) || System.Text.RegularExpressions.Regex.IsMatch(parentName, STANDARD_PATTERN);

                    if (invalidParentName && item.Parent.Parent.Parent != null)
                    {
                        parentName = readName(item.Parent.Parent.Parent.Path);
                    }

                    if (invalidParentName && item.Parent.Parent.Parent.Parent != null)
                    {
                        parentName = readName(item.Parent.Parent.Parent.Parent.Path);
                    }

                    if (parentName != string.Empty)
                    {
                        description = $"{makeItMoreNatural(parentName)} modified {makeItMoreNatural(name)} from {item.Values().First()} to {item.Values().Last()}";
                    }
                    else
                    {
                        description = $"{makeItMoreNatural(name)} changed from {item.Values().First()} to {item.Values().Last()}";
                    }

                    compResults.Add(new JsonComparisonResult()
                    {
                        Description = description,
                        Name = name,
                        Path = item.Path
                    });
                }
                else if (System.Text.RegularExpressions.Regex.IsMatch(name, REMOVE_PATTERN) && item.Type == JTokenType.Array && ((JArray)item).Count == 3 && ((JArray)item)[1].Value<string>() == "0" && ((JArray)item)[2].Value<string>() == "0")
                {
                    //array with 3 values, and the last two values are zeros. It old item removed

                    string parentName = readName(item.Parent.Path);
                    if (item.Parent.Parent != null)
                    {
                        parentName = readName(item.Parent.Parent.Path);
                    }

                    string description = describeObjectProperties(parentName, "removed with the criteria", ((JObject)((JArray)item)[0]));

                    compResults.Add(new JsonComparisonResult()
                    {
                        Description = description.ToString(),
                        Name = parentName,
                        Path = item.Path
                    });
                }
                else if (item.Type == JTokenType.Array && ((JArray)item).Count == 1 && ((JArray)item).HasValues && (((JArray)item)[0]).Type == JTokenType.Object)
                {
                    //array with one object. It is new item added

                    string parentName = readName(item.Parent.Path);
                    if (item.Parent.Parent != null)
                    {
                        parentName = readName(item.Parent.Parent.Path);
                    }

                    string description = describeObjectProperties(parentName, "added with the criteria", ((JObject)((JArray)item)[0]));

                    compResults.Add(new JsonComparisonResult()
                    {
                        Description = description.ToString(),
                        Name = parentName,
                        Path = item.Path
                    });
                }
                else if (item.Type == JTokenType.Array && ((JArray)item).Count == 1 && ((JArray)item).HasValues && (((JArray)item)[0]).Type != JTokenType.Object)
                {
                    compResults.Add(new JsonComparisonResult()
                    {
                        Description = $"new {makeItMoreNatural(name)} added with {(((JArray)item)[0]).Value<string>()}",
                        Name = name,
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

        private static bool getChangesItemsB(JToken item, List<JsonComparisonResult> compResults)
        {
            bool isRemoveAddScenario = false;

            #region validate item
            if (item.Type != JTokenType.Object || !item.HasValues)
            {
                return isRemoveAddScenario;
            }

            bool idFound = false;

            foreach (JToken token in item.Values())
            {
                if (token.Type != JTokenType.Array)
                {
                    return isRemoveAddScenario;
                }

                if (((JArray)token).Count != 2)
                {
                    return isRemoveAddScenario;
                }

                if (!idFound)
                {
                    string tokenName = makeItMoreNatural(readName(token.Path)).ToLower();

                    if (tokenName == "id")
                    {
                        idFound = true;
                    }
                }
            }

            if (!idFound)
            {
                return isRemoveAddScenario;
            }

            isRemoveAddScenario = true;
            #endregion

            string parentName = readName(item.Parent.Path);
            if (item.Parent.Parent != null)
            {
                parentName = readName(item.Parent.Parent.Path);
            }

            JObject oldItem = new JObject();
            JObject newItem = new JObject();

            foreach (JToken token in item.Values())
            {
                string tokenName = readName(token.Path);

                oldItem.Add(tokenName, ((JArray)token).ElementAt(0));
                newItem.Add(tokenName, ((JArray)token).ElementAt(1));
            }

            string oldDescription = describeObjectProperties(parentName, "removed with the criteria", oldItem);
            string newDescription = describeObjectProperties(parentName, "added with the criteria", newItem);

            compResults.Add(new JsonComparisonResult()
            {
                Description = oldDescription,
                Name = parentName,
                Path = item.Path
            });

            compResults.Add(new JsonComparisonResult()
            {
                Description = newDescription,
                Name = parentName,
                Path = item.Path
            });

            return isRemoveAddScenario;
        }

        private static string describeObjectProperties(string name, string nameDescription, JObject jObject)
        {
            StringBuilder description = new StringBuilder();
            description.Append($"{makeItMoreNatural(name)} {nameDescription}: [");

            int propCount = jObject.Properties().Count();

            for (int i = 0; i < propCount; i++)
            {
                JToken propToken =jObject.Properties().ElementAt(i);

                if (propToken.Type == JTokenType.Property)
                {
                    JProperty prop = propToken as JProperty;

                    if (!string.IsNullOrEmpty(prop.Value.ToString()))
                    {
                        string formattedDate;
                        bool isDatetime = formatIfDatetime(prop.Value.ToString(), out formattedDate);

                        if (isDatetime)
                        {
                            description.Append($"{makeItMoreNatural(prop.Name)} is {formattedDate}");
                        }
                        else
                        {
                            description.Append($"{makeItMoreNatural(prop.Name)} is {prop.Value.ToString()}");
                        }
                    }
                    else
                    {
                        description.Append($"no {makeItMoreNatural(prop.Name)}");
                    }
                }

                if (i < propCount - 1)
                {
                    description.Append(", ");
                }
            }

            description.Append("]");

            return description.ToString();
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

            //remove none alpha numeric characters. remaining characters are a-z A-Z 0-9 _
            string op2 = System.Text.RegularExpressions.Regex.Replace(op1, @"\W+",string.Empty);
            
            //if the input is all in capital and numbers do not process it. ex: ID
            bool allCapital = System.Text.RegularExpressions.Regex.IsMatch(op2, @"^[A-Z0-9]+$");

            if (allCapital)
            {
                return op2;
            }

            //add convert pascal casing to words. ex: DiscountSpecificationName to Discount Specification Name
            string result = System.Text.RegularExpressions.Regex.Replace(op2, "(\\B[A-Z])", " $1");

            result = result.ToLower();
            string[] words = result.Split(" ".ToCharArray());
            #region process abbreviations that is in the middle or the end of the input. ex: discount i d to be discount ID
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
            #endregion

            #region special words handling
            if(words.Contains("is") || words.Contains("has")|| words.Contains("in"))
            {
                result = "\"" + result + "\"";
            }
            #endregion

            return result;
        }

        private static bool formatIfDatetime(string input, out string output)
        {
            bool isDatetime = false;
            output = String.Copy(input);
            DateTime result;

            isDatetime = DateTime.TryParse(input, out result);

            if (isDatetime)
            {
                output = result.ToString("ddd, d MMM, yyyy hh:mm tt");
            }

            return isDatetime;
        }
    }
}
