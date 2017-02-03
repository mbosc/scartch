using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;

namespace view
{
	public class IfBlockWrapper : MouthBlockWrapper
	{

		public override void Init(ActorWrapper wrapper, bool autoinit = true)
		{
			testo = "se <  >, allora";
			block = new IfBlock ();
			base.Init (wrapper, autoinit);
		}

		private class IfBlock : MouthBlock
		{
			private bool evaluated = false;
			private Block flux;

			public override Block ExecuteAndGetNext ()
			{
				if (!evaluated) {
					evaluated = true;
					if (GetReferenceAs<bool>(0)) {
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

