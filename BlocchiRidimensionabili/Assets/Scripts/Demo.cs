﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;
using view;

public class Demo : MonoBehaviour {

    public GameObject block, hat, mblock, mmblock, varcirc;
    public ActorWrapper actor1, actor2, actor3;
    public Transform spawnTransform;
    
    // Use this for initialization
	void Start () {
		actor1.actor = new Actor (new Vector3 (0, 0, 0), new Vector3 (0, 0, 0), 1, 100, "", new Model ("s"));
		actor2.actor = new Actor (new Vector3 (20, 0, 0), new Vector3 (0, 0, 0), 1, 100, "", new Model ("s"));
		actor3.actor = new Actor (new Vector3 (-20, 0, 0), new Vector3 (0, 0, 0), 1, 100, "", new Model ("s"));
//        var b = InstantiateWithComponent<EndPlayModeBlockWrapper>(block);
//        b.GetComponent<BlockWrapper>().Owner = actor;

        var c = InstantiateWithComponent<MoveStepsBlockWrapper>(block);
        c.GetComponent<BlockWrapper>().Owner = actor1;

//        var c2 = InstantiateWithComponent<MoveStepsBlockWrapper>(block);
//        c2.GetComponent<BlockWrapper>().Owner = actor;


        var d = InstantiateWithComponent<OnPlayModeHatWrapper>(hat);
		d.GetComponent<HatWrapper> ().Owner = actor2;

//        var e = Instantiate(varcirc);
//        e.GetComponent<NumberReferenceWrapper>().reference = new model.NumberVariable("hello", 3);

  //      var f = Instantiate(varcirc);
  //      f.GetComponent<NumberReferenceWrapper>().reference = new model.NumberVariable("sd", 1);


		var g = InstantiateWithComponent<BounceOnBorderBlockWrapper> (block);
		g.GetComponent<BlockWrapper> ().Owner = actor2;

        //var z = InstantiateWithComponent<ForeverBlockWrapper>(mblock);

		actor1.HideBlocks ();
		actor2.HideBlocks ();
		actor3.HideBlocks ();

        
    }

    private GameObject InstantiateWithComponent<T>(GameObject prefab) where T:MonoBehaviour
    {
        GameObject b = GameObject.Instantiate(prefab);
        b.AddComponent<T>();
        GameObject c = GameObject.Instantiate(b);
        Destroy(b);
        return c;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
