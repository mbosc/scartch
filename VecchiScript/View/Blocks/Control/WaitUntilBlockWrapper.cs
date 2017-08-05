using System;
using UnityEngine;
using model;
using System.Linq;

namespace view
{
	public class WaitUntilBlockWrapper : BlockWrapper
	{
		public override void Init(ActorWrapper wrapper, bool autoinit = true){
			GetComponent<Renderer> ().material = ResourceManager.Instance.bloccoControllo;
			testo = "Aspetta finché <  >";
			block = new WaitUntilBlock (wrapper.Actor);
			base.Init (wrapper,autoinit);
		}

		private class WaitUntilBlock : Block
		{
			public WaitUntilBlock(Actor owner){
				this.owner = owner;
			}
			public override Block ExecuteAndGetNext()
			{
				if (GetReferenceAs<bool> (0))
					return this;
				else
					return Next;
			}
		}
	}
}
