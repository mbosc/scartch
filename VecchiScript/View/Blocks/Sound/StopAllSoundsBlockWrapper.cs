using System;
using UnityEngine;
using model;
using System.Linq;

namespace view
{
	public class StopAllSoundsBlockWrapper : BlockWrapper
	{
		public override void Init(ActorWrapper wrapper, bool autoinit = true){
			GetComponent<Renderer> ().material = ResourceManager.Instance.bloccoSuono;
			testo = "Arresta tutti i suoni";
			block = new StopAllSoundsBlock (wrapper.Actor);
			base.Init (wrapper,autoinit);
		}

		private class StopAllSoundsBlock : Block
		{
			public StopAllSoundsBlock(Actor owner){
				this.owner = owner;
			}
			public override Block ExecuteAndGetNext()
			{
				model.Environment.Instance.Actors.ForEach(s => s.StopSound());
				return Next;
			}
		}
	}
}
