using System;
using UnityEngine;
using model;
using System.Linq;

namespace view
{
	public class SetVolumeBlockWrapper : BlockWrapper
	{
		public override void Init(ActorWrapper wrapper, bool autoinit = true){
			GetComponent<Renderer> ().material = ResourceManager.Instance.bloccoSuono;
			testo = "Imposta il volume al (  )%";
			block = new SetVolumeBlock (wrapper.actor);
			base.Init (wrapper,autoinit);
		}

		private class SetVolumeBlock : Block
		{
			public SetVolumeBlock(Actor owner){
				this.owner = owner;
			}
			public override Block ExecuteAndGetNext()
			{
				owner.Volume = GetReferenceAs<float> (0) / 100;
				return Next;
			}
		}
	}
}
