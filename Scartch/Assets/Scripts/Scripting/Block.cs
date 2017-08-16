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

        public Block(Actor owner, List<Option> optionList, ScriptingType type, List<ReferenceSlotViewer> referenceSlotViewers, BlockViewer viewer, bool sample) : base(owner, optionList, type, referenceSlotViewers, sample)
        {
            if(!sample)
            {
                this.viewer = viewer;
                viewer.Init(this);

                viewer.SnappedNext += OnViewerSnappedNext;
                viewer.UnsnappedNext += OnViewerUnsnappedNext;
                viewer.Tested += OnViewerTested;
                viewer.Type = type;

                referenceSlotViewers.ForEach(x =>
                {
                    viewer.Regrouped += x.Regroup;
                    viewer.Degrouped += x.Degroup;
                    x.LengthUpdated += viewer.UpdateLength;
                });
            }
        }

        public override UnityEngine.Sprite Sprite
        {
            get
            {
                return ScartchResourceManager.instance.iconBlock;
            }
        }

        private void OnViewerTested()
        {
            UnityEngine.Debug.Log("Testerino");
            ExecutionController.Instance.AddFlux(new Flux(this)
            {
                CurrentBlock = this
            });
            ExecutionController.Instance.ExecuteTest();
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
            referenceSlotViewers.ForEach(x =>
            {
                viewer.Regrouped -= x.Regroup;
                viewer.Degrouped -= x.Degroup;
                x.LengthUpdated -= viewer.UpdateLength;
            });
        }
    }
}

