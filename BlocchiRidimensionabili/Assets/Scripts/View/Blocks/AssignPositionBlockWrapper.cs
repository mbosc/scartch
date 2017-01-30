﻿using System;
using UnityEngine;
using model;
using System.Linq;

namespace view
{
	public class AssignPositionBlockWrapper : BlockWrapper
	{
		protected override void Start(){
			testo = "Assegna (  ) a {  }";
			base.Start ();
		}

		public override ActorWrapper Owner {
			get {
				return base.Owner;
			}
			set {
				base.Owner = value;
				block = new AssignPositionBlock (Owner.actor);
			}
		}

		private class AssignPositionBlock : Block
		{
			public Actor owner;
			public AssignPositionBlock(Actor owner){
				var opt = new Option(new object[]{"x", "y", "z"}.ToList());
				options.Add(0, opt);
				this.owner = owner;
			}
			public override Block ExecuteAndGetNext()
			{
				var quantity = (references [0] as NumberReference).Evaluate ();
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

