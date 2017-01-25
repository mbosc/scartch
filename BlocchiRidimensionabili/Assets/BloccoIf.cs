using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloccoIf : Blocco {

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
			levert[i] = new Vector3 (levert[i].x, levert[i].y, levert[i].z  - (stretchSize-1)*2);
		mesh.SetVertices (new List<Vector3>(levert));
	}

	protected override void Start ()
	{
		offsetTestoBaseX = 2;
		base.Start ();
		extendToMatchContent ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
