using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;

namespace view{
public class NumberReferenceWrapper : ReferenceWrapper {

		public NumberReference reference;
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
