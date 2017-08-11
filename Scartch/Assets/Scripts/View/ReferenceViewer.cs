﻿using System;
using System.Collections;
using System.Collections.Generic;
using Scripting;
using UnityEngine;
using System.Linq;

namespace View
{
    public class ReferenceViewer : Resources.ScriptingElementViewer
    {
        public Reference Reference { get; set; }
        private Model.RefType refType;

        public Model.RefType RefType
        {
            get { return refType; }
            set
            {
                refType = value;
                UpdateAppearance();
            }
        }

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

                //Update objects' colours
                var mat = ScartchResourceManager.instance.blockTypeMaterials[(Scripting.ScriptingType)color];
                body.GetComponent<Renderer>().material = mat;
                head.GetComponent<Renderer>().material = mat;
                tail.GetComponent<Renderer>().material = mat;

                //Determine best colour for lettering
                var deltawhite = Vector3.Distance(new Vector3(1, 1, 1), new Vector3(mat.color.r, mat.color.g, mat.color.b));
                var deltablack = Vector3.Distance(new Vector3(0, 0, 0), new Vector3(mat.color.r, mat.color.g, mat.color.b));
                textBox.color = deltawhite > deltablack ? Color.white : Color.black;
                
            }
        }

        public override void Moving()
        {
            throw new NotImplementedException();
        }

        public override void Release()
        {
            throw new NotImplementedException();
        }

        private GameObject head, tail, body;
        private int length = 1;
        public UnityEngine.UI.Text textBox;
        private string text;
        public string Text
        {
            get { return text; }
            set { text = value;
                textBox.text = text;
                Length = text.Length;
            }
        }
        private BoxCollider coll;

        protected override void Start()
        {
            base.Start();
            coll = gameObject.GetComponent<BoxCollider>();
            UpdateAppearance();
        }

        public int Length
        {
            get { return length; }
            set
            {
                length = Math.Max(2, value);
                UpdateAppearance();
            }
        }

        private void UpdateAppearance()
        {
            // Destroy old head and tail
            if (head != null) Destroy(head);
            if (tail != null) Destroy(tail);

            // Instantiate new elements so as to possibly match new variable type
            head = GameObject.Instantiate(ScartchResourceManager.instance.referenceHeads[refType]);
            tail = GameObject.Instantiate(ScartchResourceManager.instance.referenceHeads[refType]);

            // Set hierarchy up
            head.transform.SetParent(this.transform, false);
            tail.transform.SetParent(this.transform, false);

            // Set up body on first execution
            if (body == null) body = GameObject.Instantiate(ScartchResourceManager.instance.referenceBody);
            body.transform.SetParent(this.transform, false);

            // Update positions
            head.transform.localEulerAngles = ScartchResourceManager.instance.headRotation;
            head.transform.localPosition = Vector3.zero;
            head.transform.localScale = new Vector3(1, 1, 1);
            tail.transform.localEulerAngles = ScartchResourceManager.instance.tailRotation;
            tail.transform.localPosition = new Vector3(Length / 2.0f, 0, 0);
            tail.transform.localScale = new Vector3(1, 1, 1);
            body.transform.localEulerAngles = ScartchResourceManager.instance.bodyRotation;
            body.transform.localPosition = new Vector3(Length / 4.0f, 0, 0);
            body.transform.localScale = new Vector3(Length - 2, 1, 1);

            // Update collider
            coll.center = new Vector3(.25f * Length, 0, -.25f);
            coll.size = new Vector3(.5f * Length, 2, .5f);
        }


        private int color = 0;

        protected override void Update()
        {
            base.Update();

            if (searchingNearest)
                Nearest = FindNearest();
        }

        private ReferenceSlotViewer nearest;
        public ReferenceSlotViewer Nearest
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



        private bool searchingNearest = false;
        public bool SearchingNearest
        {
            get { return searchingNearest; }
            set
            {
                searchingNearest = value;
                if (!value && Nearest != null)
                {
                    Nearest.Filler = this;

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


        private ReferenceSlotViewer FindNearest()
        {
            var min = float.PositiveInfinity;
            ReferenceSlotViewer result = null;
            //coseno positivo

            var compatibleSlots = FindObjectsOfType<ReferenceSlotViewer>().ToList().Where(x => x.Filler == null);

            compatibleSlots.ToList().ForEach(x => { if (Vector3.Distance(this.transform.position, x.transform.position + x.transform.right - x.transform.forward * 0.5f) < min) { result = x; min = Vector3.Distance(this.transform.position, x.transform.position); } });

            if (min < ScartchResourceManager.instance.referenceSnapThreshold * this.transform.localScale.x)
                return result;

            else return null;
        }
    }
}
