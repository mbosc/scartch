using System;
using UnityEngine;
using model;
using System.Linq;

namespace view
{
	public class StopScriptBlockWrapper : BlockWrapper
	{
		public override void Init(ActorWrapper wrapper, bool autoinit = true){
			GetComponent<Renderer> ().material = ResourceManager.Instance.bloccoControllo;
			testo = "Ferma script";
			block = new StopScriptBlock (wrapper.Actor);
			base.Init (wrapper,autoinit);
		}

		private class StopScriptBlock : Block
		{
			public StopScriptBlock(Actor owner){
				this.owner = owner;
			}
			public override Block ExecuteAndGetNext()
			{
				return null;
			}
		}
	}
}
