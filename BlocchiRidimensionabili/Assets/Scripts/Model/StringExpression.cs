using System.Collections;
using System;
namespace model
{
    public abstract class StringExpression : Expression, StringReference
    {
        public abstract string Evaluate();
        public string EvaluateAsString()
        {
            return Evaluate();
        }
    }
}