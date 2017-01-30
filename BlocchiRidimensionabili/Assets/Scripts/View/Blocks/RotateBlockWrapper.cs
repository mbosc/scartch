using System;
using UnityEngine;
using model;
using System.Linq;

namespace view
{
	public class RotateBlockWrapper : BlockWrapper
	{
		protected override void Start(){
			testo = "Ruota di (  ) gradi intorno all'asse {    }";
			base.Start ();
		}

		public override ActorWrapper Owner {
			get {
				return base.Owner;
			}
			set {
				base.Owner = value;
				block = new RotateBlock (Owner.actor);
			}
		}

		private class RotateBlock : Block
		{
			public Actor owner;
			public RotateBlock(Actor owner){
				var opt = new Option(new object[]{"x", "y", "z"}.ToList());
				options.Add(0, opt);
				this.owner = owner;
			}
			public override Block ExecuteAndGetNext()
			{
				var quantity = (references [0] as NumberReference).Evaluate ();
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

