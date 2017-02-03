using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;
namespace view{

	public class IfElseBlockWrapper : DoubleMouthBlockWrapper{

		public override void Init(ActorWrapper wrapper, bool autoinit = true){
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
                    if ((references[0] as BooleanReference).Evaluate())
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
