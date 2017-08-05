using System.Collections;
using System;
namespace model
{
    public class BooleanVariable : Variable, BooleanReference
    {
        private bool value;
        

        public BooleanVariable(string name, bool value)
        {
            //da controllare
            Name = name;
            this.value = value;
        }
        public BooleanVariable(string name)
            : this(name, false) { }

        public bool Value
        {
            get { return value; }
            set { this.value = value;
                OnVariableChanged();
            }
        }

        public bool Evaluate()
        {
            return value;
        }

        public override string EvaluateAsString()
        {
            return value.ToString();
        }
    }
}