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
            public VRButton button;
            private Model.RefType type;
            public event EventHandler TextChanged;

            public Model.RefType Type
            {
                get { return type; }
                set { type = value; }
            }

            private void Start()
            {
                button.Pressed += AssociateKeyboard;
            }

            private void OnDestroy()
            {
                button.Pressed -= AssociateKeyboard;
            }

            private void AssociateKeyboard(object sender, EventArgs e)
            {
                VRKeyboard.Instance.Focus = this;
            }

            public string Text
            {
                get { return text; }
                set
                {
                    if (value != text && Model.RefTypeHelper.Validate(value, type))
                    {
                        text = value;
                        if (TextChanged != null)
                            TextChanged(this, EventArgs.Empty);
                        //TODO
                        // eventualmente dare feedback di errore?
                    }
                }
            }
        }
    }
}
