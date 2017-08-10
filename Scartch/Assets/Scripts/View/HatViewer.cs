using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    public class HatViewer : Resources.ScriptingElementViewer
    {
        private BlockViewer next;

        public BlockViewer Next
        {
            get { return next; }
            set { next = value; }
        }

        public event System.Action<BlockViewer> SnappedNext;
        public event System.Action UnsnappedNext;

        public override void Moving()
        {
            throw new NotImplementedException();
        }

        public override void Release()
        {
            throw new NotImplementedException();
        }

        public void SnapNext(BlockViewer next)
        {
            Next = next;
            if (SnappedNext != null)
                SnappedNext(Next);
        }

        public void UnsnapNext()
        {
            Next = null;
            if (UnsnappedNext != null)
                UnsnappedNext();
        }
    }
}
