using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloccoIfElse : Blocco {

	public int firstStretchSize = 1;
	public int secondStretchSize = 1;
	public string secondoTesto;
	public UnityEngine.UI.Text secondoCampoTesto;

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
		extendToMatchContent ();
		evaluateVars (secondoTesto, offsetTestoBaseX, (-firstStretchSize - 1)*2); 
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
