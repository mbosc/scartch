using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using View;

namespace Scripting
{
    public class StartedPMHat : Hat
    {

        public static string description = "In Play Mode";
        private bool sample = false;

        public StartedPMHat(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, HatViewer viewer, bool sample) : base(owner, optionList, ScriptingType.control, referenceSlotViewers, viewer, sample)
        {
            this.sample = sample;
            if (!sample)
                Controller.EnvironmentController.Instance.InitiatingPlayMode += RegisterMe;
        }

        private void RegisterMe()
        {
            if (Next != null)
            {
                ExecutionController.Instance.AddFlux(new Flux(this)
                {
                    CurrentBlock = Next
                });
            }
        }

        public override void Destroy()
        {
            if (!sample)
                Controller.EnvironmentController.Instance.InitiatingPlayMode -= RegisterMe;
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
}
