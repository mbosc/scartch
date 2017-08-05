using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;

namespace view
{
	public class RepeatBlockWrapper : MouthBlockWrapper
	{

		public override void Init(ActorWrapper wrapper, bool autoinit = true)
		{
			GetComponent<Renderer> ().material = ResourceManager.Instance.bloccoControllo;
			testo = "Ripeti (  ) volte";
			block = new RepeatBlock ();
			base.Init (wrapper, autoinit);
		}

		private class RepeatBlock : MouthBlock
		{
			private int evaluationsLeft;
			private bool evaluated = false;
			private Block flux;

			public override Block ExecuteAndGetNext ()
			{
				if (!evaluated) {
					evaluationsLeft = Mathf.RoundToInt(GetReferenceAs<float> (0));
					evaluated = true;
				}
				if (evaluationsLeft > 0 && flux == null){
						flux = InnerNext;
						evaluationsLeft--;
				}
				if (flux != null) {
					flux = flux.ExecuteAndGetNext ();
					return this;
				} 
				return Next;
			}
		}
	}
}

