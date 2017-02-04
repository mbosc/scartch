using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;

namespace view
{
	public class WhileBlockWrapper : MouthBlockWrapper
	{

		public override void Init(ActorWrapper wrapper, bool autoinit = true)
		{
			GetComponent<Renderer> ().material = ResourceManager.Instance.bloccoControllo;
			testo = "finché <  >";
			block = new WhileBlock ();
			base.Init (wrapper, autoinit);
		}

		private class WhileBlock : MouthBlock
		{
			private bool evaluated = false;
			private Block flux;

			public override Block ExecuteAndGetNext ()
			{
				if (!evaluated) {
					evaluated = true;
					if (GetReferenceAs<bool> (0)) {
						flux = InnerNext;
					} else {
						return Next;
					}
				}
				if (flux != null) {
					flux = flux.ExecuteAndGetNext ();
					return this;
				} else {
					evaluated = false;
					return this;
				}
			}
		}
	}
}

