using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocco : MonoBehaviour {

	public Blocco next;
	public bool first = true;
	public string testo;
	public UnityEngine.UI.Text campoTesto;
	protected Mesh mesh;
	protected float dongExt;
	protected int lunghezzaTesto;
	protected float deformConst = 1;
	protected Vector3[] originaryVertices;
	public bool lastBlock = false;

	public void setNext(Blocco candidateNext){
		Debug.Log (testo + ".setNext(" + candidateNext.testo + ")");
		candidateNext.gameObject.transform.position = this.transform.position + new Vector3 (0, -2, 0);
		candidateNext.gameObject.transform.rotation = this.transform.rotation;
		{
			var denti = candidateNext.GetComponentsInChildren<Dente> ();
			var spaziDenti = candidateNext.GetComponentsInChildren<SpazioDente> ();
			foreach (var dente in denti) {
				if (dente.currentlyHighlighted) {
					dente.ExitTrigger (dente.currentlyHighlighted.gameObject.GetComponent<Collider> ());
				}
			}
			foreach (var spazioDente in spaziDenti) {
				if (spazioDente.currentlyHighlighted) {
					spazioDente.ExitTrigger (spazioDente.currentlyHighlighted.gameObject.GetComponent<Collider> ());
				}
			}
		}
		var oldNext = next;
		next = candidateNext;
		candidateNext.first = false;
		if (oldNext) {
			next.setNext (oldNext);
			var denti = oldNext.GetComponentsInChildren<Dente> ();
			var spaziDenti = oldNext.GetComponentsInChildren<SpazioDente> ();
			foreach (var dente in denti) {
				if (dente.currentlyHighlighted) {
					dente.ExitTrigger (dente.currentlyHighlighted.gameObject.GetComponent<Collider> ());
				}
			}
			foreach (var spazioDente in spaziDenti) {
				if (spazioDente.currentlyHighlighted) {
					spazioDente.ExitTrigger (spazioDente.currentlyHighlighted.gameObject.GetComponent<Collider> ());
				}
			}
		}
		var dentix = gameObject.GetComponentsInChildren<Dente> ();
		var spaziDentix = gameObject.GetComponentsInChildren<SpazioDente> ();
		foreach (var dente in dentix) {
			if (dente.currentlyHighlighted) {
				dente.ExitTrigger (dente.currentlyHighlighted.gameObject.GetComponent<Collider> ());
			}
		}
		foreach (var spazioDente in spaziDentix) {
			if (spazioDente.currentlyHighlighted) {
				spazioDente.ExitTrigger (spazioDente.currentlyHighlighted.gameObject.GetComponent<Collider> ());
			}
		}
	}

	public void unsetNext(){
		next.first = true;
		next = null;
	}

	public void setPrevious(Blocco candidatePrevious){
		first = false;
		Debug.Log (testo + ".setPrevious(" + candidatePrevious.testo + ")");
		candidatePrevious.next = this;
		candidatePrevious.transform.position = this.transform.position + new Vector3 (0, 2, 0);
		candidatePrevious.transform.rotation = this.transform.rotation;
		var denti = candidatePrevious.gameObject.GetComponentsInChildren<Dente> ();
		var spaziDenti = candidatePrevious.gameObject.GetComponentsInChildren<SpazioDente> ();
		foreach (var dente in denti) {
			if (dente.currentlyHighlighted) {
				dente.ExitTrigger (dente.currentlyHighlighted.gameObject.GetComponent<Collider> ());
			}
		}
		foreach (var spazioDente in spaziDenti) {
			if (spazioDente.currentlyHighlighted) {
				spazioDente.ExitTrigger (spazioDente.currentlyHighlighted.gameObject.GetComponent<Collider> ());
			}
		}
	}

	protected virtual void extendToMatchText(){
		campoTesto.text = testo;
		dongExt = lunghezzaTesto * deformConst;
		if (dongExt < 1)
			dongExt = 1;


		List<int> verticesToEdit = new List<int> ();

		int maxX = int.MinValue;
		for (int i = 0; i < originaryVertices.Length; i++) {
			var vertex = originaryVertices [i];
			if (Mathf.RoundToInt (vertex.x) > maxX)
				maxX = Mathf.RoundToInt (vertex.x);
		}

		for (int i = 0; i < originaryVertices.Length; i++) {
			var vertex = originaryVertices [i];
			if (Mathf.RoundToInt(vertex.x) == maxX)
				verticesToEdit.Add (i);
		}

		var levert = mesh.vertices;
		foreach (var i in verticesToEdit)
			levert[i] = new Vector3 (levert[i].x + dongExt, levert[i].y, levert[i].z);
		mesh.SetVertices (new List<Vector3>(levert));
	}

	protected virtual void loadOriginaryMesh(){
		mesh = GetComponent<MeshFilter> ().mesh;
		originaryVertices = mesh.vertices;
	}


	protected virtual int calcolaLunghezzaTesto(){
		return testo.Length;
	}

	public GameObject bucoVarPrefab, bucoVarAngPrefab, bucoVarCircPrefab, bucoVarDDPrefab;

	protected virtual void evaluateVars(string testo, int posBaseX, int posBaseY){
		Bucovar curBucoVar = null;
		int inizioBucoVar = 0;
		for (int i = 0; i < testo.Length; i++) {
			if (testo [i].Equals ('[')) {
				curBucoVar = GameObject.Instantiate (bucoVarPrefab).GetComponent<Bucovar> ();
				curBucoVar.transform.position = this.transform.position + new Vector3 (posBaseX + i, posBaseY, 0);
				inizioBucoVar = i;
			} else if (testo [i].Equals (']')) {
				curBucoVar.lunghezza = (i - inizioBucoVar + 1);
				curBucoVar.extend ();
				curBucoVar.transform.SetParent (this.transform);
			} else if (testo [i].Equals ('<')) {
				curBucoVar = GameObject.Instantiate (bucoVarAngPrefab).GetComponent<Bucovar> ();
				curBucoVar.transform.position = this.transform.position + new Vector3 (posBaseX + i, posBaseY, 0);
				inizioBucoVar = i;
			} else if (testo [i].Equals ('>')) {
				curBucoVar.lunghezza = (i - inizioBucoVar + 1);
				curBucoVar.extend ();
				curBucoVar.transform.SetParent (this.transform);
			} else if (testo [i].Equals ('(')) {
				curBucoVar = GameObject.Instantiate (bucoVarCircPrefab).GetComponent<Bucovar> ();
				curBucoVar.transform.position = this.transform.position + new Vector3 (posBaseX + i, posBaseY, 0);
				inizioBucoVar = i;
			} else if (testo [i].Equals (')')) {
				curBucoVar.lunghezza = (i - inizioBucoVar + 1);
				curBucoVar.extend ();
				curBucoVar.transform.SetParent (this.transform);
			}else if (testo [i].Equals ('|') && curBucoVar == null) {
				curBucoVar = GameObject.Instantiate (bucoVarDDPrefab).GetComponent<Bucovar> ();
				curBucoVar.transform.position = this.transform.position + new Vector3 (posBaseX + i, posBaseY, 0);
				inizioBucoVar = i;
			} else if (testo [i].Equals ('|') && curBucoVar != null) {
				curBucoVar.lunghezza = (i - inizioBucoVar + 1);
				curBucoVar.extend ();
				curBucoVar.transform.SetParent (this.transform);
			}
		}
	}

	protected virtual void evaluateLastBlock(){
		List<int> verticesToEdit = new List<int> ();

		int minZ = int.MaxValue;
		for (int i = 0; i < originaryVertices.Length; i++) {
			var vertex = originaryVertices [i];
			if (Mathf.RoundToInt (vertex.z) < minZ)
				minZ = Mathf.RoundToInt (vertex.z);
		}
			
		for (int i = 0; i < originaryVertices.Length; i++) {
			var vertex = originaryVertices [i];
			if (Mathf.RoundToInt(vertex.z) == minZ)
				verticesToEdit.Add (i);
		}

		var levert = mesh.vertices;
		foreach (var i in verticesToEdit)
			levert[i] = new Vector3 (levert[i].x, levert[i].y, levert[i].z + 1);
		mesh.SetVertices (new List<Vector3>(levert));
	}

	protected int offsetTestoBaseX = 1;

	// Use this for initialization
	protected virtual void Start () {
		lunghezzaTesto = calcolaLunghezzaTesto ();
		loadOriginaryMesh ();
		extendToMatchText ();
		if (lastBlock) evaluateLastBlock ();
		evaluateVars (testo, offsetTestoBaseX, 0); 
	}


	protected bool initialised = false;
	// Update is called once per frame
	protected virtual void Update () {
		if (!initialised) {
			initialised = true;
			this.gameObject.AddComponent<MeshCollider> ();
		}
	}
}
