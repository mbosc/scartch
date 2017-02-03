using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;
namespace view {
    public class StringReferenceWrapper :ReferenceWrapper  {

        public new StringReference reference
        {
            get { return base.reference as StringReference; }
            set { base.reference = value; }
        }

        public string valore
        {
            get
            {
                return reference.Evaluate();
            }
        }

        public string valoreStringa
        {
            get
            {
                return reference.EvaluateAsString();
            }
        }

        protected override bool Compatible(ReferenceContainer bucoCorrente)
        {
            return bucoCorrente.type.Equals("[");
        }
    }
}