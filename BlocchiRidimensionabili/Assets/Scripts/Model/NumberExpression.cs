using System.Collections;
using System;
namespace model
{
    public abstract class NumberExpression : Expression, NumberReference
    {
        public abstract string Name { get; }

        public abstract float Evaluate();
        public string EvaluateAsString()
        {
            return string.Format("{0:0.######}", Evaluate());
        }
    }
}