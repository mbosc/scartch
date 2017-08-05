using System;
using UnityEngine;
using model;
using System.Linq;

namespace view
{
	public class ShowActorBlockWrapper : BlockWrapper
	{
		public override void Init(ActorWrapper wrapper, bool autoinit = true){
			GetComponent<Renderer> ().material = ResourceManager.Instance.bloccoAspetto;
			testo = "Mostrati";
			block = new ShowActorBlock (wrapper.Actor);
			base.Init (wrapper,autoinit);
		}

		private class ShowActorBlock : Block
		{
			public ShowActorBlock(Actor owner){
				this.owner = owner;
			}
			public override Block ExecuteAndGetNext()
			{
				owner.Hidden = false;
				return Next;
			}
		}
	}
}
