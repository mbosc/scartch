using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using View;

namespace Scripting
{
    public class RotateBlock : Block
    {
        public RotateBlock(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, BlockViewer viewer, bool sample) : base(owner, optionList, ScriptingType.movement, referenceSlotViewers, viewer, sample)
        {
        }

        public static string description = "Rotate (  ) degrees around axis {x|y|z}";

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override void Execute()
        {
            var val = ReferenceList[0].FloatEval;
            var opt = GetOption(0).Value;
            if (opt == 0)
                owner.Rotation += new Vector3(val, 0, 0);
            else if (opt == 1)
                owner.Rotation += new Vector3(0, val, 0);
            else
                owner.Rotation += new Vector3(0, 0, val);

            Flux.current.CurrentBlock = Next;
        }
    }
}
