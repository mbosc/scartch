using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
using System;
using System.Linq;

namespace View
{
    public class ReferenceSlotViewer : MonoBehaviour
    {
        private RefType type;

        public RefType Type
        {
            get { return type; }
            set
            {
                if (type != value)
                {
                    type = value;
                    var newTail = GameObject.Instantiate(ScartchResourceManager.instance.referenceHeads[type]);
                    var newHead = GameObject.Instantiate(ScartchResourceManager.instance.referenceHeads[type]);
                    newTail.transform.SetParent(this.transform, false);
                    newHead.transform.SetParent(this.transform, false);
                    newTail.transform.position = tail.transform.position;
                    newTail.transform.rotation = tail.transform.rotation;
                    newHead.transform.position = head.transform.position;
                    newHead.transform.rotation = head.transform.rotation;
                    newHead.transform.localScale = head.transform.localScale;
                    newTail.transform.localScale = tail.transform.localScale;
                    Destroy(tail.gameObject);
                    Destroy(head.gameObject);
                    tail = newTail;
                    head = newHead;
                }
            }

        }

        public GameObject head, tail, body;

        public event System.Action<int> SlotEmptied;
        public event System.Action<int, ReferenceViewer> SlotFilled;
        public event System.Action<int, int> LengthUpdated;

        public void FillSlot()
        {
            if (LengthUpdated != null)
                LengthUpdated(number, Filler.Length);
            if (SlotFilled != null)
                SlotFilled(number, Filler);
        }

        public void EmptySlot()
        {
            if (LengthUpdated != null)
                LengthUpdated(number, 4);
            if (SlotEmptied != null)
                SlotEmptied(number);
        }

        private int number;

        public int Number
        {
            set
            {
                number = value;
            }
        }

        public void Degroup()
        {
            if (Filler != null)
            {
                Filler.Degroup();
                Filler.transform.SetParent(null);
            }
        }

        public void Regroup()
        {
            if (Filler != null)
            {
                Filler.Regroup();
                Filler.transform.SetParent(this.transform);
            }
        }

        private ReferenceViewer filler;
        public List<GameObject> DisappearingElements { get { return new List<GameObject> { head, tail, body }; } }



        public ReferenceViewer Filler
        {
            get { return filler; }
            set
            {
                //unsubscribe old
                if (Filler != null)
                {
                    Filler.Grabbed -= Detach;
                    Filler.LengthUpdated -= Filler_LengthUpdated;
                }

                //assign it
                filler = value;

                //align it
                if (filler != null)
                {
                    filler.transform.SetParent(this.transform, false);
                    filler.transform.localEulerAngles = Vector3.zero;
                    filler.transform.localPosition = new Vector3(0, 0, 0);
                    filler.transform.localScale = this.transform.localScale;
                    filler.transform.SetParent(null);
                }

                //subscribe new
                if (filler != null) {
                    filler.Grabbed += Detach;
                    filler.LengthUpdated += Filler_LengthUpdated;
                }

                DisappearingElements.ForEach(x => x.SetActive(value == null));

                if (Filler == null)
                    EmptySlot();
                else
                    FillSlot();
            }
        }

        private void Filler_LengthUpdated(int obj)
        {
            if (LengthUpdated != null)
                LengthUpdated(number, obj);
        }

        private void Detach()
        {
            Filler = null;
        }

        public void Highlight(bool doing)
        {
            Material mat;
            if (doing)
                mat = ScartchResourceManager.instance.textBoxHighlighted;
            else
                mat = ScartchResourceManager.instance.textBoxNotHighlighted;
            DisappearingElements.ForEach(x => x.GetComponent<Renderer>().material = mat);
        }

        public bool ContainsInSub(ReferenceSlotViewer x)
        {
            bool subcont = false;
            if (Filler != null)
                subcont = Filler.ScriptingElement.RSV.Any(y => y.ContainsInSub(x));
            return this.Equals(x) || subcont;
        }
    }
}
