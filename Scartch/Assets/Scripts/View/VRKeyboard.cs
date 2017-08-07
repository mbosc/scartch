﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace View
{
    namespace Resources
    {
        public class VRKeyboard : MonoBehaviour
        {

            private static VRKeyboard instance;

            public static VRKeyboard Instance
            {
                get
                {
                    return instance;
                }
            }

            private void Start()
            {
                // singleton
                if (instance == null)
                    instance = this;
                else
                    Destroy(this.gameObject);

                // buttons reference initialisation
                List<System.Reflection.FieldInfo> result = (from field in this.GetType().GetFields()
                                                            where field.Name.Length == 2
                                                            where field.Name.StartsWith("b")
                                                            select field).ToList();
                buttons = result.ToDictionary(x => (VRButton)(x.GetValue(x)), x => x.Name.Substring(1));
                buttons.Add(bDOT, ".");

                // buttons event subscription
                buttons.Keys.ToList().ForEach(x => x.Pressed += OnCharButtonPressed);
                bCONFIRM.Pressed += OnConfirmPressed;
                bBACK.Pressed += OnBackPressed;
            }

            private void OnBackPressed(object sender, System.EventArgs e)
            {
                if (text.Length > 0)
                    text = text.Substring(0, text.Length);
            }

            private void OnConfirmPressed(object sender, System.EventArgs e)
            {
                if (focus != null)
                    focus.Text = text;
            }

            private void OnCharButtonPressed(object sender, System.EventArgs e)
            {
                text += buttons[sender as VRButton];
            }

            private string text;

            public string Text
            {
                get { return text; }
                set { text = value; }
            }

            private VRTextbox focus;

            public VRTextbox Focus
            {
                get { return focus; }
                set
                {
                    focus = value;
                    Text = focus.Text;
                }
            }

            // only used in initialisation via Unity
            public VRButton b1, b2, b3, b4, b5, b6, b7, b8, b9, b0,
                            bQ, bW, bE, bR, bT, bY, bU, bI, bO, bP,
                            bA, bS, bD, bF, bG, bH, bJ, bK, bL, bDOT,
                            bZ, bX, bC, bV, bB, bN, bM, bCONFIRM, bBACK;

            // dictionary for quick retrieval of button value
            public Dictionary<VRButton, string> buttons;
        }
    }
}
