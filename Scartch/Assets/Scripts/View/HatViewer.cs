using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

                //subscribe new
                if (next != null)
                {
                    next.Grabbed += attachPoint.Detach;
                    Regrouped += next.Regroup;
                    Degrouped += next.Degroup;
                }
            }
        }

        public event System.Action<BlockViewer> SnappedNext;
        public event System.Action UnsnappedNext;

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

        public void UpdateLength(int num, int val)
        {
            Debug.Log("Reference " + num + " becomes long " + val);
            bool closing = false;
            bool moving = false;
            int delta = 0;
            char charToClose = 'a';
            string outString = "";
            int refCounter = 0;
            int optCounter = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (!closing)
                {
                    if (Scripting.ScriptingElement.refOpeningChars.Contains(text[i]))
                    {
                        if (num != refCounter)
                        {
                            if (!moving)
                                refCounter++;
                            else
                            {
                                var rsv = ScriptingElement.RSV[refCounter++];
                                rsv.Regroup();
                                rsv.transform.localPosition += new Vector3(delta / 2.0f, 0, 0);
                                rsv.Degroup();
                            }
                        }
                        else
                        {
                            closing = true;
                            charToClose = Scripting.ScriptingElement.refClosingChars[Scripting.ScriptingElement.refOpeningChars.IndexOf(text[i])];
                            refCounter++;
                            delta = i;
                        }
                    }
                    if (Scripting.ScriptingElement.optOpeningChars.Contains(text[i]))
                    {
                        if (moving)
                        {
                            ScriptingElement.OPT[optCounter++].Viewer.Combo.transform.localPosition += new Vector3(delta / 2.0f, 0, 0);
                        }
                        optCounter++;
                    }
                    outString += text[i];
                }
                else if (closing && text[i] == charToClose)
                {
                    closing = false;
                    moving = true;
                    delta = val - (i - delta + 1);
                    outString += new string(' ', val - 2);
                    outString += text[i];
                }
            }
            Text = outString;
        }

        public List<GameObject> highlightElements;
        public BlockAttachPoint attachPoint;
        public event Action Regrouped, Degrouped;

        protected override void Start()
        {
            base.Start();
            attachPoint.Attached += SnapNext;
            attachPoint.Detached += UnsnapNext;
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            attachPoint.Attached -= SnapNext;
            attachPoint.Detached -= UnsnapNext;
        }

        public void Regroup()
        {
            if (Regrouped != null)
                Regrouped();
            if (Next != null)
            {
                Next.transform.SetParent(this.transform);
            }
        }

        public void Degroup()
        {
            if (Degrouped != null)
                Degrouped();
            if (Next != null)
            {
                Next.transform.SetParent(null);
            }
        }
    }
}
