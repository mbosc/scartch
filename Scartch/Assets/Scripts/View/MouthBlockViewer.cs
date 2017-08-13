using System.Collections;
using System.Collections.Generic;
using Scripting;
using UnityEngine;
using System;

namespace View
{
    public class MouthBlockViewer : BlockViewer
    {
        private BlockViewer innerNext;

        public BlockViewer InnerNext
        {
            get { return innerNext; }
            set
            {
                //unsubscribe old
                if (innerNext != null)
                {
                    innerNext.Grabbed -= innerAttachPoint.Detach;
                    innerNext.HierarchyHeightChanged -= HeightUpdate;
                }

                //assign it
                innerNext = value;

                //align it
                if (innerNext != null)
                {
                    innerNext.transform.SetParent(this.innerAttachPoint.transform, false);
                    innerNext.transform.localEulerAngles = Vector3.zero;
                    innerNext.transform.localPosition = new Vector3(0, -4, 0);
                    innerNext.transform.SetParent(null);
                }

                //increase height porperly
                if (innerNext != null)
                    HeightUpdate(innerNext.HierarchyHeight);
                else
                    Height = 3;

                //subscribe new
                if (innerNext != null)
                {
                    innerNext.Grabbed += innerAttachPoint.Detach;
                    innerNext.HierarchyHeightChanged += HeightUpdate;
                }
            }
        }

        protected override void HierarchyHeightUpdate()
        {
            var nextLength = Next == null ? 0 : Next.HierarchyHeight;
            var innerNextLength = InnerNext == null ? 1 : InnerNext.HierarchyHeight;
            HierarchyHeight = 2 + nextLength + innerNextLength;
        }

        private void HeightUpdate(int height)
        {
            Height = 1 + 2 * height;
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

        public BlockAttachPoint innerAttachPoint;

        protected override void Start()
        {
            base.Start();
            attachPoints.Add(innerAttachPoint);
            innerAttachPoint.Attached += SnapInnerNext;
            innerAttachPoint.Detached += UnsnapInnerNext;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            innerAttachPoint.Attached -= SnapInnerNext;
            innerAttachPoint.Detached -= UnsnapInnerNext;
        }

        private int mbaseoffset = 3;
        public List<GameObject> bodies;
        public override int Length
        {
            get
            {
                return base.Length;
            }

            set
            {
                length = Math.Max(2, value);
                bodies.ForEach(x => x.transform.localPosition = new Vector3(mbaseoffset + (length - 1), x.transform.localPosition.y, 0));
                bodies.ForEach(x => x.transform.localScale = new Vector3(2 * (length - 1), 2, 2));
            }
        }

        private int height;
        public List<GameObject> lowerElements;
        public int Height
        {
            get { return height; }
            set
            {
                height = Math.Max(3, value);
                body.transform.localPosition = new Vector3(0, -3 - (height - 3) / 2.0f, 0);
                body.transform.localScale = new Vector3(2, 4 + (height - 3), 2);
                lowerElements.ForEach(x => x.transform.localPosition = new Vector3(x.transform.localPosition.x, -height - 1, 0));
                HierarchyHeightUpdate();
            }
        }

        public override void Regroup()
        {
            base.Regroup();
            if (InnerNext != null)
            {
                InnerNext.Regroup();
                InnerNext.transform.SetParent(this.transform);
            }
        }

        public override void Degroup()
        {
            base.Degroup();
            if (InnerNext != null)
            {
                InnerNext.Degroup();
                InnerNext.transform.SetParent(null);
            }
        }


        //DEBUG
        public string debugText;
        public int debugInt;
        protected override void Update()
        {
            base.Update();
            if (debugText != Text)
                Text = debugText;
        }

    }
}
