using System.Collections;
using System.Collections.Generic;
using System;
using Model;
using View;
using System.Linq;

namespace Scripting
{
    public abstract class ScriptingElement
    {
        protected Actor owner;
        protected Dictionary<int, Reference> referenceList;
        protected List<RefType> referenceTypes;
        protected List<Option> optionList;
        protected ScriptingType type;
        public event Action Destroyed;
        protected List<ReferenceSlotViewer> referenceSlotViewers;

        public abstract string Description { get; }

        public abstract UnityEngine.Sprite Sprite
        {
            get;
        }

        public ScriptingElement(Actor owner, List<RefType> referenceTypes, List<Option> optionList, ScriptingType type, List<ReferenceSlotViewer> referenceSlotViewers)
        {
            this.owner = owner;
            this.referenceTypes = referenceTypes;
            this.optionList = optionList;
            this.type = type;
            this.referenceSlotViewers = referenceSlotViewers;

            // initialise references dictionary
            referenceList = new Dictionary<int, Reference>();
            int refcount = 0;
            referenceTypes.ForEach(x => referenceList.Add(refcount++, null));

            // subscribe to referenceSlotViewers' events
            referenceSlotViewers.ForEach(x =>
            {
                x.SlotFilled += OnReferenceSlotViewerFilled;
                x.SlotEmptied += OnReferenceSlotViewerEmptied;
            });

        }

        private void OnReferenceSlotViewerEmptied(int num)
        {
            referenceList[num] = null;
        }

        private void OnReferenceSlotViewerFilled(int num, ReferenceViewer fillee)
        {
            referenceList[num] = fillee.Reference;
        }

        public bool TryGetReference(int num, out Reference reference)
        {
            reference = referenceList[num];
            return reference != null;
        }

        public Option GetOption(int num)
        {
            return optionList[num];
        }

        public virtual void Destroy()
        {
            referenceSlotViewers.ForEach(x =>
            {
                x.SlotFilled -= OnReferenceSlotViewerFilled;
                x.SlotEmptied -= OnReferenceSlotViewerEmptied;
            });
            if (Destroyed != null)
                Destroyed();
        }
    }
}