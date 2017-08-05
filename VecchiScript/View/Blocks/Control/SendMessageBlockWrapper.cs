﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;
using System;

namespace view
{
    public class SendMessageBlockWrapper : BlockWrapper
    {

        // Use this for initialization
		public override void Init(ActorWrapper wrapper, bool autoinit = true){
			GetComponent<Renderer> ().material = ResourceManager.Instance.bloccoControllo;
			testo = "Aspetta [  ] secondi";
			block = new SendMessageBlock ();
			base.Init (wrapper, autoinit);
			}

        private class SendMessageBlock : Block
        {
            public SendMessageBlock()
            {
            }
            public override Block ExecuteAndGetNext()
            {
                model.Environment.Instance.EvaluationEngine.EvaluateMessage(GetReferenceAs<string>(0));
                return Next;
            }
        }
    }
}