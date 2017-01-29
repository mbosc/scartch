using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace model
{
	namespace blocks
	{
		public class IfBlock : MouthBlock
		{
			private bool evaluated = false;
			private Block flux;
			public override Block ExecuteAndGetNext ()
			{
				if (!evaluated) {
					evaluated = true;
					if ((references [0] as BooleanReference).Evaluate ()) {
						flux = InnerNext;
					}
				}
				if (flux != null) {
					flux = flux.ExecuteAndGetNext ();
					return this;
				} else {
					return Next;
				}
			}
		}
	}
}
