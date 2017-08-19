using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using View;

namespace Scripting
{
    public class SayForSecondsBlock : Block
    {
        public SayForSecondsBlock(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, BlockViewer viewer, bool sample) : base(owner, optionList, ScriptingType.look, referenceSlotViewers, viewer, sample)
        {
        }

        public static string description = "Say [  ] for (  ) seconds";

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override void Execute()
        {
            owner.Message = ReferenceList[0].StringEval;
            owner.IsMessageVisible = true;
            ExecutionController.Instance.auxTimer.ExecuteDelayed(ReferenceList[1].FloatEval, () => { owner.IsMessageVisible = false; });
            Flux.current.CurrentBlock = Next;
        }
    }

    public class HideMessageBlock : Block
    {
        public HideMessageBlock(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, BlockViewer viewer, bool sample) : base(owner, optionList, ScriptingType.look, referenceSlotViewers, viewer, sample)
        {
        }

        public static string description = "Hide message";

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override void Execute()
        {
            owner.IsMessageVisible = false;
            Flux.current.CurrentBlock = Next;
        }
    }

    public class SetScaleToBlock : Block
    {
        public SetScaleToBlock(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, BlockViewer viewer, bool sample) : base(owner, optionList, ScriptingType.look, referenceSlotViewers, viewer, sample)
        {
        }

        public static string description = "Set scale to (  )";

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override void Execute()
        {
            owner.Scale = ReferenceList[0].FloatEval;
            Flux.current.CurrentBlock = Next;
        }
    }

    public class SetScaleToPercentageBlock : Block
    {
        public SetScaleToPercentageBlock(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, BlockViewer viewer, bool sample) : base(owner, optionList, ScriptingType.look, referenceSlotViewers, viewer, sample)
        {
        }

        public static string description = "Set scale to (  )%";

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override void Execute()
        {
            owner.Scale *= ReferenceList[0].FloatEval / 100;
            Flux.current.CurrentBlock = Next;
        }
    }

    public class ScaleReference : ExpressionReference
    {

        public static string description = "Scale";

        public ScaleReference(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer, bool sample) : base(owner, optionList, ScriptingType.look, referenceSlotViewers, viewer, RefType.numberType, sample)
        {
        }

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override string Evaluate()
        {
            float val = owner.Scale;
            return val.ToStdStr();
        }
    }

    public class ModelChangeBlock : Block
    {
        public ModelChangeBlock(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, BlockViewer viewer, bool sample) : base(owner, optionList, ScriptingType.look, referenceSlotViewers, viewer, sample)
        {
        }

        public static string description = "Set actor model to {model 1|model 2|model 3}";

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override void Execute()
        {
            var model = ActorModel.GetActorModel(GetOption(0).Value);
            owner.Model = model;
            Flux.current.CurrentBlock = Next;
        }
    }
}
