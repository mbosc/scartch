using System;
using UnityEngine;
using model;
using System.Linq;

namespace view
{
	public class AssignPositionBlockWrapper : BlockWrapper
	{
		public override void Init(ActorWrapper wrapper, bool autoinit = true){
			GetComponent<Renderer> ().material = ResourceManager.Instance.bloccoMovimento;
			testo = "Assegna (  ) a {  }";
			block = new AssignPositionBlock (wrapper.Actor);
			base.Init (wrapper,autoinit);
		}

		private class AssignPositionBlock : Block
		{
			public AssignPositionBlock(Actor owner){
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
					owner.Position = new Vector3(quantity, owner.Position.y, owner.Position.z);
					break;
				case 1:
					owner.Position = new Vector3(owner.Position.x, quantity, owner.Position.z);
					break;
				case 2:
					owner.Position = new Vector3(owner.Position.x, owner.Position.y, quantity);
					break;
				default:
					throw new ArgumentException ("Invalid option value");
				}
					
				return Next;
			}
		}
	}
}

