using System.Collections;
using System;
namespace model
{
    public class NumberVariable : Variable, NumberReference
    {
        private float value;
		public NumberVariable(string name, float value)
        {
            //da controllare
            Name = name;
            this.value = value;
        }
        public NumberVariable(string name)
            : this(name, 0) { }

        public virtual float Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public float Evaluate()
        {
            return value;
        }

        public string EvaluateAsString()
        {
            return string.Format("{0:0.######}", value);
        }
    }
}