using System;
using UnityEngine;
using model;
using System.Linq;

namespace view
{
	public class HideActorBlockWrapper : BlockWrapper
	{
		public override void Init(ActorWrapper wrapper, bool autoinit = true){
			GetComponent<Renderer> ().material = ResourceManager.Instance.bloccoAspetto;
			testo = "Nasconditi";
			block = new HideActorBlock (wrapper.actor);
			base.Init (wrapper,autoinit);
		}

		private class HideActorBlock : Block
		{
			public HideActorBlock(Actor owner){
				this.owner = owner;
			}
			public override Block ExecuteAndGetNext()
			{
				owner.Hidden = true;
				return Next;
			}
		}
	}
}
