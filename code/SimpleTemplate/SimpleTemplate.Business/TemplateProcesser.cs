using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTemplate.Business
{
    public class TemplateProcesser
    {
        #region attributes
        private List<Model.Variable> _Variables = new List<Model.Variable>();
        protected string _Result = string.Empty;
        #endregion

        #region properties
        public string Template { get; set; }
        public string Result
        {
            get
            {
                return this._Result;
            }
        }
        public JObject Data { get; set; }
        public Hashtable Mapping { get; set; }
        #endregion

        #region contructors
        public TemplateProcesser(string template, JObject data, Hashtable mapping = null)
        {
            this.Template = template;
            this.Data = data;
            this.Mapping = mapping;

            this._Result = process(this.Template, this.Data, this.Mapping);
        }
        #endregion

        #region private methods
        private string process(string template, JObject data, Hashtable mapping = null)
        {
            this._Variables.AddRange(extractVariables(template));

            foreach (var variable in this._Variables)
            {
                Function.IFunction fn = Function.FunctionFactory.GetFunction(variable, data, mapping);
                variable.Value = fn.CalculateValue();
            }

            return replaceVariablesWithValues(this.Template, this._Variables);
        }

        private List<Model.Variable> extractVariables(string template)
        {
            List<Model.Variable> variables = new List<Model.Variable>();

            int index1 = 0, index2 = 0;

            do
            {

                index1 = template.IndexOf("{{", index1 + 1);
                index2 = template.IndexOf("}}", index1 + 1);

                if (index1 != -1)
                {
                    variables.Add(new Model.Variable() { Input = template.Substring(index1 + 2, index2 - index1 - 2) });
                }

            } while (index1 >= 0);

            return variables;
        }

        private string replaceVariablesWithValues(string template, List<Model.Variable> variables)
        {
            string result = string.Copy(template);

            foreach (var variable in variables)
            {
                result = result.Replace("{{" + variable.Input + "}}", variable.Value);
            }

            return result;
        }
        #endregion
    }
}
