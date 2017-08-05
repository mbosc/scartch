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
		public override void Init(ActorWrapper wrapper, bool autoinit = true){
			GetComponent<Renderer> ().material = ResourceManager.Instance.bloccoControllo;
			testo = "Aspetta (  ) secondi";
			block = new WaitForSecondsBlock ();
			base.Init (wrapper, autoinit);
			}

        private class WaitForSecondsBlock : Block
        {
            public WaitForSecondsBlock()
            {
            }
            public override Block ExecuteAndGetNext()
            {
                model.Environment.Instance.Wait(GetReferenceAs<float>(0));
                return Next;
            }
        }
    }
}
