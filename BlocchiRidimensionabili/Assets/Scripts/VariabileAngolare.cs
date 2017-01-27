using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VariabileAngolare : Variabile {

	public bool valore {
		get {
			return GameObject.Find("Toggle").GetComponent<UnityEngine.UI.Toggle>().isOn; //servirebbe l'accesso all'environment qui
		}
	}
	public override string valoreStringa {
		get {
			return valore.ToString ();
		}
	}

}
