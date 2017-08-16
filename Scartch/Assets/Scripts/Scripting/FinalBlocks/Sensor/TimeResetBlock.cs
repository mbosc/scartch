using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using View;

namespace Scripting
{
    public class TimeResetBlock : Block
    {
        public TimeResetBlock(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, BlockViewer viewer, bool sample) : base(owner, optionList, ScriptingType.sensor, referenceSlotViewers, viewer, sample)
        {
        }

        public static string description = "Reset Timer";

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override void Execute()
        {
            Timer.instance.Reset();
            Flux.current.CurrentBlock = Next;
        }
    }
}
