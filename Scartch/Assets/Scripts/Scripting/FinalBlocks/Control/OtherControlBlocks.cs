using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using View;

namespace Scripting
{
    public class BroadcastBlock : Block
    {
        public BroadcastBlock(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, BlockViewer viewer, bool sample) : base(owner, optionList, ScriptingType.control, referenceSlotViewers, viewer, sample)
        {
        }

        public static string description = "Broadcast [  ]";

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override void Execute()
        {
            ExecutionController.Instance.BroadcastMessage(ReferenceList[0].StringEval);
            Flux.current.CurrentBlock = Next;
        }
    }

    public class ReceivedMessageHat : Hat
    {

        public static string description = "When [  ] is received";
        private bool sample = false;

        public ReceivedMessageHat(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, HatViewer viewer, bool sample) : base(owner, optionList, ScriptingType.control, referenceSlotViewers, viewer, sample)
        {
            this.sample = sample;
            if (!sample)
                Controller.EnvironmentController.Instance.BroadcastMessage += RegisterMe;
        }

        private void RegisterMe(string message)
        {
            if (message.Equals(ReferenceList[0].StringEval))
            {
                if (Next != null)
                {
                    ExecutionController.Instance.AddFlux(new Flux(this)
                    {
                        CurrentBlock = Next
                    });
                }
            }
        }

        public override void Destroy()
        {
            if (!sample)
                Controller.EnvironmentController.Instance.BroadcastMessage -= RegisterMe;
            base.Destroy();
        }

        public override string Description
        {
            get
            {
                return description;
            }
        }
    }

    public class StopPlayModeBlock : Block
    {
        public StopPlayModeBlock(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, BlockViewer viewer, bool sample) : base(owner, optionList, ScriptingType.control, referenceSlotViewers, viewer, sample)
        {
        }

        public static string description = "Stop play mode";

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override void Execute()
        {
            ExecutionController.Instance.Stop();
        }
    }
}
