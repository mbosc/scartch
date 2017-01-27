using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucovarAng : Bucovar {

	public override void CompletaCon (Variabile variabile)
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
