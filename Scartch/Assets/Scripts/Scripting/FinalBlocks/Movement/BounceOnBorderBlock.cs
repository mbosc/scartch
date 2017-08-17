using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using View;

namespace Scripting
{
    public class BounceOnBorderBlock : Block
    {
        public BounceOnBorderBlock(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, BlockViewer viewer, bool sample) : base(owner, optionList, ScriptingType.movement, referenceSlotViewers, viewer, sample)
        {
        }

        public static string description = "Bounce when you hit the border";

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override void Execute()
        {
            if (Mathf.Abs(owner.Position.x) > Actor.MaxX - Actor.BorderTolerance ||
                Mathf.Abs(owner.Position.z) > Actor.MaxZ - Actor.BorderTolerance)
                owner.Rotation += new Vector3(0, 180, 0);
            Flux.current.CurrentBlock = Next;
        }
    }
}
