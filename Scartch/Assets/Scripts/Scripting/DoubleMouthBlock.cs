using System.Collections;
using System.Collections.Generic;
using System;
using Model;
using View;

namespace Scripting
{
    public abstract class DoubleMouthBlock : Block
    {
        private Block upperInnerNext;

        public Block UpperInnerNext
        {
            get { return upperInnerNext; }
            set { upperInnerNext = value; }
        }

        private Block lowerInnerNext;

        public Block LowerInnerNext
        {
            get { return lowerInnerNext; }
            set { lowerInnerNext = value; }
        }

        private DoubleMouthBlockViewer DoubleMouthViewer
        {
            get
            {
                return viewer as DoubleMouthBlockViewer;
            }
        }

        public DoubleMouthBlock(Actor owner, List<RefType> referenceTypes, List<Option> optionList, ScriptingType type, List<ReferenceSlotViewer> referenceSlotViewers, DoubleMouthBlockViewer viewer) : base(owner, referenceTypes, optionList, type, referenceSlotViewers, viewer)
        {
            viewer.SnappedLowerInnerNext += OnViewerSnappedLowerInnerNext;
            viewer.SnappedUpperInnerNext += OnViewerSnappedUpperInnerNext;
            viewer.UnsnappedLowerInnerNext += OnViewerUnsnappedLowerInnerNext;
            viewer.UnsnappedUpperInnerNext += OnViewerUnsnappedUpperInnerNext;
        }

        private void OnViewerUnsnappedUpperInnerNext()
        {
            UpperInnerNext = null;
        }

        private void OnViewerUnsnappedLowerInnerNext()
        {
            LowerInnerNext = null;
        }

        private void OnViewerSnappedUpperInnerNext(BlockViewer obj)
        {
            UpperInnerNext = obj.Block;
        }

        private void OnViewerSnappedLowerInnerNext(BlockViewer obj)
        {
            LowerInnerNext = obj.Block;
        }

        public override void Destroy()
        {
            base.Destroy();
            DoubleMouthViewer.SnappedLowerInnerNext += OnViewerSnappedLowerInnerNext;
            DoubleMouthViewer.SnappedUpperInnerNext += OnViewerSnappedUpperInnerNext;
            DoubleMouthViewer.UnsnappedLowerInnerNext += OnViewerUnsnappedLowerInnerNext;
            DoubleMouthViewer.UnsnappedUpperInnerNext += OnViewerUnsnappedUpperInnerNext;
        }
    }
}
