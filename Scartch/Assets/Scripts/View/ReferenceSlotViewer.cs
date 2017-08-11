using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;

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

        public void FillSlot()
        {
            if (SlotFilled != null)
                SlotFilled(number, Filler);
        }

        public void EmptySlot()
        {
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

        private ReferenceViewer filler;
        public List<GameObject> Scomparsa { get { return new List<GameObject> { head, tail, body }; } }



        public ReferenceViewer Filler
        {
            get { return filler; }
            set
            {
                //unsubscribe old
                if (Filler != null)
                    Filler.Grabbed -= Detach;

                //assign it
                filler = value;

                //align it
                if (filler != null)
                {
                    filler.transform.SetParent(this.transform, false);
                    filler.transform.localEulerAngles = Vector3.zero;
                    filler.transform.localPosition = new Vector3(0, 0, 0);
                    filler.transform.SetParent(null);
                }

                //subscribe new
                if (filler != null)
                    filler.Grabbed += Detach;

                Scomparsa.ForEach(x => x.SetActive(value == null));
            }
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
            Scomparsa.ForEach(x => x.GetComponent<Renderer>().material = mat);
        }
    }
}
