using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MultiCam : MonoBehaviour {

    public KeyCode switchKey;
    private Queue<Transform> positions;
    public Transform[] Poses;

	// Use this for initialization
	void Start () {
        positions = new Queue<Transform>();
        Poses.ToList().ForEach(positions.Enqueue);
        switchPosition();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(switchKey))
            switchPosition();
	}

    private void switchPosition()
    {
        var top = positions.Dequeue();
        positions.Enqueue(top);
        this.transform.position = top.position;
        this.transform.rotation = top.rotation;
    }
}
