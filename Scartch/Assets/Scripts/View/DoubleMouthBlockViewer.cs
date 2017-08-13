using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripting;
using System;

namespace View
{
    public class DoubleMouthBlockViewer : BlockViewer
    {
        private BlockViewer upperInnerNext;

        public BlockViewer UpperInnerNext
        {
            get { return upperInnerNext; }
            set
            { 
                //unsubscribe old
                if (upperInnerNext != null)
                {
                    upperInnerNext.Grabbed -= upperInnerAttachPoint.Detach;
                    upperInnerNext.HierarchyHeightChanged -= UpperHeightUpdate;
                }

                //assign it
                upperInnerNext = value;

                //align it
                if (upperInnerNext != null)
                {
                    upperInnerNext.transform.SetParent(this.upperInnerAttachPoint.transform, false);
                    upperInnerNext.transform.localEulerAngles = Vector3.zero;
                    upperInnerNext.transform.localPosition = new Vector3(0, -4, 0);
                    upperInnerNext.transform.SetParent(null);
                }

                //increase height porperly
                if (upperInnerNext != null)
                    UpperHeightUpdate(upperInnerNext.HierarchyHeight);
                else
                    UpperHeight = 3;

                //subscribe new
                if (upperInnerNext != null)
                {
                    upperInnerNext.Grabbed += upperInnerAttachPoint.Detach;
                    upperInnerNext.HierarchyHeightChanged += UpperHeightUpdate;
                }
            }
        }

        private void UpperHeightUpdate(int hierarchyHeight)
        {
            UpperHeight = 1 + 2 * hierarchyHeight;
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
            set {
                //unsubscribe old
                if (lowerInnerNext != null)
                {
                    lowerInnerNext.Grabbed -= lowerInnerAttachPoint.Detach;
                    lowerInnerNext.HierarchyHeightChanged -= LowerHeightUpdate;
                }

                //assign it
                lowerInnerNext = value;

                //align it
                if (lowerInnerNext != null)
                {
                    lowerInnerNext.transform.SetParent(this.lowerInnerAttachPoint.transform, false);
                    lowerInnerNext.transform.localEulerAngles = Vector3.zero;
                    lowerInnerNext.transform.localPosition = new Vector3(0, -4, 0);
                    lowerInnerNext.transform.SetParent(null);
                }

                //increase height porperly
                if (lowerInnerNext != null)
                    LowerHeightUpdate(lowerInnerNext.HierarchyHeight);
                else
                    LowerHeight = 3;

                //subscribe new
                if (lowerInnerNext != null)
                {
                    lowerInnerNext.Grabbed += lowerInnerAttachPoint.Detach;
                    lowerInnerNext.HierarchyHeightChanged += LowerHeightUpdate;
                }
            }
        }

        protected override void HierarchyHeightUpdate()
        {
            var nextLength = Next == null ? 0 : Next.HierarchyHeight;
            var upperInnerNextLength = UpperInnerNext == null ? 1 : UpperInnerNext.HierarchyHeight;
            var lowerInnerNextLength = LowerInnerNext == null ? 1 : LowerInnerNext.HierarchyHeight;
            HierarchyHeight = 3 + nextLength + upperInnerNextLength + lowerInnerNextLength;
        }

