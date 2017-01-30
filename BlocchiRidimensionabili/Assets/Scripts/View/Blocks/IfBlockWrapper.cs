using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;

namespace view
{
	public class IfBlockWrapper : MouthBlockWrapper
	{

		protected override void Start ()
		{
			testo = "se <  >, allora";
			block = new IfBlock ();
			base.Start ();
		}

		private class IfBlock : MouthBlock
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
					evaluated = false;
					return Next;
				}
			}
		}
	}
}

