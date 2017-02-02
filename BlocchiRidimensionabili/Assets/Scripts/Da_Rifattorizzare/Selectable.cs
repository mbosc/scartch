using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using model;
using view;

public class Selectable : MonoBehaviour {

	protected Selector selector;
    

    // Use this for initialization
    protected virtual void Start()
    {
        selector = FindObjectOfType<Selector>();
    }
    
	protected virtual void OnMouseOver()
    {
        selector.hovered = this;
		OnMouseOverActions ();
    }

    protected virtual void OnMouseExit()
    {

        if (selector.hovered == this)
            selector.hovered = null;
    }

	public virtual void OnMouseOverActions(){

	}
	public virtual void OnSelection(){

	}
	public virtual void OnDeselection(){

	}
	public virtual void OnStaySelected(){

	}
}
