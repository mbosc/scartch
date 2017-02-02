using System;
using UnityEngine;
using model;
using System.Linq;

namespace view
{
	public class IncreasePositionBlockWrapper : BlockWrapper
	{
		protected override void Start(){
			testo = "Cambia {  } di (  )";
			base.Start ();
		}

		public override ActorWrapper Owner {
			get {
				return base.Owner;
			}
			set {
				base.Owner = value;
				block = new IncreasePositionBlock (Owner.actor);
			}
		}

		private class IncreasePositionBlock : Block
		{
			public Actor owner;
			public IncreasePositionBlock(Actor owner){
				var opt = new Option(new object[]{"x", "y", "z"}.ToList());
				options.Add(0, opt);
				this.owner = owner;
			}
			public override Block ExecuteAndGetNext()
			{
				var quantity = GetReferenceAs<float>(0);
				var axis = (options [0].chosenValue);
				switch (axis) {
				case 0:
					owner.Position += new Vector3(quantity, 0 ,0);
					break;
				case 1:
					owner.Position += new Vector3(0, quantity, 0);
					break;
				case 2:
					owner.Position += new Vector3(0, 0, quantity);
					break;
				default:
					throw new ArgumentException ("Invalid option value");
				}
					
				return Next;
			}
		}
	}
}

