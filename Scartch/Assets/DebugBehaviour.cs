using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugBehaviour : MonoBehaviour {

    // Use this for initialization
    void Start() {
        StartCoroutine(WaitAndLaunch());
    }

    private IEnumerator WaitAndLaunch()
    {
        Controller.EnvironmentController.Instance.AddActor();
        yield return new WaitForSeconds(1);
        FindObjectOfType<View.ActorViewer>().HitByBlueRay();
        yield return new WaitForSeconds(1);
        FindObjectOfType<View.ActorWindow>().addSEBtn.HitByBlueRay();
        FindObjectOfType<View.ChooseScriptingElementWindow>().bookmarks[0].HitByBlueRay();
        FindObjectOfType<View.ChooseScriptingElementWindow>().voices[0].HitByBlueRay();
        FindObjectOfType<View.ChooseScriptingElementWindow>().voices[6].HitByBlueRay();
        FindObjectOfType<View.ChooseScriptingElementWindow>().voices[6].HitByBlueRay();
        FindObjectOfType<View.ChooseScriptingElementWindow>().voices[6].HitByBlueRay();
        FindObjectOfType<View.ChooseScriptingElementWindow>().voices[6].HitByBlueRay();

    }

    // Update is called once per frame
    void Update () {
		
	}
}
