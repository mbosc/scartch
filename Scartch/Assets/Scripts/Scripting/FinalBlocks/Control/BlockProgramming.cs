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
                Flux nextFlux = new Flux(this)
                {
                    CurrentBlock = InnerNext
                };
                Action disposableCallback = null;
                var cur = Flux.current;
                disposableCallback = () =>
                {
                    nextFlux.Callbacks -= disposableCallback;
                    ScheduleNextFlux(cur, Next);
                };
                nextFlux.Callbacks += disposableCallback;
                ExecutionController.Instance.AddFlux(nextFlux);
                Flux.current.CurrentBlock = new PureWaitBlock();
                Debug.Log("  Flux initiated by " + Flux.current.initiator + " rescheduled with " + Flux.current.CurrentBlock);
            }
            else
            {
                Flux.current.CurrentBlock = Next;
            }
        }

        private void ScheduleNextFlux(Flux targetFlux, Block targetNext)
        {
            Debug.Log("     Flux initiated by " + targetFlux.initiator + " rescheduled starting with " + Next);
            targetFlux.CurrentBlock = targetNext;
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
                Flux nextFlux = new Flux(this)
                {
                    CurrentBlock = InnerNext
                };
                Action disposableCallback = null;
                var cur = Flux.current;
                disposableCallback = () => { nextFlux.Callbacks -= disposableCallback; ScheduleNextFlux(cur, Next); };
                nextFlux.Callbacks += disposableCallback;
                ExecutionController.Instance.AddFlux(nextFlux);
                Flux.current.CurrentBlock = new PureWaitBlock();
            }
            else
            {
                Flux.current.CurrentBlock = Next;
            }
        }

        private void ScheduleNextFlux(Flux targetFlux, Block targetNext)
        {
            if (ReferenceList[0].BoolEval)
            {
                Flux nextFlux = new Flux(this)
                {
                    CurrentBlock = InnerNext
                };
                Action disposableCallback = null;
                disposableCallback = () => { nextFlux.Callbacks -= disposableCallback; ScheduleNextFlux(targetFlux, targetNext); };
                nextFlux.Callbacks += disposableCallback;
                ExecutionController.Instance.AddFlux(nextFlux);
            }
            else
            {
                targetFlux.CurrentBlock = targetNext;
            }
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

            Flux nextFlux = new Flux(this)
            {
                CurrentBlock = InnerNext
            };
            Action disposableCallback = null;
            disposableCallback = () => { nextFlux.Callbacks -= disposableCallback; ScheduleNextFlux(); };
            nextFlux.Callbacks += disposableCallback;
            ExecutionController.Instance.AddFlux(nextFlux);
            Flux.current.CurrentBlock = null;
            Debug.Log("  Flux initiated by " + Flux.current.initiator + " rescheduled with " + Flux.current.CurrentBlock);
        }


        //OK, A POSTO!
        private void ScheduleNextFlux()
        {
            Debug.Log("     New flux scheduled starting with " + InnerNext);
            Flux nextFlux = new Flux(this)
            {
                CurrentBlock = InnerNext
            };
            Action disposableCallback = null;
            disposableCallback = () => { nextFlux.Callbacks -= disposableCallback; ScheduleNextFlux(); };
            nextFlux.Callbacks += disposableCallback;
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
            if (count-- > 0)
            {
                Flux nextFlux = new Flux(this)
                {
                    CurrentBlock = InnerNext
                };
                Action disposableCallback = null;
                var cur = Flux.current;
                disposableCallback = () => { nextFlux.Callbacks -= disposableCallback; ScheduleNextFlux(cur, Next); };
                nextFlux.Callbacks += disposableCallback;
                ExecutionController.Instance.AddFlux(nextFlux);
                Flux.current.CurrentBlock = new PureWaitBlock();
            }
            else
            {
                Flux.current.CurrentBlock = Next;
            }
        }

        private void ScheduleNextFlux(Flux targetFlux, Block targetNext)
        {
            if (count-- > 0)
            {
                Flux nextFlux = new Flux(this)
                {
                    CurrentBlock = InnerNext
                };
                Action disposableCallback = null;
                disposableCallback = () => { nextFlux.Callbacks -= disposableCallback; ScheduleNextFlux(targetFlux, targetNext); };
                nextFlux.Callbacks += disposableCallback;
                ExecutionController.Instance.AddFlux(nextFlux);
            }
            else
            {
                targetFlux.CurrentBlock = targetNext;
            }
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
                Flux nextFlux = new Flux(this)
                {
                    CurrentBlock = UpperInnerNext
                };
                Action disposableCallback = null;
                var cur = Flux.current;
                disposableCallback = () => 
                {
                    nextFlux.Callbacks -= disposableCallback;
                    ScheduleNextFlux(cur, Next);
                };
                nextFlux.Callbacks += disposableCallback;
                ExecutionController.Instance.AddFlux(nextFlux);
                Flux.current.CurrentBlock = new PureWaitBlock();
                Debug.Log("  Flux initiated by " + Flux.current.initiator + " rescheduled with " + Flux.current.CurrentBlock);
            }
            else
            {
                Flux nextFlux = new Flux(this)
                {
                    CurrentBlock = LowerInnerNext
                };
                Action disposableCallback = null;
                var cur = Flux.current;
                disposableCallback = () => 
                {
                    nextFlux.Callbacks -= disposableCallback;
                    ScheduleNextFlux(cur, Next);
                };
                nextFlux.Callbacks += disposableCallback;
                ExecutionController.Instance.AddFlux(nextFlux);
                Flux.current.CurrentBlock = new PureWaitBlock();
                Debug.Log("  Flux initiated by " + Flux.current.initiator + " rescheduled with " + Flux.current.CurrentBlock);
            }
        }

        private void ScheduleNextFlux(Flux targetFlux, Block targetNext)
        {
            Debug.Log("     Flux initiated by " + targetFlux.initiator + " rescheduled starting with " + Next);
            targetFlux.CurrentBlock = targetNext;
        }
    }

    public class PureWaitBlock : MouthBlock
    {
        public PureWaitBlock() : base(null, null, ScriptingType.control, null, null, true)
        {
        }

        public static string description = "NA";

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override void Execute()
        {

            Flux.current.CurrentBlock = this;

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
                Flux nextFlux = new Flux(this)
                {
                    CurrentBlock = InnerNext
                };
                Action disposableCallback = null;
                var cur = Flux.current;
                disposableCallback = () => { nextFlux.Callbacks -= disposableCallback; ScheduleNextFlux(cur, Next); };
                nextFlux.Callbacks += disposableCallback;
                ExecutionController.Instance.AddFlux(nextFlux);
                Flux.current.CurrentBlock = new PureWaitBlock();
            }
            else
            {
                Flux.current.CurrentBlock = Next;
            }
        }

        private void ScheduleNextFlux(Flux targetFlux, Block targetNext)
        {
            if (!ReferenceList[0].BoolEval)
            {
                Flux nextFlux = new Flux(this)
                {
                    CurrentBlock = InnerNext
                };
                Action disposableCallback = null;
                disposableCallback = () => { nextFlux.Callbacks -= disposableCallback; ScheduleNextFlux(targetFlux, targetNext); };
                nextFlux.Callbacks += disposableCallback;
                ExecutionController.Instance.AddFlux(nextFlux);
            }
            else
            {
                targetFlux.CurrentBlock = targetNext;
            }
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
