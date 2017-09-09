using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using View;

namespace Scripting
{
    public class BroadcastBlock : Block, Broadcaster
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
            //Debug.Log("Broadcasting " + ReferenceList[0].StringEval);
            Controller.EnvironmentController.Instance.Broadcast(this, ReferenceList[0].StringEval);
            Flux.current.CurrentBlock = Next;
        }

        private int refCounter = 0;

        public void Ack(object source)
        {
            refCounter++;
            //Debug.Log("Received ACK " + refCounter);
        }

        public void Bye(object s)
        {

        }
    }

    public class BroadcastAndWaitBlock : Block, Broadcaster
    {
        public BroadcastAndWaitBlock(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, BlockViewer viewer, bool sample) : base(owner, optionList, ScriptingType.control, referenceSlotViewers, viewer, sample)
        {
            Controller.EnvironmentController.Instance.InitiatingPlayMode += ResetState;
        }

        public override void Destroy()
        {
            base.Destroy();
            Controller.EnvironmentController.Instance.InitiatingPlayMode -= ResetState;
        }

        private void ResetState()
        {
            broadcast = false;
            proceed = false;
        }

        public static string description = "Broadcast [  ] and wait";

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override void Execute()
        {
            if (!broadcast)
            {
                Debug.Log("Broadcasting " + ReferenceList[0].StringEval + " and waiting");
                Controller.EnvironmentController.Instance.Broadcast(this, ReferenceList[0].StringEval);
                broadcast = true;
            }
            if (!proceed)
                Flux.current.CurrentBlock = this;
            else
                Flux.current.CurrentBlock = Next;
        }

        private int refCounter = 0;
        private bool proceed = false;
        private bool broadcast = false;

        public void Ack(object source)
        {
            refCounter++;
            Debug.Log("Received ACK " + refCounter);
        }

        public void Bye(object source)
        {
            Debug.Log("Received BYE " + refCounter);
            refCounter--;
            if (refCounter == 0)
                proceed = true;
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



        private void RegisterMe(Broadcaster source, string message)
        {
            if (message.Equals(ReferenceList[0].StringEval))
            {
                if (Next != null)
                {
                    var flux = new Flux(this)
                    {
                        CurrentBlock = Next

                    };
                    Action fun = null;
                    fun = () => { source.Bye(this); flux.Callbacks -= fun; };
                    flux.Callbacks += fun;
                    ExecutionController.Instance.AddFlux(flux);
                    source.Ack(this);
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
