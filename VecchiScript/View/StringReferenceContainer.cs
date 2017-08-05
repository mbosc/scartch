using model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace view
{
    public class StringReferenceContainer : ReferenceContainer
    {

        private new StringVariable innerVariable
        {
            get { return base.innerVariable as StringVariable; }
            set { base.innerVariable = value; }
        }
        public override void SelectA()
        {
            if (innerVariable == null && variabile == null)
            {
                this.innerVariable = new StringVariable("temp");

                var d = transform.parent.gameObject.GetComponent<BlockWrapper>();
                if (d == null)
                {
                    var z = transform.parent.gameObject.GetComponent<ReferenceWrapper>();

                    z.expression.AddReference(number, innerVariable);
                }
                else
                {
                    d.block.AddReference(number, innerVariable);
                }
                AttachTo(ResourceManager.Instance.keypad);
            }
            if (innerVariable != null)
            {
                AttachTo(ResourceManager.Instance.keypad);
            }
        }

        private void AttachTo(Keyboard kpad)
        {
            kpad.DetachAll();
            kpad.OutputChanged += changeImmediateValue;
            kpad.Confirmed += DetachTo;
            kpad.gameObject.SetActive(true);
        }

        private void DetachTo(object numpad)
        {
            var np = numpad as Keyboard;
            np.OutputChanged -= changeImmediateValue;
            np.Confirmed -= DetachTo;
        }

        private void changeImmediateValue(string val)
        {
            innerVariable.Value = val;
            ImmediateText.text = val.ToString();
            lunghezza = Mathf.Max(ImmediateText.text.Length, lunghezzaOriginale);
            extend();
        }
    }
}