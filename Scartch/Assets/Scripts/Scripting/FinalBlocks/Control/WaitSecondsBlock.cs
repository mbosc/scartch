using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using View;

namespace Scripting
{
    public class WaitSecondsBlock : Block
    {
        public WaitSecondsBlock(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, BlockViewer viewer, bool sample) : base(owner, optionList, ScriptingType.control, referenceSlotViewers, viewer, sample)
        {
        }

        public static string description = "Wait for (  ) seconds";

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override void Execute()
        {
            Flux.current.CurrentBlock = Next;
            ExecutionController.Instance.PauseFor(ReferenceList[0].FloatEval);
        }
    }
}
