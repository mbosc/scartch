using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using View;

namespace Scripting
{
    public class SumExpression : ExpressionReference
    {

        public static string description = "(  ) + (  )";

        public SumExpression(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer, bool sample) : base(owner, optionList, ScriptingType.operators, referenceSlotViewers, viewer, RefType.numberType, sample)
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
            float val = ReferenceList[0].FloatEval + ReferenceList[1].FloatEval;
            return val.ToStdStr();
        }
    }
}
