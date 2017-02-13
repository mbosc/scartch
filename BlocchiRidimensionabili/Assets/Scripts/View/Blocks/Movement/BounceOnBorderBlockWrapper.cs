
using UnityEngine;
using model;
using System.Linq;

namespace view
{
	public class BounceOnBorderBlockWrapper : BlockWrapper
	{
		public override void Init(ActorWrapper wrapper, bool autoinit = true){
			GetComponent<Renderer> ().material = ResourceManager.Instance.bloccoMovimento;
			testo = "Rimbalza quando tocchi il bordo";
			block = new BounceOnBorderBlock (wrapper.Actor);
			base.Init (wrapper, autoinit);
		}

		private class BounceOnBorderBlock : Block
		{
			
			public BounceOnBorderBlock(Actor owner){
				this.owner = owner;
			}
			public override Block ExecuteAndGetNext()
			{
				Vector3 rotation = new Vector3();
				if (Environment.MaxX - Mathf.Abs(owner.Position.x) < Environment.BorderTolerance)
					rotation.y += 180;
				if (Environment.MaxZ - Mathf.Abs (owner.Position.z) < Environment.BorderTolerance)
					rotation.y += rotation.y != 0 ? 0 : 180;
				if (Environment.MaxY - Mathf.Abs (owner.Position.y) < Environment.BorderTolerance)
					rotation.z += 180;
				//da controllare che questo algoritmo funzioni davvero
				owner.Rotation += rotation;
				return Next;
			}
		}
	}
}

