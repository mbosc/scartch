using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadResetterino : MonoBehaviour {
	void Start () {
        transform.position = Vector3.zero;
        transform.eulerAngles = Vector3.zero;
	}
}
