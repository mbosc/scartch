using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    namespace Resources
    {
        public class VRTextbox : MonoBehaviour
        {
            private string text;
            private VRButton button;
            public UnityEngine.UI.Text outputText, alertText;
            private Model.RefType type;
            public event EventHandler TextChanged;
            private Material Basemat
            {
                get
                {
                    return ScartchResourceManager.instance.textBoxNotHighlighted;
                }
            }
            private Material Focusmat
            {
                get
                {
                    return ScartchResourceManager.instance.textBoxHighlighted;
                }
            }
            public UnityEngine.UI.Image alertImage;

            public Model.RefType Type
            {
                get { return type; }
                set { type = value; }
            }

            public void Focus()
            {
                gameObject.GetComponent<Renderer>().material = Focusmat;
            }

            public void LostFocus()
            {
                gameObject.GetComponent<Renderer>().material = Basemat;
            }

            private void Start()
            {
                button = gameObject.GetComponent<VRButton>();
                button.Pressed += AssociateKeyboard;
                LostFocus();
                alertImage.enabled = false;
                alertText.enabled = false;
            }

            private void OnDestroy()
            {
                button.Pressed -= AssociateKeyboard;
            }

            private void AssociateKeyboard(object sender, EventArgs e)
            {
                if (VRKeyboard.Instance.Visible)
                {
                    VRKeyboard.Instance.Focus = this;
                }
            }

            public string Text
            {
                get { return text; }
                set
                {
                    if (value != text)
                        if (Model.RefTypeHelper.Validate(value, type))
                        {
                            text = value;
                            outputText.text = value;
                            if (TextChanged != null)
                                TextChanged(this, EventArgs.Empty);
                        }
                        else
                            AlertIncompatibleType();
                }
            }

            private void AlertIncompatibleType()
            {
                alertText.text = "Please insert a " + Model.RefTypeHelper.Name(type) + " value.";
                StartCoroutine(ShowAlert());
            }

            private IEnumerator ShowAlert()
            {
                alertText.enabled = true;
                alertImage.enabled = true;
                yield return new WaitForSeconds(5);
                alertText.enabled = false;
                alertImage.enabled = false;
            }
        }
    }
}
