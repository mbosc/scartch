using System;
using UnityEngine;
using model;
using System.Linq;

namespace view
{
	public class GoToPositionBlockWrapper : BlockWrapper
	{
		protected override void Start(){
			testo = "Vai alla posizione x: (  ) y: (  ) z: (  )";
			base.Start ();
		}

		public override ActorWrapper Owner {
			get {
				return base.Owner;
			}
			set {
				base.Owner = value;
				block = new GoToPositionBlock (Owner.actor);
			}
		}

		private class GoToPositionBlock : Block
		{
			public Actor owner;
			public GoToPositionBlock(Actor owner){
				this.owner = owner;
			}
			public override Block ExecuteAndGetNext()
			{
				owner.Position = new Vector3 (
					(references [0] as NumberReference).Evaluate (),
					(references [1] as NumberReference).Evaluate (),
					(references [2] as NumberReference).Evaluate ()
				);
				return Next;
			}
		}
	}
}

