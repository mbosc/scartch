using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using View;

namespace Scripting
{
    public class SetVolumeBlock : Block
    {
        public SetVolumeBlock(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, BlockViewer viewer, bool sample) : base(owner, optionList, ScriptingType.sound, referenceSlotViewers, viewer, sample)
        {
        }

        public static string description = "Set volume to (  )";

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override void Execute()
        {
            owner.Volume = referenceList[0].FloatEval;
            Flux.current.CurrentBlock = Next;
        }
    }
}
