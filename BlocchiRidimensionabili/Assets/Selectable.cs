using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour {

	private Selector selector;

	// Use this for initialization
	void Start () {
		selector = FindObjectOfType<Selector> ();
	}
	
	void OnMouseOver () {
		selector.hovered = this;
	}
	void OnMouseExit() {
		
		if (selector.hovered == this)
			selector.hovered = null;
	}

}
