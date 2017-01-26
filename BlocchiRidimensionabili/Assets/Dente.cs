using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dente : MonoBehaviour {

	public SpazioDente currentlyHighlighted;

	void OnTriggerEnter (Collider collider){
		if (Selector.instance.selected && Selector.instance.selected.gameObject != this.transform.parent.gameObject)
			return;
		var spazioDente = collider.GetComponent<SpazioDente> ();
		if (spazioDente && spazioDente.free) {
			var Blocco = spazioDente.transform.parent.GetComponent<Blocco>();
			if (!Blocco.first)
				return;
			if (currentlyHighlighted)
				currentlyHighlighted.setHighlightVisible (false);
			currentlyHighlighted = spazioDente;
			spazioDente.setHighlightVisible (true);
		}
	}

	void OnTriggerExit (Collider collider)
	{
		var spazioDente = collider.GetComponent<SpazioDente> ();
		if (spazioDente && spazioDente.transform.parent.gameObject.GetComponent<Blocco>() == transform.parent.gameObject.GetComponent<Blocco>().next)
			transform.parent.GetComponent<Blocco> ().unsetNext();
		ExitTrigger (collider);
	}

	public void ExitTrigger(Collider collider){
	var spazioDente = collider.GetComponent<SpazioDente> ();
	if (spazioDente && spazioDente == currentlyHighlighted) {
		spazioDente.setHighlightVisible (false);
		currentlyHighlighted = null;
	}
	}

	public GameObject highlight;

	public void setHighlightVisible (bool v){
		highlight.SetActive (v);
	}
}
