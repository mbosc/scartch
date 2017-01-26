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
			var denti = selected.gameObject.GetComponentsInChildren<Dente> ();
			var spaziDenti = selected.gameObject.GetComponentsInChildren<SpazioDente> ();
			foreach (var dente in denti) {
				dente.Receiving = false;
			}
			foreach (var spazioDente in spaziDenti) {
				spazioDente.Receiving = false;
			}
		}
		if (Input.GetMouseButtonUp (0) && selected) {
			var oldSelected = selected;
			selected = null;
			var denti = oldSelected.gameObject.GetComponentsInChildren<Dente> ();
			var spaziDenti = oldSelected.gameObject.GetComponentsInChildren<SpazioDente> ();
			foreach (var dente in denti) {
				dente.Receiving = true;
				if (dente.currentlyHighlighted) {
					dente.currentlyHighlighted.setPrevious (oldSelected.gameObject.GetComponent<Blocco> ());

				}
			}
			foreach (var spazioDente in spaziDenti) {
				spazioDente.Receiving = true;
				if (spazioDente.currentlyHighlighted) {
					spazioDente.currentlyHighlighted.setNext (oldSelected.gameObject.GetComponent<Blocco> ());

				}
			}

		}
		if (selected) {
			var mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			selected.gameObject.transform.position = new Vector3 (mousePos.x, mousePos.y, selected.gameObject.transform.position.z);
		}
	}
}
