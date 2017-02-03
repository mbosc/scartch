﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;

namespace view{
	public class MoveStepsBlockWrapper : BlockWrapper {

		public override void Init(ActorWrapper wrapper, bool autoinit = true){
			testo = "Fai (  ) passi";
			block = new MoveStepsBlock (wrapper.actor);
			base.Init (wrapper, autoinit);
		}

        private class MoveStepsBlock : Block
        {
            
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