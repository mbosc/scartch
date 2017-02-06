using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using model;
using view;

public class SelectableVariable : Selectable {

    
    private Dictionary<GameObject, Vector3> selectionList;

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
    

    internal void follow()
    {
        selectionList.Keys.ToList().ForEach(k => k.transform.position = new Vector3(this.transform.position.x - selectionList[k].x, this.transform.position.y - selectionList[k].y, this.transform.position.z - selectionList[k].z));
    }

    internal void move(Vector2 input)
    {
        move(new Vector3(input.x, input.y, 0));
    }
	private Vector2 baseClickPosition;
	public override void OnSelection(){
		baseClickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        selectionList = new Dictionary<GameObject, Vector3>();
        var wrap = GetComponent<ReferenceWrapper>();
        if (wrap)
        {
            wrap.linkedVariables.ForEach(s =>
            {
                if (s)
                    selectionList.Add(s.gameObject,
                                new Vector3(gameObject.transform.position.x - s.gameObject.transform.position.x,
                                    gameObject.transform.position.y - s.gameObject.transform.position.y,
                                     gameObject.transform.position.z - s.gameObject.transform.position.z));
            });
        }
        //selectionList.Add(gameObject, new Vector2(0, 0));
    }


	public override void OnDeselection(){
		var variabile = this.GetComponent<ReferenceWrapper>();
		if (variabile && variabile.currentlyHighlighted)
		{
			variabile.currentlyHighlighted.CompletaCon(variabile);
		}
	}

	public override void OnStaySelected(){
        follow();
	}

}
