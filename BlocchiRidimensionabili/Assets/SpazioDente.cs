using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpazioDente : MonoBehaviour {

	public delegate void SetPrevious(Blocco prevBlocco);
	public SetPrevious setPrevious;
	public GameObject highlight;
	public GameObject passiveHighlight;
	public bool receiving = true;
	public bool Receiving {
		get { return receiving; }
		set {
			passiveHighlight.SetActive (value);
			if (value == false) {
				highlight.SetActive (false);
				currentlyHighlighted = null;
			}
			receiving = value;
		}
	}

	public void setHighlightVisible (bool v){
		highlight.SetActive (v);
		passiveHighlight.SetActive(!v && Receiving);
	}

	public Dente currentlyHighlighted;

	void OnTriggerEnter (Collider collider){
		if (Selector.instance.selected && Selector.instance.selected.gameObject != this.transform.parent.gameObject)
			return;
		var dente = collider.GetComponent<Dente> ();
		if (dente && dente.Receiving) {
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
