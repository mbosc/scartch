using System;
using UnityEngine;
using model;
using System.Linq;

namespace view
{
	public class SayBlockWrapper : BlockWrapper
	{
		public override void Init(ActorWrapper wrapper, bool autoinit = true){
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
                owner.Message = (references[0] as StringReference).Evaluate();
                owner.ShowMessage();
				return Next;
			}
		}
	}
}

