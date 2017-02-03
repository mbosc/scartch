using System.Collections;
using System;
namespace model
{
    public abstract class BooleanExpression : Expression, BooleanReference
    {
        public abstract string Name { get; }

        public abstract bool Evaluate();
        public string EvaluateAsString()
        {
            return Evaluate().ToString();
        }
    }
}
