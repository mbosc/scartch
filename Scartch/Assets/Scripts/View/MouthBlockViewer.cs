using System.Collections;
using System.Collections.Generic;
using Scripting;
using UnityEngine;

namespace View
{
    public class MouthBlockViewer : BlockViewer
    {
        private BlockViewer innerNext;

        public BlockViewer InnerNext
        {
            get { return innerNext; }
            set { innerNext = value; }
        }

        public override Block Block
        {
            get
            {
                return base.Block;
            }

            set
            {
                if (!(value is MouthBlock))
                    throw new System.ArgumentException("Mouth Block Viewer must receive Mouth Blocks");
                base.Block = value;
            }
        }

        public event System.Action<BlockViewer> SnappedInnerNext;
        public event System.Action UnsnappedInnerNext;

        public void SnapInnerNext(BlockViewer next)
        {
            InnerNext = next;
            if (SnappedInnerNext != null)
                SnappedInnerNext(Next);
        }

        public void UnsnapInnerNext()
        {
            InnerNext = null;
            if (UnsnappedInnerNext != null)
                UnsnappedInnerNext();
        }

    }
}
