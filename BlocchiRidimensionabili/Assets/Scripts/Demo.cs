using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;
using view;

public class Demo : MonoBehaviour {

    public GameObject block, hat, mblock, mmblock, varcirc, varAng, varSqr;
    public ActorWrapper actor1, actor2, actor3;
    public Transform spawnTransform;

    private class SumExpression : NumberExpression
    {
        public SumExpression()
        {
            name = "(  ) + (  )";
        }

        private string name;
        public override string Name
        {
            get
            {
                return name;
            }
        }

        public override float Evaluate()
        {
            var a1 = GetReferenceAs<float>(0);
            var a2 = GetReferenceAs<float>(1);
            return a1 + a2;
        }
    }

    // Use this for initialization
    void Start () {
		actor1.actor = new Actor (new Vector3 (0, 0, 0), new Vector3 (0, 0, 0), 1, 100, "", new Model ("s"));
		actor2.actor = new Actor (new Vector3 (40, 0, 0), new Vector3 (0, 0, 0), 1, 100, "", new Model ("s"));
		actor3.actor = new Actor (new Vector3 (-40, 0, 0), new Vector3 (0, 0, 0), 1, 100, "", new Model ("s"));
//        var b = InstantiateWithComponent<EndPlayModeBlockWrapper>(block);
//        b.GetComponent<BlockWrapper>().Owner = actor;

        var c = InstantiateWithComponent<MoveStepsBlockWrapper>(block);
		c.Init(actor2);

//        var c2 = InstantiateWithComponent<MoveStepsBlockWrapper>(block);
//        c2.GetComponent<BlockWrapper>().Owner = actor;


        var d = InstantiateWithComponent<OnPlayModeHatWrapper>(hat);
		d.Init (actor2);

		var i = InstantiateWithComponent<BounceOnBorderBlockWrapper>(block);
		i.Init (actor2);


        var e = Instantiate(varcirc);
        e.GetComponent<NumberReferenceWrapper>().Init(actor2, new model.NumberVariable("three", 3));
        var e2 = Instantiate(varcirc);
        e2.GetComponent<NumberReferenceWrapper>().Init(actor2, new model.NumberVariable("two", 2));
        var e3 = Instantiate(varcirc);
        e3.GetComponent<NumberReferenceWrapper>().Init(actor2, new SumExpression());

        var zz = Instantiate(varAng);
        zz.GetComponent<BooleanReferenceWrapper>().Init(actor2, new model.BooleanVariable("tru story", true));

        var soso = InstantiateWithComponent<SayBlockWrapper>(block);
        soso.Init(actor2);

		var ifels = InstantiateWithComponent<TimeResetBlockWrapper> (block);
		ifels.Init (actor2);

        var afispo = Instantiate(varSqr);
        afispo.GetComponent<StringReferenceWrapper>().Init(actor2, new model.StringVariable("one thing", "one thing"));
        //settare proprieta' variabili a personaggio
        //confronta come abbiamo fatto con wrapper blocchi

  //      var f = Instantiate(varcirc);
  //      f.GetComponent<NumberReferenceWrapper>().reference = new model.NumberVariable("sd", 1);


		var g = InstantiateWithComponent<StopAllSoundsBlockWrapper> (block);
		g.Init(actor2);

        var z = InstantiateWithComponent<ForeverBlockWrapper>(mblock);
		z.Init(actor2);

		actor1.HideBlocks ();
		actor2.HideBlocks ();
		actor3.HideBlocks ();

        
    }

    private T InstantiateWithComponent<T>(GameObject prefab) where T:MonoBehaviour
    {
        GameObject b = GameObject.Instantiate(prefab);
        b.AddComponent<T>();
        GameObject c = GameObject.Instantiate(b);
        Destroy(b);
		return c.GetComponent<T>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
