using System.Collections;
using System;
namespace model
{
    public class StringVariable : Variable, StringReference
    {
        private string value;
        

        public StringVariable(string name, string value)
        {
            //da controllare
            Name = name;
            this.value = value;
        }
        public StringVariable(string name)
            : this(name, "") { }

        public string Value
        {
            get { return value; }
            set { this.value = value; OnVariableChanged();
            }
        }

        public string Evaluate()
        {
            return value;
        }

        public override string EvaluateAsString()
        {
            return value;
        }
    }
}