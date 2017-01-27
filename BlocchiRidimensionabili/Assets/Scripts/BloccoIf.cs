using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloccoIf : BloccoBocca {

	public override string EvaluateMe(string tabs)
	{
		var output = "";
		if (slotVariabili[0].Evaluate() && internalNext)
			output = tabs + internalNext.EvaluateMe (tabs);
		if (next)
			output += next.EvaluateMe(tabs);
		return output;
	}

	protected override void Start()
	{
		testo = "se <  >, allora";
		base.Start();
	}
}
