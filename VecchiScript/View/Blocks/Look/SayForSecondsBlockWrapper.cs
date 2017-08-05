using System;
using UnityEngine;
using model;
using System.Linq;

namespace view
{
	public class SayForSecondsBlockWrapper : BlockWrapper
	{
		public override void Init(ActorWrapper wrapper, bool autoinit = true){
			GetComponent<Renderer> ().material = ResourceManager.Instance.bloccoAspetto;
			testo = "Di' [  ] per (  ) secondi";
			block = new SayForSecondsBlock (wrapper.Actor);
			base.Init (wrapper,autoinit);
		}

		private class SayForSecondsBlock : Block
		{
			public SayForSecondsBlock(Actor owner){
				this.owner = owner;
			}
			public override Block ExecuteAndGetNext()
			{
                owner.Message = GetReferenceAs<string>(0);
                owner.ShowMessage();
				model.Environment.Instance.Wait (GetReferenceAs<float> (1));
				owner.HideMessage ();
				return Next;
			}
		}
	}
}
