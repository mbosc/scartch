using System;
using UnityEngine;
using model;
using System.Linq;

namespace view
{
	public class HelloBlockWrapper : BlockWrapper
	{
		public override void Init(ActorWrapper wrapper, bool autoinit = true){
			GetComponent<Renderer> ().material = ResourceManager.Instance.bloccoSuono;
			testo = "Imposta il volume al (  )%";
			block = new HelloBlock (wrapper.actor);
			base.Init (wrapper,autoinit);
		}

		private class HelloBlock : Block
		{
			public HelloBlock(Actor owner){
				this.owner = owner;
			}
			public override Block ExecuteAndGetNext()
			{
				//FILL HERE
				return Next;
			}
		}
	}
}
