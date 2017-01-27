using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Selector : MonoBehaviour
{

    public static Selector instance;
    // Use this for initialization
    void Start()
    {
        instance = this;
    }

    public Selectable selected = null;
    public Selectable hovered = null;
    private Dictionary<Dente, bool> statoDenti;
    private Dictionary<SpazioDente, bool> statoSpazioDenti;
    private Vector2 baseClickPosition;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && hovered && selected == null)
        {
            selected = hovered;
            baseClickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - selected.transform.position;

            statoDenti = new Dictionary<Dente, bool>();
            statoSpazioDenti = new Dictionary<SpazioDente, bool>();
            var denti = selected.denti;
            foreach (var dente in denti)
            {
                statoDenti.Add(dente, dente.Receiving);
                dente.Receiving = false;

            }
            denti[0].searching = true;
            var spazi = selected.spaziDenti;
			if (spazi.Last())
				spazi.Last().Receiving = true;
            foreach (var spazioDente in spazi)
            {
                statoSpazioDenti.Add(spazioDente, spazioDente.Receiving);
                spazioDente.Receiving = false;
            }
			//Debug.Log("Enabling research: spaziodente di " + spazi[0].transform.parent.gameObject.GetComponent<Blocco>().testo);
            if (spazi.Count > 0)
                spazi[spazi.Count - 1].searching = true;
        }
        if (Input.GetMouseButtonUp(0) && selected)
        {
			selected.move (selected.transform.position + new Vector3 (0, 0, 0.5f));
            var oldSelected = selected;
            selected = null;
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
                        spazioDente.currentlyHighlighted.setNext(oldSelected.gameObject.GetComponent<Blocco>());
                    spazioDente.searching = false;
                }
            }

        }
        if (selected)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos -= baseClickPosition;
            selected.move(new Vector3(mousePos.x, mousePos.y, -.5f));
        }
    }
}
