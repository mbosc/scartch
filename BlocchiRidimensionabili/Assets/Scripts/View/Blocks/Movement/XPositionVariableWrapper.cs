﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;
using System;

namespace view
{

    public class XPositionVariableWrapper : NumberReferenceWrapper
    {
        //mettere riferimento a una classe nuova derivata da Variable che faccia da proxy per owner.Position.x;
        protected override void Start()
        {
			GetComponent<Renderer> ().material = ResourceManager.Instance.bloccoMovimento;
            reference = new DynamicNumberVariable("_px", () => Owner.Actor.Position.x);
            base.Start();
        }
    }
}

namespace model
{
    public class DynamicNumberVariable : NumberVariable
    {
        private Func<float> valueCalc;
        public DynamicNumberVariable(string name, Func<float> valueCalc) : base(name) {
            if (valueCalc == null)
                throw new Exception("We");// TODO messaggio stupido
            this.valueCalc = valueCalc;
        }
        public override float Value
        {
            get
            {
                OnVariableChanged();
                return valueCalc();
            }

            set
            {
                throw new System.Exception("This variable cannot be accessed");
            }
        }
    }
}