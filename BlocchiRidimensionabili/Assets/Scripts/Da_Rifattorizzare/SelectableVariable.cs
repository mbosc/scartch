using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using model;
using view;

public class SelectableVariable : Selectable {

    
    private Dictionary<GameObject, Vector2> selectionList;

	public override void OnMouseOverActions() {
		selectionList = new Dictionary<GameObject, Vector2>();
        var wrap = GetComponent<ReferenceWrapper>();
        if (wrap)
        {
            wrap.linkedVariables.ForEach(s =>
            {
                if (s)
                selectionList.Add(s.gameObject,
                            new Vector2(gameObject.transform.position.x - s.gameObject.transform.position.x,
                                gameObject.transform.position.y - s.gameObject.transform.position.y));
            });
        }
        selectionList.Add(gameObject, new Vector2(0, 0));
    }

    public List<BlockWrapperCog> denti
    {
        get
        {
            List<BlockWrapperCog> risposta = new List<BlockWrapperCog>();
            selectionList.Keys.ToList().ForEach(k => k.GetComponentsInChildren<BlockWrapperCog>().ToList().ForEach(risposta.Add));
            return risposta;
        }
    }
    public List<BlockWrapperCogHole> spaziDenti
    {
        get
        {
            List<BlockWrapperCogHole> risposta = new List<BlockWrapperCogHole>();
            selectionList.Keys.ToList().ForEach(k => k.GetComponentsInChildren<BlockWrapperCogHole>().ToList().ForEach(risposta.Add));
            return risposta;
        }
	}
    

    internal void move(Vector3 input)
    {
        selectionList.Keys.ToList().ForEach(k => k.transform.position = new Vector3(input.x - selectionList[k].x, input.y - selectionList[k].y, input.z));
    }

    internal void move(Vector2 input)
    {
        move(new Vector3(input.x, input.y, 0));
    }
	private Vector2 baseClickPosition;
	public override void OnSelection(){
		baseClickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
	}


	public override void OnDeselection(){
		move(this.transform.position + new Vector3(0, 0, Selector.zOffset));
		var variabile = this.GetComponent<ReferenceWrapper>();
		if (variabile && variabile.currentlyHighlighted)
		{
			variabile.currentlyHighlighted.CompletaCon(variabile);
		}
	}

	public override void OnStaySelected(){
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos -= baseClickPosition;
		move(new Vector3(mousePos.x, mousePos.y, -Selector.zOffset));
	}

}
