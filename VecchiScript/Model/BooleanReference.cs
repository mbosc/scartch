using System.Collections;
using System;
namespace model
{
    public interface BooleanReference : Reference
    {
        bool Evaluate();
    }
}