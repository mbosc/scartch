using System;
using UnityEngine;
using model;
using System.Linq;

namespace view
{
	public class SayBlockWrapper : BlockWrapper
	{
		public override void Init(ActorWrapper wrapper, bool autoinit = true){
			GetComponent<Renderer> ().material = ResourceManager.Instance.bloccoAspetto;
			testo = "Di' [  ]";
			block = new SayBlock (wrapper.actor);
			base.Init (wrapper,autoinit);
		}

		private class SayBlock : Block
		{
			public SayBlock(Actor owner){
				this.owner = owner;
			}
			public override Block ExecuteAndGetNext()
			{
                owner.Message = GetReferenceAs<string>(0);
                owner.ShowMessage();
				return Next;
			}
		}
	}
}
