using System.Collections;
using System;
namespace model
{
	public interface Reference : ScriptingElement
    {
        string EvaluateAsString();
    }
}