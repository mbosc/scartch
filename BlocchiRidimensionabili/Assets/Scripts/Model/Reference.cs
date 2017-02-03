using System.Collections;
using System;
namespace model
{
	public interface Reference : ScriptingElement
    {
        string Name { get; }
        string EvaluateAsString();
    }
}