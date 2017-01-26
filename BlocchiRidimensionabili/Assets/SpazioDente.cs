using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpazioDente : MonoBehaviour {

	public bool free = true;

	public GameObject highlight;

	public void setHighlightVisible (bool v){
		highlight.SetActive (v);
	}

	public Dente currentlyHighlighted;

	void OnTriggerEnter (Collider collider){
		if (Selector.instance.selected && Selector.instance.selected.gameObject != this.transform.parent.gameObject)
			return;
		var dente = collider.GetComponent<Dente> ();
		if (dente) {
			var Blocco = dente.transform.parent.GetComponent<Blocco>();
			if (currentlyHighlighted)
				currentlyHighlighted.setHighlightVisible (false);
			currentlyHighlighted = dente;
			dente.setHighlightVisible (true);
		}
	}

	void OnTriggerExit (Collider collider)
	{
		ExitTrigger (collider);
	}

	public void ExitTrigger (Collider collider){
		var dente = collider.GetComponent<Dente> ();
		if (dente && dente == currentlyHighlighted) {
			dente.setHighlightVisible (false);
			currentlyHighlighted = null;
		}
	}
}
