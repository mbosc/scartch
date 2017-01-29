using System.Collections;
using System;
namespace model
{
    public interface NumberReference : Reference
    {
        float Evaluate();
    }
}