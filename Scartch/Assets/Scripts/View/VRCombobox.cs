using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    namespace Resources
    {
        public class VRCombobox : MonoBehaviour
        {
            public VRComboboxVoice menuButton;
            private List<VRComboboxVoice> optionButtons;
            public List<string> options;
            private int currentLength;
            public int minLength = 2;
            private int selected;
            private bool listVisible;

            private bool ListVisible
            {
                get { return listVisible; }
                set
                {
                    optionButtons.ForEach(x => x.gameObject.SetActive(value));
                    listVisible = value;
                }
            }
            
            public void Init()
            {
                currentLength = minLength;
                optionButtons = new List<VRComboboxVoice>();
                options.ForEach(x =>
                {
                    var voice = GameObject.Instantiate(ScartchResourceManager.instance.comboboxVoice);
                    optionButtons.Add(voice);
                    voice.Text = x;
                    voice.transform.SetParent(this.transform, false);
                });
                optionButtons.ForEach(x => { if (x.Text.Length > currentLength) currentLength = x.Text.Length; });
                currentLength += 2;
                menuButton.Length = currentLength;
                int offset = 1;
                optionButtons.ForEach(x => { x.Length = currentLength; x.transform.localPosition = new Vector3(0, -offset++ * 4, -0.4f); });
                menuButton.Pressed += OnMenuPressed;
                optionButtons.ForEach(x => x.Pressed += OnOptionPressed);
                menuButton.Text = "  " + options[selected];
                ListVisible = false;
            }

            private void OnDestroy()
            {
                menuButton.Pressed -= OnMenuPressed;
                optionButtons.ForEach(x => x.Pressed -= OnOptionPressed);
            }

            private void OnOptionPressed(object sender, System.EventArgs e)
            {
                Selected = optionButtons.IndexOf(sender as VRComboboxVoice);
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
                    menuButton.Text = "  " + options[selected];
                }
            }
            public event System.Action<int> SelectionChanged;


        }
    }
}
