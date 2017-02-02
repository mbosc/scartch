using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;

namespace view{
public class NumberReferenceWrapper : ReferenceWrapper {

		public new NumberReference reference
        {
            get { return base.reference as NumberReference; }
            set { base.reference = value;
                base.testo = value.EvaluateAsString(); extend();
            }
        }
	public float valore {
		get {
				return reference.Evaluate ();
		}
	}
	public  string valoreStringa {
			get {
				return reference.EvaluateAsString ();
			}
		}

	}
}
