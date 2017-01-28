﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucovar : MonoBehaviour {

	private int _lunghezza = 1;
	public System.Action OnLunghezzaChange;
	public int lunghezza {
		get {
			return _lunghezza;
		}
		set {
			_lunghezza = value;
			if (OnLunghezzaChange != null)
				OnLunghezzaChange ();
		}
	}
	public int lunghezzaOriginale;
	protected Mesh mesh;
	protected Vector3[] originaryVertices;
	public Variabile variabile;
	private bool initialised = false;

	private void Init(){
		_lunghezza = lunghezzaOriginale;
		loadOriginaryMesh ();
		def = GetComponent<Renderer> ().material.color;
	}

	public virtual void extend(){
		if (!initialised)
			Init ();
		if (lunghezza < 1)
			lunghezza = 1;
		
		List<int> verticesToEdit = new List<int> ();

		for (int i = 0; i < originaryVertices.Length; i++) {
			var vertex = originaryVertices [i];
			if (vertex.x < 0)
				verticesToEdit.Add (i);
		}

		var levert = mesh.vertices;
		foreach (var i in verticesToEdit)
			levert[i] = new Vector3 (originaryVertices[i].x - lunghezza + 2, levert[i].y, levert[i].z);
		mesh.SetVertices (new List<Vector3>(levert));

		//recompute mesh collider
		if (GetComponent<MeshCollider> ())
			Destroy (GetComponent<MeshCollider> ());
		gameObject.AddComponent<MeshCollider> ();
		GetComponent<MeshCollider> ().convex = true;
		GetComponent<MeshCollider> ().isTrigger = true;
	}

	protected virtual void loadOriginaryMesh(){
		mesh = GetComponent<MeshFilter> ().mesh;
		originaryVertices = mesh.vertices;
	}
		
	private Color def;
	public virtual void setHighlightVisible(bool v){ 
		GetComponent<MeshRenderer> ().enabled = true;
		Color col = v ? Color.green : def;
		GetComponent<Renderer> ().material.color = col;
	}
		
	public virtual void CompletaCon (Variabile variabile){
		lunghezza = variabile.lunghezza;
		variabile.transform.position = this.transform.position;
		this.variabile = variabile;
		GetComponent<MeshRenderer> ().enabled = false;
	}

	public virtual void Svuota (){
		lunghezza = lunghezzaOriginale;
//		extend ();
		this.variabile = null;

	}

	// Use this for initialization
	protected virtual void Start () {
		
	}


}