using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View.Resources;

public class ScriptingElementRayRec : RayHittable
{
    private ScriptingElementViewer viewer;
    public ScriptingElementViewer winOverload;

    public override void HitByBlueRay()
    {
        if (viewer is View.BlockViewer)
            (viewer as View.BlockViewer).Test();
    }

    public override void HitByRedRay()
    {
        viewer.Delete();
    }

    private void Start()
    {
        if (winOverload != null)
            viewer = winOverload;
        else
            viewer = this.GetComponent<ScriptingElementViewer>();
        viewer.gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

}

