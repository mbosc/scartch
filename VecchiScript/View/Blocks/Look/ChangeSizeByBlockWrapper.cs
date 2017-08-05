using System;
using UnityEngine;
using model;
using System.Linq;

namespace view
{
	public class ChangeSizeByBlockWrapper : BlockWrapper
	{
		public override void Init(ActorWrapper wrapper, bool autoinit = true){
			GetComponent<Renderer> ().material = ResourceManager.Instance.bloccoAspetto;
			testo = "Imposta dimensioni al (  )%";
			block = new ChangeSizeByBlock (wrapper.Actor);
			base.Init (wrapper,autoinit);
		}

		private class ChangeSizeByBlock : Block
		{
			public ChangeSizeByBlock(Actor owner){
				this.owner = owner;
			}
			public override Block ExecuteAndGetNext()
			{
				owner.Scale *= GetReferenceAs<float> (0) / 100.0f;
				return Next;
			}
		}
	}
}
