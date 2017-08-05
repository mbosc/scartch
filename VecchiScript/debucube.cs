using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class debucube : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var acc = new Dictionary<Vector3, int>();
        var Mesh = this.GetComponent<MeshFilter>().mesh;
        Mesh.vertices.ToList().ForEach(s => { if (acc.ContainsKey(s)) acc[s]++;
            else acc.Add(s, 1);
        });
        acc.Keys.ToList().ForEach(s => Debug.Log(s + " " + acc[s]));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
