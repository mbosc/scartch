using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;
using System;

namespace view
{

    public class ZPositionVariableWrapper : ReferenceWrapper
    {
        //mettere riferimento a una classe nuova derivata da Variable che faccia da proxy per owner.Position.x;
        protected override void Start()
        {
            reference = new DynamicNumberVariable("_pz", () => Owner.actor.Position.z);
            base.Start();
        }
    }
}

