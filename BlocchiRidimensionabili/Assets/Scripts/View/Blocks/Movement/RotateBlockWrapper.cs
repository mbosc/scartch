using System;
using UnityEngine;
using model;
using System.Linq;

namespace view
{
	public class RotateBlockWrapper : BlockWrapper
	{
		public override void Init(ActorWrapper wrapper, bool autoinit = true){
			testo = "Ruota di (  ) ° intorno all'asse {  }";
			block = new RotateBlock (wrapper.actor);
			base.Init (wrapper, autoinit);
		}

		private class RotateBlock : Block
		{
			
			public RotateBlock(Actor owner){
				var opt = new Option(new object[]{"x", "y", "z"}.ToList());
				options.Add(0, opt);
				this.owner = owner;
			}
			public override Block ExecuteAndGetNext()
			{
				var quantity = GetReferenceAs<float>(0);
				var axis = (options [0].chosenValue);
				var addedRotation = new Vector3 ();
				switch (axis) {
				case 0:
					addedRotation.x += quantity;
					break;
				case 1:
					addedRotation.y += quantity;
					break;
				case 2:
					addedRotation.z += quantity;
					break;
				default:
					throw new ArgumentException ("Invalid option value");
				}
					owner.Rotation += addedRotation;
				return Next;
			}
		}
	}
}

