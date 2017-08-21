using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using View;

namespace Scripting
{
    public class PauseExecutionBlock : Block
    {
        public PauseExecutionBlock(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, BlockViewer viewer, bool sample) : base(owner, optionList, ScriptingType.control, referenceSlotViewers, viewer, sample)
        {
        }

        public static string description = "Pause execution for (  ) seconds";

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
            Flux.current.CurrentBlock = new WaitTillBlock(Next, Timer.instance.Time + ReferenceList[0].FloatEval);
        }

        private class WaitTillBlock : Block
        {
            public Block nextBlock;
            public float timeval;

            public WaitTillBlock(Block nextblock, float timeval) : base(null, null, ScriptingType.control, null, null, true)
            {
                this.nextBlock = nextblock;
                this.timeval = timeval;
            }

            public static string description = "Wait until - internal";

            public override string Description
            {
                get
                {
                    return description;
                }
            }

            public override void Execute()
            {
                if (Timer.instance.Time < timeval)
                    Flux.current.CurrentBlock = this;
                else
                    Flux.current.CurrentBlock = nextBlock;

            }
        }
    }
}
