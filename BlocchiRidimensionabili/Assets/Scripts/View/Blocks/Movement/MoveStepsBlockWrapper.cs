using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;

namespace view{
	public class MoveStepsBlockWrapper : BlockWrapper {

		protected override void Start ()
		{
			testo = "Fai (  ) passi";
			base.Start ();
		}
		public override ActorWrapper Owner {
			get {
				return base.Owner;
			}
			set {
				base.Owner = value;
				block = new MoveStepsBlock (Owner.actor);
			}
		}

        private class MoveStepsBlock : Block
        {
            public Actor owner;
            public MoveStepsBlock(Actor owner)
            {
                this.owner = owner;
            }
            public override Block ExecuteAndGetNext()
            {
				var forward = (Quaternion.Euler(owner.Rotation) * new Vector3(0,0,1));
				owner.Position += forward * (references[0] as NumberReference).Evaluate();
                return Next;
            }
        }
    }
}