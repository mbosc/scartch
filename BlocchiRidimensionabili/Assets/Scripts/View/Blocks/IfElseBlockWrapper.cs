using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;
namespace view{

	public class IfElseBlockWrapper : DoubleMouthBlockWrapper{

		protected override void Start(){
			testo = "se <  >, allora";
			secondoTesto = "altrimenti";
			block = new IfElseBlock ();
			base.Start ();
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
