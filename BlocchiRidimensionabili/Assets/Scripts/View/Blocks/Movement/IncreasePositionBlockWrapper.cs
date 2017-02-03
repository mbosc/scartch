using System;
using UnityEngine;
using model;
using System.Linq;

namespace view
{
	public class IncreasePositionBlockWrapper : BlockWrapper
	{
		public override void Init(ActorWrapper wrapper, bool autoinit = true){
			testo = "Cambia {  } di (  )";
			block = new IncreasePositionBlock (wrapper.actor);
			base.Init (wrapper, autoinit);
		}

		private class IncreasePositionBlock : Block
		{
			
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

