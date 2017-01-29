using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;

namespace view{
public class NumberReferenceWrapper : ReferenceWrapper {

		public NumberReference reference;
	public int valore {
		get {
				return reference.Evaluate ();
		}
	}
	public override string valoreStringa {
			get {
				return reference.EvaluateAsString ();
			}
		}

	}
}
