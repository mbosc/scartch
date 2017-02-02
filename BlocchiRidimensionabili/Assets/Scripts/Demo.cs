using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;
using view;

public class Demo : MonoBehaviour {

    public GameObject block, hat, mblock, mmblock, varcirc;
    public ActorWrapper actor;
    public Transform spawnTransform;
    
    // Use this for initialization
	void Start () {
        actor.actor = new Actor(new Model());

        var b = InstantiateWithComponent<EndPlayModeBlockWrapper>(block);
        b.GetComponent<BlockWrapper>().Owner = actor;

        var c = InstantiateWithComponent<MoveStepsBlockWrapper>(block);
        c.GetComponent<BlockWrapper>().Owner = actor;
        var c2 = InstantiateWithComponent<MoveStepsBlockWrapper>(block);
        c2.GetComponent<BlockWrapper>().Owner = actor;


        var d = InstantiateWithComponent<OnPlayModeHatWrapper>(hat);

        var e = Instantiate(varcirc);
        e.GetComponent<NumberReferenceWrapper>().reference = new model.NumberVariable("hello", 3);

        var f = Instantiate(varcirc);
        f.GetComponent<NumberReferenceWrapper>().reference = new model.NumberVariable("sd", -10);

        

        var z = InstantiateWithComponent<ForeverBlockWrapper>(mblock);

        
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
