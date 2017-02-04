using System;
using UnityEngine;
using model;
using System.Linq;

namespace view
{
	public class GoToPositionBlockWrapper : BlockWrapper
	{
		public override void Init(ActorWrapper wrapper, bool autoinit = true){
			GetComponent<Renderer> ().material = ResourceManager.Instance.bloccoMovimento;
			testo = "Vai alla posizione x: (  ) y: (  ) z: (  )";
			block = new GoToPositionBlock (wrapper.actor);
			base.Init (wrapper, autoinit);
		}

		private class GoToPositionBlock : Block
		{
			
			public GoToPositionBlock(Actor owner){
				this.owner = owner;
			}
			public override Block ExecuteAndGetNext()
			{
				owner.Position = new Vector3 (
					GetReferenceAs<float>(0),
					GetReferenceAs<float>(1),
					GetReferenceAs<float>(2)
				);
				return Next;
			}
		}
	}
}

