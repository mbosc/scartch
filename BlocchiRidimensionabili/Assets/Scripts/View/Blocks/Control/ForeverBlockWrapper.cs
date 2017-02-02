using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;

namespace view
{
	public class ForeverBlockWrapper : MouthBlockWrapper
	{

		protected override void Start ()
		{
			testo = "Per sempre";
			block = new ForeverBlock();
            lastBlock = true;
			base.Start ();
		}

		private class ForeverBlock : MouthBlock
		{


            private Block flux;
			public override Block ExecuteAndGetNext ()
			{
                if (flux == null)
                {
                    flux = innerNext.ExecuteAndGetNext();
                    return this;
                }
                else
                {
                    flux = flux.ExecuteAndGetNext();
                    return this;
                }
			}
		}
	}
}

