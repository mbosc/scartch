using System;
using UnityEngine;
using model;
using System.Linq;

namespace view
{
	public class HideMessageBlockWrapper : BlockWrapper
	{
		public override void Init(ActorWrapper wrapper, bool autoinit = true){
			GetComponent<Renderer> ().material = ResourceManager.Instance.bloccoAspetto;
			testo = "Rimuovi Fumetto";
			block = new HideMessageBlock (wrapper.Actor);
			base.Init (wrapper,autoinit);
		}

		private class HideMessageBlock : Block
		{
			public HideMessageBlock(Actor owner){
				this.owner = owner;
			}
			public override Block ExecuteAndGetNext()
			{
				owner.HideMessage ();
				return Next;
			}
		}
	}
}
