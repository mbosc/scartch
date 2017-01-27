using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BloccoDoppiaBocca : Blocco
{

    public int firstStretchSize = 1;
    public int secondStretchSize = 1;
    public string secondoTesto;
    public UnityEngine.UI.Text secondoCampoTesto;

    public Blocco upperInternalNext;
    public Blocco lowerInternalNext;
    public Dente denteInternoSuperiore, denteInternoInferiore;
    protected float nextBlockInternoSuperioreOffsetX = 1;
    protected float nextBlockInternoSuperioreOffsetY = -2;
    protected float nextBlockInternoInferioreOffsetX = 1;
    protected float nextBlockInternoInferioreOffsetY
    {
        get
        {
            return -4 - firstStretchSize * 2;
        }
    }
    public override int size
    {
        get
        {
            return 3 + firstStretchSize + secondStretchSize;
        }
    }
    public override List<Dente> denti
    {
        get
        {
            var r = new List<Dente>();
            r.Add(dente);
            r.Add(denteInternoInferiore);
            r.Add(denteInternoSuperiore);
            return r;
        }
    }

    public virtual void setNextInternoSuperiore(Blocco candidateNext)
    {
        Debug.Log(testo + ".setNextInternoSuperiore(" + candidateNext.testo + ")");


        var selectionList = new Dictionary<GameObject, Vector2>();
        candidateNext.linkedBlocks.ForEach(s => selectionList.Add(s.gameObject, new Vector2(candidateNext.gameObject.transform.position.x - s.gameObject.transform.position.x, candidateNext.gameObject.transform.position.y - s.gameObject.transform.position.y)));
        selectionList.Add(candidateNext.gameObject, new Vector2(0, 0));

        selectionList.Keys.ToList().ForEach(k =>
        {
            k.transform.position = this.transform.position + new Vector3(nextBlockInternoSuperioreOffsetX - selectionList[k].x, nextBlockInternoSuperioreOffsetY - selectionList[k].y, 0);
            k.transform.rotation = this.transform.rotation;
        });



        var oldNext = upperInternalNext;
        upperInternalNext = candidateNext;
        upperInternalNext.linkedBlocks.ForEach(AumentaLunghezzaSuperiore);
        AumentaLunghezzaSuperiore(upperInternalNext);

        candidateNext.spazioDente.Receiving = false;
        denteInternoSuperiore.Receiving = false;
        denteInternoSuperiore.Receiving = true;
        if (oldNext)
        {
            upperInternalNext.setNext(oldNext);
        }
    }

    public void AumentaLunghezzaSuperiore(Blocco next)
    {
        var lung = 0;
        Blocco catena = upperInternalNext;
        while (catena)
        {
			lung+= catena.size;
            catena = catena.next;
        }
        firstStretchSize = lung;
        extendToMatchContent();
        next.denti.ForEach(d => d.setNext += AumentaLunghezzaSuperiore);
        Debug.Log("Invocation list di " + next + ".setNext: " + next.dente.setNext.GetInvocationList().Length);
        next.denti.ForEach(d => d.unsetNext += DiminuisciLunghezzaSuperiore);

		RiposizionaBloccoInferiore ();
    }

	private void RiposizionaBloccoInferiore(){
		//Aggiornamento lunghezza inferiore
		if (lowerInternalNext) {
			var selectionList = new Dictionary<GameObject, Vector2> ();
			if (lowerInternalNext.linkedBlocks.Count > 0)
				lowerInternalNext.linkedBlocks.ForEach (s => selectionList.Add (s.gameObject, new Vector2 (lowerInternalNext.gameObject.transform.position.x - s.gameObject.transform.position.x, lowerInternalNext.gameObject.transform.position.y - s.gameObject.transform.position.y)));
			selectionList.Add (lowerInternalNext.gameObject, new Vector2 (0, 0));

			selectionList.Keys.ToList ().ForEach (k => {
				k.transform.position = this.transform.position + new Vector3 (nextBlockInternoInferioreOffsetX - selectionList [k].x, nextBlockInternoInferioreOffsetY - selectionList [k].y, 0);
				k.transform.rotation = this.transform.rotation;
			});
		}
	}

    public void DiminuisciLunghezzaSuperiore(Blocco next)
    {
        var lung = 0;
        Blocco catena = next;
        while (catena)
        {
            catena.denti.ForEach(d => d.setNext -= AumentaLunghezzaSuperiore);
            Debug.Log("Invocation list di " + catena + ".setNext: " + catena.dente.setNext.GetInvocationList().Length);
            catena.denti.ForEach(d => d.unsetNext -= DiminuisciLunghezzaSuperiore);
            catena = catena.next;
        }
        catena = upperInternalNext;
        while (catena)
        {
            lung += catena.size;
            catena = catena.next;
        }
        firstStretchSize = lung;


        extendToMatchContent();
		RiposizionaBloccoInferiore ();
    }

    public virtual void unsetNextInternoSuperiore(Blocco next)
    {

        Blocco catena = upperInternalNext;
        while (catena)
        {

            catena.denti.ForEach(d => d.setNext -= AumentaLunghezzaSuperiore);
            Debug.Log("Invocation list di " + catena + ".setNext: " + catena.dente.setNext.GetInvocationList().Length);
            catena.denti.ForEach(d => d.unsetNext -= DiminuisciLunghezzaSuperiore);
            catena = catena.next;
        }
        upperInternalNext.spazioDente.Receiving = true;
        upperInternalNext = null;
        firstStretchSize = 0;

        extendToMatchContent();
		RiposizionaBloccoInferiore ();
    }

    public virtual void setNextInternoInferiore(Blocco candidateNext)
    {
        Debug.Log(testo + ".setNextInternoInferiore(" + candidateNext.testo + ")");


        var selectionList = new Dictionary<GameObject, Vector2>();
        candidateNext.linkedBlocks.ForEach(s => selectionList.Add(s.gameObject, new Vector2(candidateNext.gameObject.transform.position.x - s.gameObject.transform.position.x, candidateNext.gameObject.transform.position.y - s.gameObject.transform.position.y)));
        selectionList.Add(candidateNext.gameObject, new Vector2(0, 0));

        selectionList.Keys.ToList().ForEach(k =>
        {
            k.transform.position = this.transform.position + new Vector3(nextBlockInternoInferioreOffsetX - selectionList[k].x, nextBlockInternoInferioreOffsetY - selectionList[k].y, 0);
            k.transform.rotation = this.transform.rotation;
        });

        var oldNext = lowerInternalNext;
        lowerInternalNext = candidateNext;
        lowerInternalNext.linkedBlocks.ForEach(AumentaLunghezzaInferiore);
        AumentaLunghezzaInferiore(lowerInternalNext);
        candidateNext.spazioDente.Receiving = false;
        denteInternoInferiore.Receiving = false;
        denteInternoInferiore.Receiving = true;
        if (oldNext)
        {
            lowerInternalNext.setNext(oldNext);
        }
    }

    public void AumentaLunghezzaInferiore(Blocco next)
    {
        var lung = 0;
        Blocco catena = lowerInternalNext;
        while (catena)
        {
			lung += catena.size;
            catena = catena.next;
        }
        secondStretchSize = lung;
        extendToMatchContent();
        next.denti.ForEach(d => d.setNext += AumentaLunghezzaInferiore);
        Debug.Log("Invocation list di " + next + ".setNext: " + next.dente.setNext.GetInvocationList().Length);
        next.denti.ForEach(d => d.unsetNext += DiminuisciLunghezzaInferiore);

    }

    public void DiminuisciLunghezzaInferiore(Blocco next)
    {
        var lung = 0;
        Blocco catena = next;
        while (catena)
        {
            catena.denti.ForEach(d => d.setNext -= AumentaLunghezzaInferiore);
            Debug.Log("Invocation list di " + catena + ".setNext: " + catena.dente.setNext.GetInvocationList().Length);
            catena.denti.ForEach(d => d.unsetNext -= DiminuisciLunghezzaInferiore);
            catena = catena.next;
        }
        catena = lowerInternalNext;
        while (catena)
        {
            lung += catena.size;
            catena = catena.next;
        }
        secondStretchSize = lung;

        extendToMatchContent();
    }

    public virtual void unsetNextInternoInferiore(Blocco next)
    {

        Blocco catena = lowerInternalNext;
        while (catena)
        {

            catena.denti.ForEach(d => d.setNext -= AumentaLunghezzaInferiore);
            Debug.Log("Invocation list di " + catena + ".setNext: " + catena.dente.setNext.GetInvocationList().Length);
            catena.denti.ForEach(d => d.unsetNext -= DiminuisciLunghezzaInferiore);
            catena = catena.next;
        }
        lowerInternalNext.spazioDente.Receiving = true;
        lowerInternalNext = null;
        secondStretchSize = 0;
        extendToMatchContent();
    }


    public override List<Blocco> directlyLinkedBlocks
    {
        get
        {
            var ex = new List<Blocco>();
            ex.Add(next);
            ex.Add(upperInternalNext);
            ex.Add(lowerInternalNext);
            return ex;

        }
    }
    public override List<Blocco> linkedBlocks
    {
        get
        {
            var ex = new List<Blocco>();
            if (upperInternalNext)
            {
                upperInternalNext.linkedBlocks.ForEach(ex.Add);
                ex.Add(upperInternalNext);
            }
            if (lowerInternalNext)
            {
                lowerInternalNext.linkedBlocks.ForEach(ex.Add);
                ex.Add(lowerInternalNext);
            }
            if (next)
            {
                next.linkedBlocks.ForEach(ex.Add);
                ex.Add(next);
            }
            return ex;
        }
    }


    protected virtual void extendToMatchContent()
    {
        secondoCampoTesto.text = secondoTesto;
        if (firstStretchSize < 1)
            firstStretchSize = 1;
        if (secondStretchSize < 1)
            secondStretchSize = 1;

        List<int> verticesToStretch = new List<int>();
        List<int> verticesToStretchTwice = new List<int>();
        for (int i = 0; i < originaryVertices.Length; i++)
        {
            var vertex = originaryVertices[i];
            if (Mathf.RoundToInt(vertex.z) <= -3)
                verticesToStretch.Add(i);
            if (Mathf.RoundToInt(vertex.z) <= -7)
                verticesToStretchTwice.Add(i);
        }

        var levert = mesh.vertices;
        foreach (var i in verticesToStretch)
			levert[i] = new Vector3(levert[i].x, levert[i].y, originaryVertices[i].z - (firstStretchSize - 1) * 2);
        denteInternoInferiore.gameObject.transform.localPosition = new Vector3(denteInternoInferiore.gameObject.transform.localPosition.x, denteInternoInferiore.gameObject.transform.localPosition.y, 0.5f - 4 - firstStretchSize * 2);
        foreach (var i in verticesToStretchTwice)
			levert[i] = new Vector3(levert[i].x, levert[i].y, levert[i].z - (secondStretchSize - 1) * 2);
        dente.gameObject.transform.localPosition = new Vector3(dente.gameObject.transform.localPosition.x, dente.gameObject.transform.localPosition.y, 0.5f - 6 - (firstStretchSize + secondStretchSize) * 2);
        mesh.SetVertices(new List<Vector3>(levert));
        if (GetComponent<MeshCollider>())
            Destroy(GetComponent<MeshCollider>());
        initialised = false;
        nextBlockOffsetY = -6 - firstStretchSize * 2 - secondStretchSize * 2;
		secondoCampoTesto.transform.parent.transform.localPosition = posizioneBaseSecondoCampoTesto + new Vector3(0, 0, -(firstStretchSize-1)*2);

        if (next)
        {
            // align next blocks
            var selectionList = new Dictionary<GameObject, Vector2>();
            next.linkedBlocks.ForEach(s => selectionList.Add(s.gameObject, new Vector2(next.gameObject.transform.position.x - s.gameObject.transform.position.x, next.gameObject.transform.position.y - s.gameObject.transform.position.y)));
            selectionList.Add(next.gameObject, new Vector2(0, 0));

            selectionList.Keys.ToList().ForEach(k =>
            {
                k.transform.position = this.transform.position + new Vector3(nextBlockOffsetX - selectionList[k].x, nextBlockOffsetY - selectionList[k].y, 0);
                k.transform.rotation = this.transform.rotation;
            });
        }
    }

    protected override int lunghezzaTesto
    {
		get{
			return Mathf.Max (campoTesto.text.Length, secondoCampoTesto.text.Length);
		}
    }

    public override string EvaluateMe(string tabs)
    {
        var output = tabs + testo + " {\n";
        if (upperInternalNext)
            output += upperInternalNext.EvaluateMe("  " + tabs);
        output += tabs + "} " + secondoTesto + " {\n";
        if (lowerInternalNext)
            output += lowerInternalNext.EvaluateMe("  " + tabs);
        output += tabs + "}\n";
        if (next)
            output += next.EvaluateMe(tabs);
        return output;
    }

	private Vector3 posizioneBaseSecondoCampoTesto;

    protected override void Start()
    {
        offsetTestoBaseX = 2;
        base.Start();
		posizioneBaseSecondoCampoTesto = secondoCampoTesto.transform.parent.localPosition;
        denteInternoSuperiore.setNext = setNextInternoSuperiore;
        denteInternoSuperiore.unsetNext = unsetNextInternoSuperiore;
        denteInternoInferiore.setNext = setNextInternoInferiore;
        denteInternoInferiore.unsetNext = unsetNextInternoInferiore;
        extendToMatchContent();
        nextBlockOffsetY = -6 - firstStretchSize * 2 - secondStretchSize * 2;
    }

}
