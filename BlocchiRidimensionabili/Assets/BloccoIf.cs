using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloccoIf : Blocco {

	public Blocco internalNext;
	public Dente denteInterno;
	protected float nextBlockInternoOffsetX = 1;
	protected float nextBlockInternoOffsetY = -2;

	public virtual void setNextInterno(Blocco candidateNext){
		Debug.Log (testo + ".setNextInterno(" + candidateNext.testo + ")");

		candidateNext.gameObject.transform.position = this.transform.position + new Vector3 (nextBlockInternoOffsetX, nextBlockInternoOffsetY, 0);
		candidateNext.gameObject.transform.rotation = this.transform.rotation;

		var oldNext = internalNext;
		internalNext = candidateNext;
		AumentaLunghezza (internalNext);
		candidateNext.spazioDente.Receiving = false;
		denteInterno.Receiving = false;
		denteInterno.Receiving = true;
		if (oldNext) {
			internalNext.setNext (oldNext);
		}
	}

	public void AumentaLunghezza(Blocco next){
		Debug.Log ("AumentaLunghezza");
		var lung = 0;
		Blocco catena = internalNext;
		while (catena) {
			lung++;
			catena = catena.next;
		}
		stretchSize = lung;
		extendToMatchContent ();
		next.dente.setNext += AumentaLunghezza;
		next.dente.unsetNext += DiminuisciLunghezza;
	}

	public void DiminuisciLunghezza(){
		Debug.Log ("DiminuisciLunghezza");
		var lung = 0;
		Blocco catena = internalNext;
		while (catena) {
			lung++;
			catena = catena.next;
		}
		stretchSize = lung;
		extendToMatchContent ();
	}

	public override List<Blocco> linkedBlocks {
		get {
			var ex = new List<Blocco> ();
			ex.Add (next);
			ex.Add (internalNext);
			return ex;
		}
	}

	public virtual void unsetNextInterno(){
		Blocco catena = internalNext;
		while (catena) {
			catena.dente.setNext -= AumentaLunghezza;
			catena.dente.unsetNext -= DiminuisciLunghezza;
			catena = catena.next;
		}
		internalNext.spazioDente.Receiving = true;
		internalNext = null;
	}

	public int stretchSize = 1;

	protected virtual void extendToMatchContent(){

		if (stretchSize < 1)
			stretchSize = 1;

		List<int> verticesToStretch = new List<int> ();
		for (int i = 0; i < originaryVertices.Length; i++) {
			var vertex = originaryVertices [i];
			if (Mathf.RoundToInt(vertex.z) <= -3)
				verticesToStretch.Add (i);
		}

		var levert = mesh.vertices;
		foreach (var i in verticesToStretch)
			levert[i] = new Vector3 (levert[i].x, levert[i].y, originaryVertices[i].z  - (stretchSize-1)*2);
		dente.gameObject.transform.localPosition = new Vector3(dente.gameObject.transform.localPosition.x, dente.gameObject.transform.localPosition.y,  0.5f -4 -stretchSize*2);
		mesh.SetVertices (new List<Vector3>(levert));
		if (GetComponent<MeshCollider> ())
			Destroy (GetComponent<MeshCollider> ());
		initialised = false;
	}

	protected override void Start ()
	{
		offsetTestoBaseX = 2;
		base.Start ();
		denteInterno.setNext = setNextInterno;
		denteInterno.unsetNext = unsetNextInterno;
		extendToMatchContent ();
		nextBlockOffsetY = -4 - stretchSize * 2;

	}

}
