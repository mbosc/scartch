using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using model;
using view;
public class DeltaState
{
    public Vector3 position;
    public Vector3 rotation;
    public DeltaState(Vector3 pos, Vector3 rot)
    {
        position = pos;
        rotation = rot;
    }
}
public class SelectableVariable : Selectable {

    

    private Dictionary<GameObject, DeltaState> selectionList;

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
        //selectionList.Keys.ToList().ForEach(k =>
        //{
        //    k.transform.position = new Vector3(this.transform.position.x - selectionList[k].position.x, this.transform.position.y - selectionList[k].position.y, this.transform.position.z - selectionList[k].position.z);
        //    k.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x - selectionList[k].rotation.x, this.transform.eulerAngles.y - selectionList[k].rotation.y, this.transform.eulerAngles.z - selectionList[k].rotation.z);
        //}
        //);
    }

    internal void move(Vector2 input)
    {
        move(new Vector3(input.x, input.y, 0));
    }

    public bool selected = false;
        
	public override void OnSelection(){
        selected = true;
		selectionList = new Dictionary<GameObject, DeltaState>();
        var wrap = GetComponent<ReferenceWrapper>();
        if (wrap)
        {
            wrap.linkedVariables.ForEach(s =>
            {
            if (s)
                selectionList.Add(s.gameObject,
                    new DeltaState(
                            new Vector3(gameObject.transform.position.x - s.gameObject.transform.position.x,
                                gameObject.transform.position.y - s.gameObject.transform.position.y,
                                 gameObject.transform.position.z - s.gameObject.transform.position.z),
                    new Vector3(gameObject.transform.eulerAngles.x - s.gameObject.transform.eulerAngles.x,
                                gameObject.transform.eulerAngles.y - s.gameObject.transform.eulerAngles.y,
                                 gameObject.transform.eulerAngles.z - s.gameObject.transform.eulerAngles.z))
                );
            });
        }
        selectionList.Add(gameObject, new DeltaState(new Vector3(0, 0, 0), Vector3.zero));

        GetComponent<ReferenceWrapper>().Compact();
    }


	public override void OnDeselection(){
        selected = false;
		var variabile = this.GetComponent<ReferenceWrapper>();
		if (variabile && variabile.currentlyHighlighted)
		{
			variabile.currentlyHighlighted.CompletaCon(variabile);
		}
        variabile.Uncompact();
	}

	public override void OnStaySelected(){
	}

}
