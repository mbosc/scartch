using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloccoIfElse : BloccoDoppiaBocca {

	// Use this for initialization
	protected override void Start () {
		testo = "se <  >, allora";
		secondoTesto = "altrimenti";
		base.Start ();
	}
	
	public override string EvaluateMe (string tabs)
	{
		var output = "";
		if ((slotVariabili [0] as BucovarAng).Evaluate ()) {
			if (upperInternalNext)
				output += upperInternalNext.EvaluateMe (tabs);
		} else {
			if (lowerInternalNext)
				output += lowerInternalNext.EvaluateMe (tabs);
		}
		if (next)
			output += next.EvaluateMe(tabs);
		return output;
	}
}
