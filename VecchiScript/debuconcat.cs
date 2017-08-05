using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using view;

public class debuconcat : MonoBehaviour {

    public ReferenceWrapper tet;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            GetComponent<ReferenceContainer>().CompletaCon(tet);
        }	
	}
}
