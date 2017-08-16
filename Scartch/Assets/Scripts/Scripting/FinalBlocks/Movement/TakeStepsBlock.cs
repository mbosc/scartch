using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using View;

namespace Scripting
{
    public class TakeStepsBlock : Block
    {
        public TakeStepsBlock(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, BlockViewer viewer, bool sample) : base(owner, optionList, ScriptingType.movement, referenceSlotViewers, viewer, sample)
        {
        }

        public static string description = "Take (  ) steps";

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override void Execute()
        {
            var forward = (Quaternion.Euler(owner.Rotation) * new Vector3(0, 0, 1));
            owner.Position += forward * referenceList[0].FloatEval;
            Flux.current.CurrentBlock = Next;
        }
    }
}
