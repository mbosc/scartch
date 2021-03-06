﻿using System.Collections;
using System.Collections.Generic;
using System;
using Model;
using View;

namespace Scripting
{
    public abstract class ExpressionReference : Reference
    {
        private RefType rType;

        public override RefType GetRefType()
        {
            return rType;
        }

        public ExpressionReference(Actor owner, List<Option> optionList, ScriptingType type, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer, RefType rType, bool sample) : base(owner, optionList, type, referenceSlotViewers, viewer, sample)
        {
            if (!sample)
            {
                this.rType = rType;
                viewer.RefType = rType;
            }
        }
    }
}
