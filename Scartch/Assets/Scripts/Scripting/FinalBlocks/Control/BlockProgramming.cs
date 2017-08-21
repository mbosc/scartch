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

    public class WhileBlock : MouthBlock
    {
        public WhileBlock(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, MouthBlockViewer viewer, bool sample) : base(owner, optionList, ScriptingType.control, referenceSlotViewers, viewer, sample)
        {
        }

        public static string description = "While <  >";

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
            Flux nextFlux = new Flux(this);
            if (ReferenceList[0].BoolEval)
            {
                nextFlux.CurrentBlock = InnerNext;
                nextFlux.Callbacks += ScheduleNextFlux;
            }
            else
            {
                nextFlux.CurrentBlock = Next;
            }
            ExecutionController.Instance.AddFlux(nextFlux);
        }
    }

    public class ForeverBlock : MouthBlock
    {
        public ForeverBlock(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, MouthBlockViewer viewer, bool sample) : base(owner, optionList, ScriptingType.control, referenceSlotViewers, viewer, sample)
        {
        }

        public static string description = "Forever";

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override void Execute()
        {
            Flux.current.CurrentBlock = InnerNext;
            Flux.current.Callbacks += ScheduleNextFlux;
            
        }

        private void ScheduleNextFlux()
        {
            Flux.current.Callbacks -= ScheduleNextFlux;

            Flux nextFlux = new Flux(this);

            nextFlux.CurrentBlock = InnerNext;
            nextFlux.Callbacks += ScheduleNextFlux;

            ExecutionController.Instance.AddFlux(nextFlux);
        }
    }

    public class RepeatBlock : MouthBlock
    {
        public RepeatBlock(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, MouthBlockViewer viewer, bool sample) : base(owner, optionList, ScriptingType.control, referenceSlotViewers, viewer, sample)
        {
        }

        public static string description = "Repeat (  ) times";

        public override string Description
        {
            get
            {
                return description;
            }
        }

        private int count = 0;
        public override void Execute()
        {
            count = Mathf.RoundToInt(ReferenceList[0].FloatEval);
            if (count-- <= 0)
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
            Flux nextFlux = new Flux(this);
            if (count-- <= 0)
            {
                nextFlux.CurrentBlock = InnerNext;
                nextFlux.Callbacks += ScheduleNextFlux;
            }
            else
            {
                nextFlux.CurrentBlock = Next;
            }
            ExecutionController.Instance.AddFlux(nextFlux);
        }
    }

    public class IfElseBlock : DoubleMouthBlock
    {
        public IfElseBlock(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, DoubleMouthBlockViewer viewer, bool sample) : base(owner, optionList, ScriptingType.control, referenceSlotViewers, viewer, sample)
        {
        }

        public static string description = "If <  >";
        
        public override string Description
        {
            get
            {
                return description + " " + secondDescription;
            }
        }

        public static string secondDescription = "else";

        public override void Execute()
        {
            if (ReferenceList[0].BoolEval)
            {
                Flux.current.CurrentBlock = UpperInnerNext;
                Flux.current.Callbacks += ScheduleNextFlux;
            }
            else
            {
                Flux.current.CurrentBlock = LowerInnerNext;
                Flux.current.Callbacks += ScheduleNextFlux;
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

    public class UntilBlock : MouthBlock
    {
        public UntilBlock(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, MouthBlockViewer viewer, bool sample) : base(owner, optionList, ScriptingType.control, referenceSlotViewers, viewer, sample)
        {
        }

        public static string description = "Until <  >";

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override void Execute()
        {
            if (!ReferenceList[0].BoolEval)
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
            Flux nextFlux = new Flux(this);
            if (!ReferenceList[0].BoolEval)
            {
                nextFlux.CurrentBlock = InnerNext;
                nextFlux.Callbacks += ScheduleNextFlux;
            }
            else
            {
                nextFlux.CurrentBlock = Next;
            }
            ExecutionController.Instance.AddFlux(nextFlux);
        }
    }

    public class WaitUntilBlock : Block
    {
        public WaitUntilBlock(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, BlockViewer viewer, bool sample) : base(owner, optionList, ScriptingType.control, referenceSlotViewers, viewer, sample)
        {
        }

        public static string description = "Wait until <  >";

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override void Execute()
        {
            if (!ReferenceList[0].BoolEval)
            {
                Flux.current.CurrentBlock = this;
            }
            else
            {
                Flux.current.CurrentBlock = Next;
            }
        }
    }
}
