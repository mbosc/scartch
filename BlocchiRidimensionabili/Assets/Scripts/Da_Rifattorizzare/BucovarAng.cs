using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucovarAng : ReferenceContainer {

	public override void CompletaCon (ReferenceWrapper variabile)
	{
		if (variabile is VariabileAngolare)
			base.CompletaCon(variabile);
		else
			throw new UnityException(); //da catturare
	}

	public virtual bool Evaluate (){
		if (variabile)
			return (variabile as VariabileAngolare).valore;
		else
			return false;
	}

}
