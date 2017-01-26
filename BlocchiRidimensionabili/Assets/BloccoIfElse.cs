using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BloccoIfElse : Blocco {

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
    protected float nextBlockInternoInferioreOffsetY = -2;

    public virtual void setNextInternoSuperiore(Blocco candidateNext)
    {
        Debug.Log(testo + ".setNextInterno(" + candidateNext.testo + ")");


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


    protected virtual void extendToMatchContent(){
		secondoCampoTesto.text = secondoTesto;
		if (firstStretchSize < 1)
			firstStretchSize = 1;
		if (secondStretchSize < 1)
			secondStretchSize = 1;
		
		List<int> verticesToStretch = new List<int> ();
		List<int> verticesToStretchTwice = new List<int> ();
		for (int i = 0; i < originaryVertices.Length; i++) {
			var vertex = originaryVertices [i];
			if (Mathf.RoundToInt(vertex.z) <= -3)
				verticesToStretch.Add (i);
			if (Mathf.RoundToInt(vertex.z) <= -7)
				verticesToStretchTwice.Add (i);
		}

		var levert = mesh.vertices;
		foreach (var i in verticesToStretch)
			levert[i] = new Vector3 (levert[i].x, levert[i].y, levert[i].z  - (firstStretchSize-1)*2);
		foreach (var i in verticesToStretchTwice)
			levert[i] = new Vector3 (levert[i].x, levert[i].y, levert[i].z  - (secondStretchSize-1)*2);
		mesh.SetVertices (new List<Vector3>(levert));

		secondoCampoTesto.transform.parent.transform.position -= new Vector3 (0, (firstStretchSize - 1) * 2, 0);
	}

	protected override int calcolaLunghezzaTesto(){
		return Mathf.Max (testo.Length, secondoTesto.Length);
	}

	protected override void Start ()
	{
		offsetTestoBaseX = 2;
		base.Start ();
		
        denteInternoSuperiore.setNext = setNextInternoSuperiore;
        denteInternoSuperiore.unsetNext = unsetNextInternoSuperiore;
        denteInternoInferiore.setNext = setNextInternoInferiore;
        denteInternoInferiore.unsetNext = unsetNextInternoInferiore;
        //evaluateVars (secondoTesto, offsetTestoBaseX, (-firstStretchSize - 1)*2);
        extendToMatchContent();
        nextBlockOffsetY = -6 - firstStretchSize*2 - secondStretchSize*2 -1 ;
    }

}
