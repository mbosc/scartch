using model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace view
{
    public class BooleanReferenceContainer : ReferenceContainer
    {

        private new BooleanVariable innerVariable
        {
            get { return base.innerVariable as BooleanVariable; }
            set { base.innerVariable = value; }
        }
        public override void SelectA()
        {
            if (innerVariable == null && variabile == null)
            {
                this.innerVariable = new BooleanVariable("temp");

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
                AttachTo(ResourceManager.Instance.boolpad);
            }
            if (innerVariable != null)
            {
                AttachTo(ResourceManager.Instance.boolpad);
            }
        }

        private void AttachTo(Boolpad pad)
        {
            pad.DetachAll();
            pad.OutputChanged += changeImmediateValue;
            pad.Confirmed += DetachTo;
            pad.gameObject.SetActive(true);
        }

        private void DetachTo(object numpad)
        {
            var np = numpad as Boolpad;
            np.OutputChanged -= changeImmediateValue;
            np.Confirmed -= DetachTo;
        }



        private void changeImmediateValue(bool val)
        {
            innerVariable.Value = val;
            ImmediateText.text = val.ToString();
            lunghezza = Mathf.Max(ImmediateText.text.Length, lunghezzaOriginale);
            extend();
        }
    }
}