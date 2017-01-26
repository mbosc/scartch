using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dente : MonoBehaviour {

	public SpazioDente currentlyHighlighted;
	public delegate void SetNext(Blocco prevBlocco);
	public SetNext setNext;
	public delegate void UnsetNext();
	public UnsetNext unsetNext;

	void OnTriggerEnter (Collider collider){
		if (Selector.instance.selected && Selector.instance.selected.gameObject != this.transform.parent.gameObject)
			return;
		var spazioDente = collider.GetComponent<SpazioDente> ();
		if (spazioDente && spazioDente.Receiving) {
			if (currentlyHighlighted)
				currentlyHighlighted.setHighlightVisible (false);
			currentlyHighlighted = spazioDente;
			spazioDente.setHighlightVisible (true);
		}
	}

	void OnTriggerExit (Collider collider)
	{
		var spazioDente = collider.GetComponent<SpazioDente> ();
		if (spazioDente && transform.parent.gameObject.GetComponent<Blocco> ().linkedBlocks.Contains(spazioDente.transform.parent.gameObject.GetComponent<Blocco> ()))
			unsetNext ();
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
}
