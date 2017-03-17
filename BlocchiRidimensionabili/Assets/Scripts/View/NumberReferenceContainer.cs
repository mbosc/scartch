using model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace view
{
    public class NumberReferenceContainer : ReferenceContainer
    {

        private new NumberVariable innerVariable {
            get { return base.innerVariable as NumberVariable; }
            set { base.innerVariable = value; }
        }
        public override void SelectA()
        {
            if (innerVariable == null && variabile == null)
            {
                this.innerVariable = new NumberVariable("temp");

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
                AttachTo(ResourceManager.Instance.numpad);
            }
            if (innerVariable != null)
            {
                AttachTo(ResourceManager.Instance.numpad);
            }
        }

        private void AttachTo(Numpad numpad)
        {
            numpad.DetachAll();
            numpad.OutputChanged += changeImmediateValue;
            numpad.Confirmed += DetachTo;
            numpad.gameObject.SetActive(true);
        }

        private void DetachTo(object numpad)
        {
            var np = numpad as Numpad;
            np.OutputChanged -= changeImmediateValue;
            np.Confirmed -= DetachTo;
        }



        private void changeImmediateValue(float val)
        {
            innerVariable.Value = val;
            ImmediateText.text = val.ToString();
            lunghezza = Mathf.Max(ImmediateText.text.Length, lunghezzaOriginale);
            extend();
        }
    }
}
