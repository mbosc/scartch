using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucovarDD : ReferenceContainer {

	public override void extend(){
		loadOriginaryMesh ();
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
			levert[i] = new Vector3 (levert[i].x - lunghezza + 3, levert[i].y, levert[i].z);
		mesh.SetVertices (new List<Vector3>(levert));

		GameObject dropB = GameObject.Instantiate (dropButton);
		dropB.transform.position = this.transform.position + new Vector3((lunghezza-2)+0.5f, 0, 0);
		dropB.transform.SetParent (this.transform);

	}

	public GameObject dropButton;
}
