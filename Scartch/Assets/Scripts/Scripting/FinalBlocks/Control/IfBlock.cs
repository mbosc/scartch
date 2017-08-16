using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using View;

namespace Scripting
{
    public class IfBlock : MouthBlock
    {
        public IfBlock(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, MouthBlockViewer viewer, bool sample) : base(owner, optionList, ScriptingType.control, referenceSlotViewers, viewer, sample)
        {
        }

        public static string description = "If <  >";

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override void Execute()
        {
            if (ReferenceList[0].BoolEval)
            {
                Flux.current.CurrentBlock = InnerNext;
                Flux.current.Callbacks += ScheduleNextFlux;
            }
            else
            {
                Flux.current.CurrentBlock = Next;
            }
        }

        private void ScheduleNextFlux()
        {
            Flux.current.Callbacks -= ScheduleNextFlux;
            Flux nextFlux = new Flux(this)
            {
                CurrentBlock = Next
            };
            ExecutionController.Instance.AddFlux(nextFlux);
        }
    }
}
