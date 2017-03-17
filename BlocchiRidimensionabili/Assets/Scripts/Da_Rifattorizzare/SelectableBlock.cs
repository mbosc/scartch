using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using model;
using view;

public class SelectableBlock : Selectable {

    
    private List<GameObject> selectionList;


    public List<BlockWrapperCog> denti
    {
        get
        {
            List<BlockWrapperCog> risposta = new List<BlockWrapperCog>();
            selectionList.ForEach(k => k.GetComponentsInChildren<BlockWrapperCog>().ToList().ForEach(risposta.Add));
            return risposta;
        }
    }
    public List<BlockWrapperCogHole> spaziDenti
    {
        get
        {
            List<BlockWrapperCogHole> risposta = new List<BlockWrapperCogHole>();
            selectionList.ForEach(k => k.GetComponentsInChildren<BlockWrapperCogHole>().ToList().ForEach(risposta.Add));
            return risposta;
        }
    }

    internal void follow()
    {
        //selectionList.Keys.ToList().ForEach(k =>
        //{
        //    k.transform.position = this.transform.position - selectionList[k].position;
        //    k.transform.eulerAngles = this.transform.eulerAngles - selectionList[k].rotation;
        //}
        //);
    }

    private Dictionary<BlockWrapperCog, bool> statoDenti;
	private Dictionary<BlockWrapperCogHole, bool> statoSpazioDenti;

	public override void OnSelection ()
	{
        Debug.Log("Selected " + name);
        selectionList = new List<GameObject>();
        var blocco = GetComponent<BlockWrapper>();
        if (blocco)
        {
            blocco.linkedBlocks.ForEach(
                s =>
                {
                    selectionList.Add(s.gameObject);
                });
        }
        selectionList.Add(gameObject);

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


        // il codice sopra e' solo legacy per trovare i denti
        GetComponent<BlockWrapper>().Compact();


        
    }

    public override void OnDeselection ()
	{

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
        var blocco = GetComponent<BlockWrapper>();
        blocco.Uncompact();
    }
	public override void OnStaySelected ()
	{
	}


}
