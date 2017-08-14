using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace View
{
    public class BlockViewer : BlockoidViewer
    {
        public BlockAttachPoint attachPoint;
        private BlockViewer next;
        protected List<BlockAttachPoint> attachPoints;
        protected override void Start()
        {
            base.Start();
            attachPoints = new List<BlockAttachPoint>();
            attachPoints.Add(attachPoint);
            HierarchyHeightUpdate();
            attachPoint.Attached += SnapNext;
            attachPoint.Detached += UnsnapNext;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            attachPoint.Attached -= SnapNext;
            attachPoint.Detached -= UnsnapNext;
        }

        public override BlockViewer Next
        {
            get { return next; }
            set
            {
                //unsubscribe old
                if (next != null)
                {
                    next.Grabbed -= attachPoint.Detach;
                    Regrouped -= next.Regroup;
                    Degrouped -= next.Degroup;
                }

                //assign it
                next = value;

                //align it
                if (next != null)
                {
                    next.transform.SetParent(this.attachPoint.transform, false);
                    next.transform.localEulerAngles = Vector3.zero;
                    next.transform.localPosition = new Vector3(0, -4, 0);
                    next.transform.SetParent(null);
                }

                HierarchyHeightUpdate();

                //subscribe new
                if (next != null)
                {
                    next.Grabbed += attachPoint.Detach;
                    Regrouped += next.Regroup;
                    Degrouped += next.Degroup;
                }
            }
        }

        private Scripting.Block block;

        virtual public Scripting.Block Block
        {
            get { return block; }
            set { block = value; }
        }


        public event System.Action<BlockViewer> SnappedNext;
        public event System.Action UnsnappedNext, Tested;

        public void SnapNext(BlockViewer next)
        {
            Next = next;
            HierarchyHeightUpdate();
            if (SnappedNext != null)
                SnappedNext(Next);
        }

        private int hierarchyHeight;

        public int HierarchyHeight
        {
            get { return hierarchyHeight; }
            set { hierarchyHeight = value;
                if (HierarchyHeightChanged != null)
                    HierarchyHeightChanged(value);
            }
        }

        protected virtual void HierarchyHeightUpdate()
        {
            var nextLength = next == null ? 0 : next.HierarchyHeight;
            HierarchyHeight = 1 + nextLength;
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

        public GameObject body;
        public List<GameObject> elementsToPaint;
        private Scripting.ScriptingType type;
        public override Scripting.ScriptingType Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;

                // Update block colour
                var mat = ScartchResourceManager.instance.blockTypeMaterials[Type];
                elementsToPaint.ForEach(x => x.GetComponent<Renderer>().material = mat);

                // Determine best colour for lettering
                var deltawhite = Vector3.Distance(new Vector3(1, 1, 1), new Vector3(mat.color.r, mat.color.g, mat.color.b));
                var deltablack = Vector3.Distance(new Vector3(0, 0, 0), new Vector3(mat.color.r, mat.color.g, mat.color.b));
                textBox.color = deltawhite > deltablack ? Color.white : Color.black;
            }
        }

        protected int length = 1;
        protected int baseoffset = 1, lettersPerUnit = 4;
        private bool searchingNearest = false;
        public bool SearchingNearest
        {
            get { return searchingNearest; }
            set
            {
                searchingNearest = value;
                if (!value && Nearest != null)
                {
                    Nearest.Attach(this);

                    Nearest = null;
                }
            }
        }
        public void StartSearchingNearest()
        {
            SearchingNearest = true;
        }
        public void StopSearchingNearest()
        {
            SearchingNearest = false;

        }
        public UnityEngine.UI.Text textBox;
        protected string text;
        public virtual string Text
        {
            get { return text; }
            set
            {
                text = value;
                Length = 1 + (text.Length - 1) / lettersPerUnit;
                textBox.text = text;
            }
        }
        public virtual int Length
        {
            get { return length; }
            set
            {
                length = Math.Max(1, value);
                body.transform.localPosition = new Vector3(baseoffset + length, 0, 0);
                body.transform.localScale = new Vector3(2 * length, 2, 2);
            }
        }

        private BlockAttachPoint nearest;
        public BlockAttachPoint Nearest
        {
            get { return nearest; }
            set
            {
                if (nearest != null)
                    nearest.Highlight(false);
                nearest = value;
                if (nearest != null)
                    nearest.Highlight(true);
            }
        }

        public event Action Regrouped, Degrouped;

        public virtual void Regroup()
        {
            if (Regrouped != null)
                Regrouped();
            if (Next != null)
            {
                Next.transform.SetParent(this.transform);
            }
        }

        public virtual void Degroup()
        {
            if (Degrouped != null)
                Degrouped();
            if (Next != null)
            {
                Next.transform.SetParent(null);

            }
        }


        public string debugtxt;
        public bool locked;
        protected override void Update()
        {
            base.Update();

            if (searchingNearest)
                Nearest = FindNearest();

            //SUPERDEBUGGO
            if (debugtxt != Text && !locked)
                Text = debugtxt;
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                locked = true;
                var text = Text;
                Scripting.ScriptingElement.GenerateViewersFromText(ref text, this.gameObject).ForEach(x => { Regrouped += x.Regroup; Degrouped += x.Degroup; }) ;
                Scripting.ScriptingElement.GenerateOptionsFromText(ref text, this.gameObject);
                Text = text;
            }
        }

        private BlockAttachPoint FindNearest()
        {
            var min = float.PositiveInfinity;
            BlockAttachPoint result = null;

            var compatibleBlocks = FindObjectsOfType<BlockAttachPoint>().ToList().Where(x => !attachPoints.Contains(x) && Mathf.Cos(Mathf.PI / 180 * Vector3.Angle(this.transform.up, x.transform.up)) > 0 &&
                Mathf.Abs(Vector3.Angle(-this.transform.up, (x.transform.position - this.transform.position).normalized)) > 90 && x.Free);
            compatibleBlocks.ToList().ForEach(x => { if (Vector3.Distance(this.transform.position, x.transform.position) < min) { result = x; min = Vector3.Distance(this.transform.position, x.transform.position); } });
            if (min < ScartchResourceManager.instance.blockSnapThreshold * this.transform.localScale.x)
                return result;

            else return null;
        }

        public override void Moving()
        {
            throw new NotImplementedException();
        }

        public override void Release()
        {
            throw new NotImplementedException();
        }

        public event System.Action<int> HierarchyHeightChanged;


    }
}
