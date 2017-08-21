using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace View
{
    namespace Resources
    {
        public class VRKeyboard : VRWindow
        {

            private static VRKeyboard instance;

            public static VRKeyboard Instance
            {
                get
                {
                    return instance;
                }
            }

            protected override void Start()
            {


                base.Start();
                // singleton
                if (instance == null)
                    instance = this;
                else
                    Destroy(this.gameObject);

                base.Start();

                // buttons reference initialisation
                List<System.Reflection.FieldInfo> result = (from field in this.GetType().GetFields()
                                                            where field.Name.Length == 2
                                                            where field.Name.StartsWith("b")
                                                            select field).ToList();
                buttons = result.ToDictionary(x => (VRButton)(x.GetValue(this)), x => x.Name.Substring(1));
                buttons.Add(bDOT, ".");
                buttons.Add(bMIN, "-");

                // buttons event subscription
                buttons.Keys.ToList().ForEach(x => x.Pressed += OnCharButtonPressed);
                bCONFIRM.Pressed += OnConfirmPressed;
                bBACK.Pressed += OnBackPressed;

                //Close when play mode starts
                Controller.EnvironmentController.Instance.InitiatingPlayMode += Close;

                //close at start
                Close();

            }



            private void OnBackPressed(object sender, System.EventArgs e)
            {
                if (Text.Length > 0)
                    Text = Text.Substring(0, Text.Length -1);
            }

            private void OnConfirmPressed(object sender, System.EventArgs e)
            {
                if (focus != null)
                    focus.Text = Text;
            }

            private void OnCharButtonPressed(object sender, System.EventArgs e)
            {
                Text += buttons[sender as VRButton];
            }

            public UnityEngine.UI.Text textBox;

            private string text;

            public string Text
            {
                get { return text; }
                set
                {
                    text = value;
                    textBox.text = text;
                }
            }

            public override void Close()
            {
                base.Close();
                Focus = null;
            }

            private VRTextbox focus;

            public VRTextbox Focus
            {
                get { return focus; }
                set
                {
                    if (focus != null) focus.LostFocus();
                    focus = value;
                    if (focus != null)
                    {
                        focus.Focus();
                        Text = focus.Text;
                    }
                }
            }

            // only used in initialisation via Unity
            public VRButton b1, b2, b3, b4, b5, b6, b7, b8, b9, b0,
                            bQ, bW, bE, bR, bT, bY, bU, bI, bO, bP,
                            bA, bS, bD, bF, bG, bH, bJ, bK, bL, bDOT,
                            bZ, bX, bC, bV, bB, bN, bM, bMIN, bCONFIRM, bBACK;

            // dictionary for quick retrieval of button value
            public Dictionary<VRButton, string> buttons;
        }
    }
}
