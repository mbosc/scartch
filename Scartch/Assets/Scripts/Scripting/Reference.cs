using System.Collections;
using System.Collections.Generic;
using System;
using View;
using Model;
using UnityEngine;

namespace Scripting
{
    public abstract class Reference : ScriptingElement
    {
        protected ReferenceViewer viewer;

        public Reference(Actor owner, List<Option> optionList, ScriptingType type, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer) : base(owner, optionList, type, referenceSlotViewers)
        {
            this.viewer = viewer;
            viewer.Reference = this;
            viewer.Type = type;
            viewer.Init(this);
            referenceSlotViewers.ForEach(x =>
            {
                viewer.Regrouped += x.Regroup;
                viewer.Degrouped += x.Degroup;
                x.LengthUpdated += viewer.UpdateLength;
            });
        }

        public override Sprite Sprite
        {
            get
            {
                switch (GetRefType()) {
                    case RefType.boolType: return ScartchResourceManager.instance.iconBool;
                    case RefType.numberType: return ScartchResourceManager.instance.iconNum;
                    case RefType.stringType: return ScartchResourceManager.instance.iconString;
                    default: throw new ArgumentException("What did you give me?");
                }
            }
        }

        public abstract RefType GetRefType();
        public abstract string Evaluate();


        public override void Destroy()
        {
            base.Destroy();
            referenceSlotViewers.ForEach(x =>
            {
                viewer.Regrouped -= x.Regroup;
                viewer.Degrouped -= x.Degroup;
                x.LengthUpdated -= viewer.UpdateLength;
            });
        }
    }
}
