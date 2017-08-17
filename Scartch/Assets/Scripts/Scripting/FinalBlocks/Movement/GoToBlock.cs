using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using View;

namespace Scripting
{
    public class GoToBlock : Block
    {
        public GoToBlock(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, BlockViewer viewer, bool sample) : base(owner, optionList, ScriptingType.movement, referenceSlotViewers, viewer, sample)
        {
        }

        public static string description = "Go to position x (  ) y (  ) z (  )";

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override void Execute()
        {
            owner.Position = new Vector3(ReferenceList[0].FloatEval, ReferenceList[1].FloatEval, ReferenceList[2].FloatEval);
            Flux.current.CurrentBlock = Next;
        }
    }
}
