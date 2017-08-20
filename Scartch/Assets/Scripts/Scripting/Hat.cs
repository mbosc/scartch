using System.Collections;
using System.Collections.Generic;
using System;
using View;
using Model;
using UnityEngine;

namespace Scripting
{
    public abstract class Hat : ScriptingElement
    {
        private HatViewer viewer;

        private Block next;

        public override Sprite Sprite
        {
            get
            {
                return ScartchResourceManager.instance.iconHat;
            }
        }

        public Hat(Actor owner, List<Option> optionList, ScriptingType type, List<ReferenceSlotViewer> referenceSlotViewers, HatViewer viewer, bool sample) : base(owner, optionList, type, referenceSlotViewers, sample)
        {
            if (!sample)
            {
                this.viewer = viewer;
                viewer.SnappedNext += OnViewerSnappedNext;
                viewer.UnsnappedNext += OnViewerUnsnappedNext;
                viewer.Type = type;

                referenceSlotViewers.ForEach(x =>
                {
                    viewer.Regrouped += x.Regroup;
                    viewer.Degrouped += x.Degroup;
                    x.LengthUpdated += viewer.UpdateLength;
                });
            }
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
            referenceSlotViewers.ForEach(x =>
            {
                viewer.Regrouped -= x.Regroup;
                viewer.Degrouped -= x.Degroup;
                x.LengthUpdated -= viewer.UpdateLength;
            });
        }

    }
}
