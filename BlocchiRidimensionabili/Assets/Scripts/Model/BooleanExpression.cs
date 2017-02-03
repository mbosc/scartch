using System.Collections;
using System;
namespace model
{
    public abstract class BooleanExpression : Expression, BooleanReference
    {
		
        public abstract bool Evaluate();
        public string EvaluateAsString()
        {
            return Evaluate().ToString();
        }
    }
}
