using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    public class BlockViewer : Resources.ScriptingElementViewer
    {
        private BlockViewer next;
        
        public BlockViewer Next
        {
            get { return next; }
            set { next = value; }
        }

        private Scripting.Block block;

        virtual public Scripting.Block Block
        {
            get { return block; }
            set { block = value; }
        }


        public event System.Action<BlockViewer> SnappedNext;
        public event System.Action UnsnappedNext, Tested;

        public override void HitByBlueRay()
        {
            Test();
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

        public void Test()
        {
            if (Tested != null)
                Tested();
        }
    }
}