        private void LowerHeightUpdate(int hierarchyHeight)
        {
            LowerHeight = 1 + 2 * hierarchyHeight;
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

        public BlockAttachPoint upperInnerAttachPoint, lowerInnerAttachPoint;

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

        protected override void Start()
        {
            base.Start();


            attachPoints.Add(upperInnerAttachPoint);
            upperInnerAttachPoint.Attached += SnapUpperInnerNext;
            upperInnerAttachPoint.Detached += UnsnapUpperInnerNext;
            attachPoints.Add(lowerInnerAttachPoint);
            lowerInnerAttachPoint.Attached += SnapLowerInnerNext;
            lowerInnerAttachPoint.Detached += UnsnapLowerInnerNext;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            upperInnerAttachPoint.Attached -= SnapUpperInnerNext;
            upperInnerAttachPoint.Detached -= UnsnapUpperInnerNext;
            lowerInnerAttachPoint.Attached -= SnapLowerInnerNext;
            lowerInnerAttachPoint.Detached -= UnsnapLowerInnerNext;
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
        private int upperHeight;
        public GameObject lowerBody;
        public List<GameObject> upperMiddleElements;
        public int UpperHeight
        {
            get { return upperHeight; }
            set
            {
                upperHeight = Math.Max(3, value);
                body.transform.localPosition = new Vector3(0, -3 - (upperHeight - 3) / 2.0f, 0);
                body.transform.localScale = new Vector3(2, 4 + (upperHeight - 3), 2);
                upperMiddleElements.ForEach(x => x.transform.localPosition = new Vector3(x.transform.localPosition.x, -upperHeight - 1, 0));

                //Move lower middle elements
                lowerBody.transform.localPosition = new Vector3(0, -3 - upperHeight - 1 - (lowerHeight - 3) / 2.0f, 0);
                lowerMiddleElements.ForEach(x => x.transform.localPosition = new Vector3(x.transform.localPosition.x, -upperHeight - 1 - lowerHeight - 1, 0));

                //realign blocks
                if (LowerInnerNext != null)
                {
                    LowerInnerNext.Regroup();
                    lowerInnerNext.transform.SetParent(this.lowerInnerAttachPoint.transform, false);
                    lowerInnerNext.transform.localEulerAngles = Vector3.zero;
                    lowerInnerNext.transform.localPosition = new Vector3(0, -4, 0);
                    lowerInnerNext.transform.SetParent(null);
                    LowerInnerNext.Degroup();
                }

                HierarchyHeightUpdate();
            }
        }
        private int lowerHeight;
        public List<GameObject> lowerMiddleElements;
        public int LowerHeight
        {
            get { return lowerHeight; }
            set
            {
                lowerHeight = Math.Max(3, value);
                lowerBody.transform.localPosition = new Vector3(0, -3 - upperHeight - 1 - (lowerHeight - 3) / 2.0f, 0);
                lowerBody.transform.localScale = new Vector3(2, 4 + (lowerHeight - 3), 2);
                lowerMiddleElements.ForEach(x => x.transform.localPosition = new Vector3(x.transform.localPosition.x, -upperHeight -1 -lowerHeight - 1, 0));
                HierarchyHeightUpdate();
            }
        }

        public override void Regroup()
        {
            base.Regroup();
            if (UpperInnerNext != null)
            {
                UpperInnerNext.Regroup();
                UpperInnerNext.transform.SetParent(this.transform);
            }
            if (LowerInnerNext != null)
            {
                LowerInnerNext.Regroup();
                LowerInnerNext.transform.SetParent(this.transform);
            }
        }

        public override void Degroup()
        {
            base.Degroup();
            if (UpperInnerNext != null)
            {
                UpperInnerNext.Degroup();
                UpperInnerNext.transform.SetParent(null);
            }
            if (LowerInnerNext != null)
            {
                LowerInnerNext.Degroup();
                LowerInnerNext.transform.SetParent(null);
            }
        }

        public UnityEngine.UI.Text secondTextBox;
        private string secText;
        public string SecText
        {
            get { return secText; }
            set
            {
                secText = value;
                Length = Math.Max(1 + (text.Length - 1) / lettersPerUnit, 4 + 1 + (secText.Length - 1) / lettersPerUnit);
                secondTextBox.text = secText;
            }
        }
        public override string Text
        {
            get
            {
                return text;
            }

            set
            {
                text = value;
                Length = Math.Max(1 + (text.Length - 1) / lettersPerUnit, 4 + 1 + (secText.Length - 1) / lettersPerUnit);
                textBox.text = text;
            }
        }
    }
}
