using System;
using UnityEngine;
using model;
using System.Linq;

namespace view
{
	public class ChangeVolumeByBlockWrapper : BlockWrapper
	{
		public override void Init(ActorWrapper wrapper, bool autoinit = true){
			GetComponent<Renderer> ().material = ResourceManager.Instance.bloccoSuono;
			testo = "Cambia volume di (  )";
			block = new ChangeVolumeByBlock (wrapper.actor);
			base.Init (wrapper,autoinit);
		}

		private class ChangeVolumeByBlock : Block
		{
			public ChangeVolumeByBlock(Actor owner){
				this.owner = owner;
			}
			public override Block ExecuteAndGetNext()
			{
				owner.Volume += GetReferenceAs<float>(0);
				return Next;
			}
		}
	}
}
