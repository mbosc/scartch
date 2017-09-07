using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace View.Resources
{
    public class VRComboboxVoice : VRButton
    {
        public GameObject head, tail, body;
        private int length = 1;
        public UnityEngine.UI.Text textBox;
        private string text;
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = " " + value + " ";
                textBox.text = text;
            }
        }
        private Scripting.ScriptingType type;
        public Scripting.ScriptingType Type
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
                head.GetComponent<Renderer>().material = mat;
                tail.GetComponent<Renderer>().material = mat;
                body.GetComponent<Renderer>().material = mat;

                // Determine best colour for lettering
                var deltawhite = Vector3.Distance(new Vector3(1, 1, 1), new Vector3(mat.color.r, mat.color.g, mat.color.b));
                var deltablack = Vector3.Distance(new Vector3(0, 0, 0), new Vector3(mat.color.r, mat.color.g, mat.color.b));
                textBox.color = deltawhite > deltablack ? Color.white : Color.black;
            }
        }

        public int Length
        {
            get { return length; }
            set
            {
                length = Math.Max(2, value);
                UpdateLength();
            }
        }

        private void UpdateLength()
        {
            //posiziona correttamente
            head.transform.localEulerAngles = ScartchResourceManager.instance.headRotation;
            head.transform.localPosition = Vector3.zero;
            head.transform.localScale = new Vector3(1, 1, 1);
            tail.transform.localEulerAngles = ScartchResourceManager.instance.tailRotation;
            tail.transform.localPosition = new Vector3(Length / 2.0f, 0, 0);
            tail.transform.localScale = new Vector3(1, 1, 1);
            body.transform.localEulerAngles = ScartchResourceManager.instance.bodyRotation;
            body.transform.localPosition = new Vector3(Length / 4.0f, 0, 0);
            body.transform.localScale = new Vector3(Length - 2, 1, 1);
        }

       

    }
}