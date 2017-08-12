using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
using View;
using Controller;

public class Engine : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Actor act = new Actor()
        {
            Name = "Actorino"
        };
        ActorViewer vie = GameObject.Instantiate(ScartchResourceManager.instance.actorViewer);
        ActorWindow win = GameObject.Instantiate(ScartchResourceManager.instance.actorWindow);
        ActorController cnt = new ActorController(act, vie, win);
	}
	
}
