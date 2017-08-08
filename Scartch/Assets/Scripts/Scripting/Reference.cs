using System.Collections;
using System.Collections.Generic;
using System;
using View;
using Model;

namespace Scripting
{
    public abstract class Reference : ScriptingElement
    {
        protected ReferenceViewer viewer;

        public Reference(Actor owner, List<RefType> referenceTypes, List<Option> optionList, ScriptingType type, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer) : base(owner, referenceTypes, optionList, type, referenceSlotViewers)
        {
            this.viewer = viewer;
            viewer.Reference = this;
        }

        public abstract RefType GetRefType();
        public abstract string Evaluate();
    }
}
