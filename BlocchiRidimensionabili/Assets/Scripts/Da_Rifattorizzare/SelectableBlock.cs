using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using model;
using view;

public class SelectableBlock : Selectable {

    
    private Dictionary<GameObject, Vector2> selectionList;

	public override void OnMouseOverActions() {
		selectionList = new Dictionary<GameObject, Vector2>();
		var blocco = GetComponent<BlockWrapper>();
		if (blocco)
		{
			blocco.linkedBlocks.ForEach(
				s =>
				{
					selectionList.Add(s.gameObject, new Vector2(gameObject.transform.position.x - s.gameObject.transform.position.x, gameObject.transform.position.y - s.gameObject.transform.position.y));
					s.slotVariabili.ForEach(z =>
						{
							if (z.variabile)
								selectionList.Add(z.variabile.gameObject, new Vector2(gameObject.transform.position.x - z.variabile.gameObject.transform.position.x, gameObject.transform.position.y - z.variabile.gameObject.transform.position.y));
						});
				});
			blocco.slotVariabili.ForEach(s =>
				{
					if (s.variabile)
						selectionList.Add(s.variabile.gameObject,
							new Vector2(gameObject.transform.position.x - s.variabile.gameObject.transform.position.x,
								gameObject.transform.position.y - s.variabile.gameObject.transform.position.y));
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

	private Dictionary<BlockWrapperCog, bool> statoDenti;
	private Dictionary<BlockWrapperCogHole, bool> statoSpazioDenti;

	public override void OnSelection ()
	{
		baseClickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		statoDenti = new Dictionary<BlockWrapperCog, bool>();
		statoSpazioDenti = new Dictionary<BlockWrapperCogHole, bool>();
		foreach (var dente in denti)
		{
			statoDenti.Add(dente, dente.Receiving);
			dente.Receiving = false;

		}
		if (denti.Count > 0)
		{
			denti[0].searching = true;
			//Debug.Log("BlockWrapperCog di " + denti[0].transform.parent.name + " " + denti[0].name);
		}

		if (spaziDenti.Count > 0)
			spaziDenti.Last().Receiving = true;
		foreach (var spazioDente in spaziDenti)
		{
			statoSpazioDenti.Add(spazioDente, spazioDente.Receiving);
			spazioDente.Receiving = false;
		}
		//Debug.Log("Enabling research: spaziodente di " + spazi[0].transform.parent.gameObject.GetComponent<Blocco>().testo);
		if (spaziDenti.Count > 0)
			spaziDenti[spaziDenti.Count - 1].searching = true;
	}

	public override void OnDeselection ()
	{
		move(this.transform.position + new Vector3(0, 0, Selector.zOffset));
		foreach (var dente in statoDenti.Keys)
		{
			if (statoDenti[dente])
			{
				dente.Receiving = true;
				if (dente.currentlyHighlighted)
					dente.currentlyHighlighted.setPrevious(dente);
				dente.searching = false;
			}
		}
		foreach (var spazioDente in statoSpazioDenti.Keys)
		{
			if (statoSpazioDenti[spazioDente])
			{
				spazioDente.Receiving = true;
				if (spazioDente.currentlyHighlighted)
					spazioDente.currentlyHighlighted.setNext(this.gameObject.GetComponent<BlockWrapper>());
				spazioDente.searching = false;
			}
		}
	}
	private Vector2 baseClickPosition;
	public override void OnStaySelected ()
	{
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos -= baseClickPosition;
		move(new Vector3(mousePos.x, mousePos.y, -Selector.zOffset));
	}


}
