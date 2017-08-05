using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSelectableBlock : LaserSelectable
{
    public override void SelectA()
    {

    }
    public override void SelectB()
    {
        ScriptingElementWrapper bwrap = gameObject.GetComponent<view.BlockWrapper>();
        view.ActorWrapper owner = null;
        if (bwrap != null)
            owner = (bwrap as view.BlockWrapper).Owner;
        else {
            bwrap = gameObject.GetComponent<view.ReferenceWrapper>();
            if (bwrap != null)
                owner = (bwrap as view.ReferenceWrapper).Owner;
        }
        if (owner != null)
            owner.RemoveScriptingElement(bwrap);
        Destroy(gameObject);
    }
}
