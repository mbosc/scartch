using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using View;

namespace Scripting
{
    public class SayBlock : Block
    {
        public SayBlock(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, BlockViewer viewer, bool sample) : base(owner, optionList, ScriptingType.look, referenceSlotViewers, viewer, sample)
        {
        }

        public static string description = "Say [  ]";

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override void Execute()
        {
            owner.Message = referenceList[0].StringEval;
            owner.IsMessageVisible = true;
            Flux.current.CurrentBlock = Next;
        }
    }
}
