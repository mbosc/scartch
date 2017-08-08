using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripting;

namespace View
{
    public class DoubleMouthBlockViewer : BlockViewer
    {
        private BlockViewer upperInnerNext;

        public BlockViewer UpperInnerNext
        {
            get { return upperInnerNext; }
            set { upperInnerNext = value; }
        }

        public event System.Action<BlockViewer> SnappedUpperInnerNext;
        public event System.Action UnsnappedUpperInnerNext;

        public void SnapUpperInnerNext(BlockViewer next)
        {
            UpperInnerNext = next;
            if (SnappedUpperInnerNext != null)
                SnappedUpperInnerNext(Next);
        }

        public void UnsnapUpperInnerNext()
        {
            UpperInnerNext = null;
            if (UnsnappedUpperInnerNext != null)
                UnsnappedUpperInnerNext();
        }

        private BlockViewer lowerInnerNext;

        public BlockViewer LowerInnerNext
        {
            get { return lowerInnerNext; }
            set { lowerInnerNext = value; }
        }

        public event System.Action<BlockViewer> SnappedLowerInnerNext;
        public event System.Action UnsnappedLowerInnerNext;

        public void SnapLowerInnerNext(BlockViewer next)
        {
            LowerInnerNext = next;
            if (SnappedLowerInnerNext != null)
                SnappedLowerInnerNext(Next);
        }

        public void UnsnapLowerInnerNext()
        {
            LowerInnerNext = null;
            if (UnsnappedLowerInnerNext != null)
                UnsnappedLowerInnerNext();
        }

        public override Block Block
        {
            get
            {
                return base.Block;
            }

            set
            {
                if (!(value is DoubleMouthBlock))
                    throw new System.ArgumentException("Double Mouth Block Viewer must receive Double Mouth Blocks");
                base.Block = value;
            }
        }
    }
}
