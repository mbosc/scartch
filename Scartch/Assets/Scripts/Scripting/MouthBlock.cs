using System.Collections;
using System.Collections.Generic;
using System;
using Model;
using View;

namespace Scripting
{
    public abstract class MouthBlock : Block
    {
        private Block innerNext;

        public Block InnerNext
        {
            get { return innerNext; }
            set { innerNext = value; }
        }

        private MouthBlockViewer MouthViewer
        {
            get
            {
                return viewer as MouthBlockViewer;
            }
        }

        public MouthBlock(Actor owner, List<RefType> referenceTypes, List<Option> optionList, ScriptingType type, List<ReferenceSlotViewer> referenceSlotViewers, MouthBlockViewer viewer) : base(owner, referenceTypes, optionList, type, referenceSlotViewers, viewer)
        {
            viewer.SnappedInnerNext += OnViewerSnappedInnerNext;
            viewer.UnsnappedInnerNext += OnViewerUnsnappedInnerNext;
        }

        private void OnViewerUnsnappedInnerNext()
        {
            InnerNext = null;
        }

        private void OnViewerSnappedInnerNext(BlockViewer obj)
        {
            InnerNext = obj.Block;
        }

        public override void Destroy()
        {
            base.Destroy();
            MouthViewer.SnappedInnerNext -= OnViewerSnappedInnerNext;
            MouthViewer.UnsnappedInnerNext -= OnViewerUnsnappedInnerNext;
        }
    }
}
