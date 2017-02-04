﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;
using System;

namespace view
{

    public class SizeVariableWrapper : NumberReferenceWrapper
    {
        //mettere riferimento a una classe nuova derivata da Variable che faccia da proxy per owner.Position.x;
        protected override void Start()
        {
			GetComponent<Renderer> ().material = ResourceManager.Instance.bloccoMovimento;
			reference = new DynamicNumberVariable("_dim", () => Owner.actor.Scale);
            base.Start();
        }
    }
}
