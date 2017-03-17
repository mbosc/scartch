using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;
using System.Linq;
using view;

public class Demo : MonoBehaviour {

    public GameObject block, hat, mblock, mmblock, varcirc, varAng, varSqr;
    public ActorWrapper actor1, actor2, actor3;
    public Transform spawnTransform;
    public static Demo instance;

    private class SumExpression : NumberExpression
    {
        public SumExpression()
        {
            name = "(  ) - (  )";
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
            return a1 - a2;
        }
    }

    public UnityEngine.UI.Text output;

    // Use this for initialization
    void Start () {
        instance = this;

		actor1.Init(new Actor ("Ciro", new Vector3 (0, 0, 0), new Vector3 (0, 0, 0), 1, 100, "", new Model ("s")));
		actor2.Init(new Actor ("Pino", new Vector3 (40, 0, 0), new Vector3 (0, 0, 0), 1, 100, "", new Model ("s")));
		actor3.Init(new Actor ("Lino", new Vector3 (-40, 0, 0), new Vector3 (0, 0, 0), 1, 100, "", new Model ("s")));
//        var b = InstantiateWithComponent<EndPlayModeBlockWrapper>(block);
//        b.GetComponent<BlockWrapper>().Owner = actor;

  //      var c = InstantiateWithComponent<MoveStepsBlockWrapper>(block);
		//c.Init(actor2);

//        var c2 = InstantiateWithComponent<MoveStepsBlockWrapper>(block);
//        c2.GetComponent<BlockWrapper>().Owner = actor;

		model.Environment.Instance.AddVariable(new NumberVariable("globalNumberVariableTwo", 2));
		model.Environment.Instance.AddVariable(new StringVariable("globalStringVariableHello", "Hello"));
		actor3.Actor.AddVariable (new StringVariable ("VariabileDiQuestoTizio", "Salve!"));
//
//        var d = InstantiateWithComponent<OnPlayModeHatWrapper>(hat);
//		d.Init (actor2);

		//var i = InstantiateWithComponent<BounceOnBorderBlockWrapper>(block);
		//i.Init (actor2);


//        var e = Instantiate(varcirc);
//        e.GetComponent<NumberReferenceWrapper>().Init(actor2, new model.NumberVariable("three", 3));
//        var e2 = Instantiate(varcirc);
//        v2 = new DynamicNumberVariable("Posiz. z", () => actor2.Actor.Position.z);
//        e2.GetComponent<NumberReferenceWrapper>().Init(actor2, v2);
//        GameObject.Instantiate(ResourceManager.Instance.varviewer).GetComponent<VariableViewer>().Init(v2);
//        
//
//        var zz = Instantiate(varAng);
//        zz.GetComponent<BooleanReferenceWrapper>().Init(actor2, new model.BooleanVariable("tru story", true));
//
//        var syi = Instantiate(varSqr);
//        syi.GetComponent<StringReferenceWrapper>().Init(actor2, new model.StringVariable("stringo", "stringo"));
//
//        var soso = InstantiateWithComponent<BounceOnBorderBlockWrapper>(block);
//        soso.Init(actor2);

		//var ifels = InstantiateWithComponent<TimeResetBlockWrapper> (block);
		//ifels.Init (actor2);

//        var omega = InstantiateWithComponent<MoveStepsBlockWrapper>(block);
//        omega.Init(actor2);

        //var afispo = Instantiate(varSqr);
        //afispo.GetComponent<StringReferenceWrapper>().Init(actor2, new model.StringVariable("one thing", "one thing"));
        ////settare proprieta' variabili a personaggio
        //confronta come abbiamo fatto con wrapper blocchi

  //      var f = Instantiate(varcirc);
  //      f.GetComponent<NumberReferenceWrapper>().reference = new model.NumberVariable("sd", 1);
        

		//var g = InstantiateWithComponent<IfElseBlockWrapper> (mmblock);
	 //   g.Init(actor2);

//        var z = InstantiateWithComponent<ForeverBlockWrapper>(mblock);
//		z.Init(actor2);

		actor1.HideBlocks ();
		actor2.HideBlocks ();
		actor3.HideBlocks ();

       // FindObjectOfType<Boolpad>().InnerStringChanged += ChangeTxt;
    }

    private void ChangeTxt(string obj)
    {
        output.text = obj;
    }
    private void ChangeTxt(float obj)
    {
        output.text = obj.ToString();
    }

    public static T InstantiateWithComponent<T>(GameObject prefab) where T:MonoBehaviour
    {
        GameObject b = GameObject.Instantiate(prefab);
        b.AddComponent<T>();
        GameObject c = GameObject.Instantiate(b);
        Destroy(b);
		return c.GetComponent<T>();
    }

    NumberVariable v2;

	// Update is called once per frame
	void Update () {
        if (GameObject.Find("LeftHand").GetComponent<NewtonVR.NVRHand>().Inputs[NewtonVR.NVRButtons.X].IsPressed)
            v2.Value += 1;
	}
}
