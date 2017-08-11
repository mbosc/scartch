using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace View
{
    public class BlockViewer : BlockoidViewer
    {
        private BlockViewer next;

        public override BlockViewer Next
        {
            get { return next; }
            set
            { 
                //unsubscribe old
                if (next != null)
                    next.Grabbed -= Detach;

                //assign it
                next = value;

                //align it
                if (next != null)
                {
                    next.transform.SetParent(this.transform);
                    next.transform.localEulerAngles = Vector3.zero;
                    next.transform.localPosition = new Vector3(0, -2, 0);
                    next.transform.SetParent(null);
                }

                //subscribe new
                if (next != null)
                    next.Grabbed += Detach;
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

        public GameObject hook, body;
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
                body.GetComponent<Renderer>().material = mat;
                hook.GetComponent<Renderer>().material = mat;

                // Determine best colour for lettering
                var deltawhite = Vector3.Distance(new Vector3(1, 1, 1), new Vector3(mat.color.r, mat.color.g, mat.color.b));
                var deltablack = Vector3.Distance(new Vector3(0, 0, 0), new Vector3(mat.color.r, mat.color.g, mat.color.b));
                textBox.color = deltawhite > deltablack ? Color.white : Color.black;
            }
        }

        private int length = 1, baseoffset = 1, lettersPerUnit = 4;
        private bool searchingNearest = false;
        public bool SearchingNearest
        {
            get { return searchingNearest; }
            set
            {
                searchingNearest = value;
                if (!value && Nearest != null)
                {
                    Nearest.Next = this;

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
        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                Length = 1 + (text.Length - 1) / lettersPerUnit;
                textBox.text = text;
            }
        }
        private void Detach(object sender, EventArgs e)
        {
            this.Next = null;
        }
        public int Length
        {
            get { return length; }
            set
            {
                length = Math.Max(1, value);
                body.transform.localPosition = new Vector3(baseoffset + value, 0, 0);
                body.transform.localScale = new Vector3(2 * value, 2, 2);
            }
        }

        private BlockoidViewer nearest;
        public BlockoidViewer Nearest
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
        public List<GameObject> highlightElements;
        public override void Highlight(bool doing)
        {
            highlightElements.ForEach(x => x.SetActive(doing));
        }
        public void Regroup()
        {
            if (Next != null)
            {
                Next.Regroup();
                Next.transform.SetParent(this.transform);
            }
        }

        public void Degroup()
        {
            if (Next != null)
            {
                Next.Degroup();
                Next.transform.SetParent(null);

            }
        }

        protected override void Update()
        {
            base.Update();

            if (searchingNearest)
                Nearest = FindNearest();
        }

        private BlockoidViewer FindNearest()
        {
            var min = float.PositiveInfinity;
            BlockoidViewer result = null;
            //coseno positivo

            var compatibleBlocks = FindObjectsOfType<BlockoidViewer>().ToList().Where(x => !x.Equals(this) && Mathf.Cos(Mathf.PI / 180 * Vector3.Angle(this.transform.up, x.transform.up)) > 0 &&
                Mathf.Abs(Vector3.Angle(-this.transform.up, (x.transform.position - this.transform.position).normalized)) > 90 && x.Next == null);
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

        private void Detach()
        {
            this.Next = null;
        }
    }
}
