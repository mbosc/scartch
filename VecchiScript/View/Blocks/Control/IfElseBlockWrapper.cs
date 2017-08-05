using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;
namespace view{

	public class IfElseBlockWrapper : DoubleMouthBlockWrapper{

		public override void Init(ActorWrapper wrapper, bool autoinit = true){
			GetComponent<Renderer> ().material = ResourceManager.Instance.bloccoControllo;
			testo = "se <  >, allora";
			secondoTesto = "altrimenti";
			block = new IfElseBlock ();
			base.Init (wrapper, autoinit);
		}


        private class IfElseBlock : DoubleMouthBlock
        {

            private bool evaluated = false;
            private Block flux;
            public override Block ExecuteAndGetNext()
            {
                if (!evaluated)
                {
                    evaluated = true;
					if (GetReferenceAs<bool>(0))
                    {
                        flux = firstInnerNext;
                    }
                    else
                    {
                        flux = secondInnerNext;
                    }
                }
                if (flux != null)
                {
                    flux = flux.ExecuteAndGetNext();
                    return this;
                }
                else
                {
                    evaluated = false;
                    return Next;
                }

            }

        }
    }
}
