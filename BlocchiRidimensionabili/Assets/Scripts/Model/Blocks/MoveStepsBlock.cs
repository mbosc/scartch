using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace model
{
    namespace blocks
    {
        public class MoveStepsBlock : Block
        {
            private Actor owner
                ;
            public MoveStepsBlock(Actor owner)
            {
                this.owner = owner;
            }
            public override Block ExecuteAndGetNext()
            {
                //da dirigere nella direzione del vettore di direzione.
                //owner.Position += new Vector3(0,0,(references[0] as NumberReference).Evaluate());
                owner.Position += new Vector3(0, 0, 10);
                return Next;
            }
        }
    }   }