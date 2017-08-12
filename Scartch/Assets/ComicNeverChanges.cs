using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComicNeverChanges : MonoBehaviour {

    public Vector3 Rotation, Scale;
	
	// Update is called once per frame
	void Update () {
        this.transform.eulerAngles = Rotation;
        this.transform.localScale = new Vector3(Scale.x / this.transform.parent.localScale.x, Scale.y / this.transform.parent.localScale.y, Scale.z / this.transform.parent.localScale.z);
	}
}
