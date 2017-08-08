using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    namespace Resources
    {
        public class VRCombobox : MonoBehaviour
        {
            public VRButton menuButton;
            public List<VRButton> optionButtons;
            public List<string> options;
            private int selected;
            private bool listVisible;

            private bool ListVisible
            {
                get { return listVisible; }
                set
                {
                    if (value)
                    {
                        //rendi visibili tutti i sottoelementi
                    }
                    else
                    {
                        //nascondi tutti i sottoelementi
                    }
                    listVisible = value;
                }
            }

            private void Start()
            {
                menuButton.Pressed += OnMenuPressed;
                optionButtons.ForEach(x => x.Pressed += OnOptionPressed);
            }

            private void OnDestroy()
            {
                menuButton.Pressed -= OnMenuPressed;
                optionButtons.ForEach(x => x.Pressed -= OnOptionPressed);
            }

            private void OnOptionPressed(object sender, System.EventArgs e)
            {
                Selected = optionButtons.IndexOf(sender as VRButton);
                ListVisible = false;
            }

            private void OnMenuPressed(object sender, System.EventArgs e)
            {
                ListVisible = !ListVisible;
            }

            public int Selected
            {
                get { return selected; }
                set
                {
                    if (value > options.Count)
                        throw new System.ArgumentException("Value is too high");
                    if (value != selected && SelectionChanged != null)
                        SelectionChanged(value);
                    selected = value;
                }
            }
            public event System.Action<int> SelectionChanged;
        }
    }
}
