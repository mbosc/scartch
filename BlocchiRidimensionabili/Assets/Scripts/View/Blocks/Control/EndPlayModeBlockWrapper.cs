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
		public override void Init(ActorWrapper ownerWrapper, bool autoinit = true)
        {
			GetComponent<Renderer> ().material = ResourceManager.Instance.bloccoControllo;
            lastBlock = true;
            testo = "Termina play mode";
            block = new EndPlayModeBlock();
			base.Init(ownerWrapper, autoinit);
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