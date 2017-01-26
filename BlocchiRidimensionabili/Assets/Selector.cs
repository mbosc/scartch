using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour {

	public static Selector instance;
	// Use this for initialization
	void Start () {
		instance = this;
	}

	public Selectable selected = null;
	public Selectable hovered = null;

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0) && hovered && selected == null) {
			selected = hovered;
		}
		if (Input.GetMouseButtonUp (0) && selected) {
			var denti = selected.gameObject.GetComponentsInChildren<Dente> ();
			var spaziDenti = selected.gameObject.GetComponentsInChildren<SpazioDente> ();
			foreach (var dente in denti) {
				if (dente.currentlyHighlighted) {
					dente.currentlyHighlighted.transform.parent.gameObject.GetComponent<Blocco> ().setPrevious (selected.gameObject.GetComponent<Blocco> ());
//					dente.ExitTrigger (dente.currentlyHighlighted.gameObject.GetComponent<Collider> ());
				}
			}
			foreach (var spazioDente in spaziDenti) {
				if (spazioDente.currentlyHighlighted) {
					spazioDente.currentlyHighlighted.transform.parent.gameObject.GetComponent<Blocco> ().setNext (selected.gameObject.GetComponent<Blocco> ());
//					spazioDente.ExitTrigger (spazioDente.currentlyHighlighted.gameObject.GetComponent<Collider> ());
				}
			}
			selected = null;
		}
		if (selected) {
			var mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			selected.gameObject.transform.position = new Vector3 (mousePos.x, mousePos.y, selected.gameObject.transform.position.z);
		}
	}
}
