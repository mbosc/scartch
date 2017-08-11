using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    public class HatViewer : BlockoidViewer
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


        public GameObject hook, body, cap;
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
                cap.GetComponent<Renderer>().material = mat;

                // Determine best colour for lettering
                var deltawhite = Vector3.Distance(new Vector3(1, 1, 1), new Vector3(mat.color.r, mat.color.g, mat.color.b));
                var deltablack = Vector3.Distance(new Vector3(0, 0, 0), new Vector3(mat.color.r, mat.color.g, mat.color.b));
                textBox.color = deltawhite > deltablack ? Color.white : Color.black;
            }
        }

        private int length = 1, baseoffset = 1, lettersPerUnit = 4;
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
                cap.transform.localScale = new Vector3(value+1, 1, 1);
                cap.transform.localPosition = new Vector3(value, 1, 0);
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

        private void Detach()
        {
            this.Next = null;
        }
    }
}
