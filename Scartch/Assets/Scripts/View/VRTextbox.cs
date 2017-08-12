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
            public Renderer bodyRenderer;
            public UnityEngine.UI.Text outputText;
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

            public Model.RefType Type
            {
                get { return type; }
                set { type = value; }
            }

            public void Focus()
            {
                bodyRenderer.material = Focusmat;
            }

            public void LostFocus()
            {
                bodyRenderer.material = Basemat;
            }

            private bool inited = false;

            private void Start()
            {
                button = gameObject.GetComponent<VRButton>();
                button.Pressed += AssociateKeyboard;
                LostFocus();
                inited = true;
            }

            private void OnDestroy()
            {
                if (inited)
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
                ScartchResourceManager.instance.lastRayCaster.Alert("Please insert a " + Model.RefTypeHelper.Name(type) + " value.");
            }
        }
    }
}
