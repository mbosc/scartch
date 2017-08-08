using System.Collections;
using System.Collections.Generic;
using System;
using Model;
using View;

namespace Scripting
{
    public abstract class Block : ScriptingElement
    {
        private Block next;

        public Block(Actor owner, List<RefType> referenceTypes, List<Option> optionList, ScriptingType type, List<ReferenceSlotViewer> referenceSlotViewers, BlockViewer viewer) : base(owner, referenceTypes, optionList, type, referenceSlotViewers)
        {
            this.viewer = viewer;

            viewer.SnappedNext += OnViewerSnappedNext;
            viewer.UnsnappedNext += OnViewerUnsnappedNext;
            viewer.Tested += OnViewerTested;
        }

        private void OnViewerTested()
        {
            //TODO
            // Genera un singolo flusso e passa a play mode
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

        public abstract void Execute();

        protected BlockViewer viewer;

        public override void Destroy()
        {
            base.Destroy();
            viewer.SnappedNext -= OnViewerSnappedNext;
            viewer.UnsnappedNext -= OnViewerUnsnappedNext;
            viewer.Tested -= OnViewerTested;
        }
    }
}

