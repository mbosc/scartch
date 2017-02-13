using System;
using UnityEngine;
using model;
using System.Linq;

namespace view
{
	public class SetSizeBlockWrapper : BlockWrapper
	{
		public override void Init(ActorWrapper wrapper, bool autoinit = true){
			GetComponent<Renderer> ().material = ResourceManager.Instance.bloccoAspetto;
			testo = "Imposta dimensioni a (  )";
			block = new SetSizeBlock (wrapper.Actor);
			base.Init (wrapper,autoinit);
		}

		private class SetSizeBlock : Block
		{
			public SetSizeBlock(Actor owner){
				this.owner = owner;
			}
			public override Block ExecuteAndGetNext()
			{
				owner.Scale = GetReferenceAs<float>(0);
				return Next;
			}
		}
	}
}
