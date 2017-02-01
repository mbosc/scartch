using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;
using System;

namespace view
{
    public class WaitForSecondsBlockWrapper : BlockWrapper
    {

        // Use this for initialization
        protected override void Start()
        {
            testo = "Aspetta (  ) secondi";
            block = new WaitForSecondsBlock();
            base.Start();
        }

        private class WaitForSecondsBlock : Block
        {
            public WaitForSecondsBlock()
            {
            }
            public override Block ExecuteAndGetNext()
            {
                model.Environment.Instance.Wait(((references[0]) as NumberReference).Evaluate());
                return Next;
            }
        }
    }
}