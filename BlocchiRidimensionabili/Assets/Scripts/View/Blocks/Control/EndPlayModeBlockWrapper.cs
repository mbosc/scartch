using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;
using System;

namespace view
{
    public class EndPlayModeBlockWrapper : BlockWrapper
    {

        // Use this for initialization
        protected override void Start()
        {
            lastBlock = true;
            testo = "Termina play mode";
            block = new EndPlayModeBlock();
            base.Start();
        }

        public class EndPlayModeBlock : Block
        {
            public EndPlayModeBlock()
            {
            }
            public override Block ExecuteAndGetNext()
            {
                model.Environment.Instance.PlayMode = false;
                return Next;
            }
        }
    }
}