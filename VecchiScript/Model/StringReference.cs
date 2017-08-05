using System.Collections;
using System;
namespace model
{
    public interface StringReference : Reference
    {
        string Evaluate();
    }
}