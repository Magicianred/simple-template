using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTemplate.Model
{
    public class Variable
    {
        #region attributes
        protected string _Input = string.Empty;
        protected string _Name = string.Empty;
        protected bool _IsFunction = false;
        protected string _FunctionName = string.Empty;
        protected List<string> _FunctionArguments = new List<string>();
        #endregion

        #region properties
        public string Input
        {
            get
            {
                return this._Input;
            }
            set
            {
                init(value);
            }
        }

        public string Name
        {
            get
            {
                return this._Name;
            }
        }

        public bool IsFunction
        {
            get
            {
                return this._IsFunction;
            }
        }
        
        public List<string> FunctionArguments
        {
            get
            {
                return this._FunctionArguments;
            }
        }

        public string FunctionName
        {
            get
            {
                return this._FunctionName;
            }
        }

        public string Value { get; set; }
        #endregion

        #region private methods
        private void init(string value)
        {
            this._Input = value;
            this._Name = value;
            int indexOfFunction1 = value.IndexOf("$");
            int indexOfFunction2 = value.IndexOf("$", indexOfFunction1 + 1);

            this._IsFunction = indexOfFunction1 >= 0 && indexOfFunction2 >= 0; //contains $some chracters$

            if (this.IsFunction)
            {
                this._FunctionName = value.Substring(indexOfFunction1 + 1, indexOfFunction2 - indexOfFunction1 - 1);

                string args = value.Substring(indexOfFunction2 + 1);

                if (!string.IsNullOrWhiteSpace(args))
                {
                    this._FunctionArguments = value.Substring(indexOfFunction2 + 1).Split(",".ToCharArray()).ToList();
                }

                this._Name = string.Empty;
            }
        }
        #endregion
    }
}
