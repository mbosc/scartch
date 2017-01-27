using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Blocco : MonoBehaviour
{
	public List<Bucovar> slotVariabili;
    public Blocco next;
    public Dente dente;
    public virtual List<Dente> denti
    {
        get
        {
            var r = new List<Dente>();
            r.Add(dente);
            return r;
        }
    }
    public SpazioDente spazioDente;
    public bool first = true;
    public string testo;
    public UnityEngine.UI.Text campoTesto;
    public virtual int size
    {
        get { return 1; }
    }
    protected Mesh mesh;
    protected float nextBlockOffsetX = 0;
    protected float nextBlockOffsetY = -2;
    protected float prevBlockOffsetX = 0;
    protected float prevBlockOffsetY = 2;
    protected float dongExt;
    
    protected float deformConst = 1;
    protected Vector3[] originaryVertices;
    public bool lastBlock = false;

    public virtual string EvaluateMe(string tabs)
    {
        var output = tabs + testo + "\n";
        if (next)
            output += next.EvaluateMe(tabs);
        return output;
    }

    public override string ToString()
    {
        return testo;
    }

    public virtual void setNext(Blocco candidateNext)
    {
        Debug.Log(testo + ".setNext(" + candidateNext.testo + ")");

        var selectionList = new Dictionary<GameObject, Vector2>();
        candidateNext.linkedBlocks.ForEach(s => selectionList.Add(s.gameObject, new Vector2(candidateNext.gameObject.transform.position.x - s.gameObject.transform.position.x, candidateNext.gameObject.transform.position.y - s.gameObject.transform.position.y)));
        selectionList.Add(candidateNext.gameObject, new Vector2(0, 0));

        

        var selectionVariables = new Dictionary<GameObject, Vector2>();
        candidateNext.linkedVariables.ForEach(s => selectionVariables.Add(s.gameObject, new Vector2(candidateNext.gameObject.transform.position.x - s.gameObject.transform.position.x, candidateNext.gameObject.transform.position.y - s.gameObject.transform.position.y)));

        selectionList.Keys.ToList().ForEach(k =>
        {
            k.transform.position = this.transform.position + new Vector3(nextBlockOffsetX - selectionList[k].x, nextBlockOffsetY - selectionList[k].y, 0);
            k.transform.rotation = this.transform.rotation;
        });

        selectionVariables.Keys.ToList().ForEach(k =>
        {
            Debug.Log("Sposto " + k.name);
            k.transform.position = this.transform.position + new Vector3(nextBlockOffsetX - selectionVariables[k].x, nextBlockOffsetY - selectionVariables[k].y, 0);
        });


        var oldNext = next;
        next = candidateNext;

        candidateNext.spazioDente.Receiving = false;
        dente.Receiving = false;
        dente.Receiving = true;
        if (oldNext)
        {
            var blocchi = new List<Blocco>();
            blocchi.Add(next);
            next.linkedBlocks.ForEach(blocchi.Add);
            blocchi.Last().setNext(oldNext);
        }
    }

    public virtual List<Blocco> linkedBlocks
    {
        get
        {
            var ex = new List<Blocco>();
            if (next)
            {
                next.linkedBlocks.ForEach(s => { if (s) ex.Add(s); });
                ex.Add(next);
            }
            return ex;
        }
    }
    public virtual List<VariabileAngolare> linkedVariables
    {
        get
        {
            var ex = new List<VariabileAngolare>();
            linkedBlocks.ForEach(s => { s.slotVariabili.ForEach(z => { if (z.variabile) ex.Add(z.variabile); }); });
            slotVariabili.ForEach(s => { if (s.variabile) ex.Add(s.variabile); });
            ex.ForEach(Debug.Log);
            return ex;
        }
    }
    public virtual List<Blocco> directlyLinkedBlocks
    {
        get
        {
            var ex = new List<Blocco>();
            ex.Add(next);
            return ex;
        }
    }
    public virtual void unsetNext(Blocco thisBlocco)
    {
        Debug.Log(testo + ".unsetNext()");
        if (next)
            next.spazioDente.Receiving = true;
        next = null;
    }

    public virtual void setPrevious(Dente candidatePrevious)
    {
        Debug.Log(testo + ".setPrevious(" + candidatePrevious.transform.parent.GetComponent<Blocco>().testo + ")");

        candidatePrevious.setNext(this);
        spazioDente.Receiving = false;
    }

    protected virtual void extendToMatchText()
    {
        dongExt = lunghezzaTesto * deformConst;

        if (dongExt < 1)
            dongExt = 1;


        List<int> verticesToEdit = new List<int>();

        int maxX = int.MinValue;
        for (int i = 0; i < originaryVertices.Length; i++)
        {
            var vertex = originaryVertices[i];
            if (Mathf.RoundToInt(vertex.x) > maxX)
                maxX = Mathf.RoundToInt(vertex.x);
        }

        for (int i = 0; i < originaryVertices.Length; i++)
        {
            var vertex = originaryVertices[i];
            if (Mathf.RoundToInt(vertex.x) == maxX)
                verticesToEdit.Add(i);
        }

        var levert = mesh.vertices;
        foreach (var i in verticesToEdit)
			levert[i] = new Vector3(originaryVertices[i].x + dongExt, levert[i].y, levert[i].z);
        mesh.SetVertices(new List<Vector3>(levert));
    }

    protected virtual void loadOriginaryMesh()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        originaryVertices = mesh.vertices;
    }


    protected virtual int lunghezzaTesto
    {
		get {
			return campoTesto.text.Length;
		}
    }

    public GameObject bucoVarPrefab, bucoVarAngPrefab, bucoVarCircPrefab, bucoVarDDPrefab;

	public virtual void reExtendToMatchText(){
		adaptText ();
		extendToMatchText ();
	}

	protected virtual void adaptText(){
		var builder = new System.Text.StringBuilder ();
		char c = testo [0];
		var goingThrough = false;
		var varIndex = 0;
		for (int i = 0; i < testo.Length; i++) {
			c = testo [i];
			if (varStarts.Contains (c)) {
				builder.Append (new string(' ', slotVariabili [varIndex++].lunghezza));
				goingThrough = true;
			} else if (varEnds.Contains (c)) {
				goingThrough = false;
			} else if (!goingThrough) {
				builder.Append (c);
			}
		}
		campoTesto.text = builder.ToString ();
	}

	private char[] varStarts = new char[]{'[', '<', '(', '{'};
	private char[] varEnds   = new char[]{']', '>', ')', '}'};

    protected virtual void evaluateVars(string testo, int posBaseX, int posBaseY)
    {
        Bucovar curBucoVar = null;
        int inizioBucoVar = 0;
        for (int i = 0; i < testo.Length; i++)
        {
            if (testo[i].Equals('['))
            {
                curBucoVar = GameObject.Instantiate(bucoVarPrefab).GetComponent<Bucovar>();
                curBucoVar.transform.position = this.transform.position + new Vector3(posBaseX + i, posBaseY, 0);
                inizioBucoVar = i;
            }
            else if (testo[i].Equals(']'))
            {
				curBucoVar.lunghezzaOriginale = (i - inizioBucoVar + 1);
                curBucoVar.extend();
                curBucoVar.transform.SetParent(this.transform);
				slotVariabili.Add (curBucoVar);
            }
            else if (testo[i].Equals('<'))
            {
                curBucoVar = GameObject.Instantiate(bucoVarAngPrefab).GetComponent<Bucovar>();
                curBucoVar.transform.position = this.transform.position + new Vector3(posBaseX + i, posBaseY, 0);
                inizioBucoVar = i;
            }
            else if (testo[i].Equals('>'))
            {
				curBucoVar.lunghezzaOriginale = (i - inizioBucoVar + 1);
                curBucoVar.extend();
                curBucoVar.transform.SetParent(this.transform);
				curBucoVar.OnLunghezzaChange += reExtendToMatchText;
				slotVariabili.Add (curBucoVar);
            }
            else if (testo[i].Equals('('))
            {
                curBucoVar = GameObject.Instantiate(bucoVarCircPrefab).GetComponent<Bucovar>();
                curBucoVar.transform.position = this.transform.position + new Vector3(posBaseX + i, posBaseY, 0);
                inizioBucoVar = i;
            }
            else if (testo[i].Equals(')'))
            {
				curBucoVar.lunghezzaOriginale = (i - inizioBucoVar + 1);
                curBucoVar.extend();
                curBucoVar.transform.SetParent(this.transform);
				slotVariabili.Add (curBucoVar);
            }
				else if (testo[i].Equals('{') && curBucoVar == null)
            {
                curBucoVar = GameObject.Instantiate(bucoVarDDPrefab).GetComponent<Bucovar>();
                curBucoVar.transform.position = this.transform.position + new Vector3(posBaseX + i, posBaseY, 0);
                inizioBucoVar = i;
            }
				else if (testo[i].Equals('}') && curBucoVar != null)
            {
				curBucoVar.lunghezzaOriginale = (i - inizioBucoVar + 1);
                curBucoVar.extend();
                curBucoVar.transform.SetParent(this.transform);
				slotVariabili.Add (curBucoVar);
            }
        }
    }

    protected virtual void evaluateLastBlock()
    {
        List<int> verticesToEdit = new List<int>();

        int minZ = int.MaxValue;
        for (int i = 0; i < originaryVertices.Length; i++)
        {
            var vertex = originaryVertices[i];
            if (Mathf.RoundToInt(vertex.z) < minZ)
                minZ = Mathf.RoundToInt(vertex.z);
        }

        for (int i = 0; i < originaryVertices.Length; i++)
        {
            var vertex = originaryVertices[i];
            if (Mathf.RoundToInt(vertex.z) == minZ)
                verticesToEdit.Add(i);
        }

        var levert = mesh.vertices;
        foreach (var i in verticesToEdit)
            levert[i] = new Vector3(levert[i].x, levert[i].y, levert[i].z + 1);
        mesh.SetVertices(new List<Vector3>(levert));
    }

    protected int offsetTestoBaseX = 1;



    // Use this for initialization
    protected virtual void Start()
    {
		slotVariabili = new List<Bucovar> ();
        dente.setNext = setNext;
        dente.unsetNext = unsetNext;
        spazioDente.setPrevious = setPrevious;
        name = testo;
        loadOriginaryMesh();
		campoTesto.text = testo;
        extendToMatchText();
        if (lastBlock) evaluateLastBlock();
        evaluateVars(testo, offsetTestoBaseX, 0);
    }


    protected bool initialised = false;
    // Update is called once per frame
    protected virtual void Update()
    {
        if (!initialised)
        {
            initialised = true;
            this.gameObject.AddComponent<MeshCollider>();
        }
    }
}
