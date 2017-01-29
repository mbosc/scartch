using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariabileCircolare : ReferenceWrapper {

	public int valore {
		get {
			return 2; //servirebbe l'accesso all'environment qui
		}
	}
	public override string valoreStringa {
		get {
			return valore.ToString ();
		}
	}
}
