using System.Collections;
using System.Collections.Generic;
using System;
using View;
using Model;

namespace Scripting
{
    public class Hat : ScriptingElement
    {
        private HatViewer viewer;

        private Block next;

        public Hat(Actor owner, List<RefType> referenceTypes, List<Option> optionList, ScriptingType type, List<ReferenceSlotViewer> referenceSlotViewers, HatViewer viewer) : base(owner, referenceTypes, optionList, type, referenceSlotViewers)
        {
            this.viewer = viewer;

            viewer.SnappedNext += OnViewerSnappedNext;
            viewer.UnsnappedNext += OnViewerUnsnappedNext;
        }

        private void OnViewerUnsnappedNext()
        {
            Next = null;
        }

        private void OnViewerSnappedNext(BlockViewer obj)
        {
            Next = obj.Block;
        }

        public Block Next
        {
            get { return next; }
            set { next = value; }
        }

        public override void Destroy()
        {
            base.Destroy();
            viewer.SnappedNext -= OnViewerSnappedNext;
            viewer.UnsnappedNext -= OnViewerUnsnappedNext;
        }

    }
}
