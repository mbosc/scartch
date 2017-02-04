using System;
using UnityEngine;
using model;
using System.Linq;

namespace view
{
	public class TimeResetBlockWrapper : BlockWrapper
	{
		public override void Init(ActorWrapper wrapper, bool autoinit = true){
			GetComponent<Renderer> ().material = ResourceManager.Instance.bloccoSensori;
			testo = "Resetta il timer";
			block = new TimeResetBlock (wrapper.actor);
			base.Init (wrapper,autoinit);
		}

		private class TimeResetBlock : Block
		{
			public TimeResetBlock(Actor owner){
				this.owner = owner;
			}
			public override Block ExecuteAndGetNext()
			{
				model.Environment.Instance.Timer.StartTimer ();
				return Next;
			}
		}
	}
}
