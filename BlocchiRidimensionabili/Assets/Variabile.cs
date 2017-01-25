using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variabile : Bucovar {

    public string testo;
    public UnityEngine.UI.Text myText;

	public override void extend(){
		loadOriginaryMesh ();
        lunghezza = testo.ToString().Length;
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
			levert[i] = new Vector3 (levert[i].x - lunghezza + 2, levert[i].y, levert[i].z);
		mesh.SetVertices (new List<Vector3>(levert));
        myText.text = testo;
	}

}
