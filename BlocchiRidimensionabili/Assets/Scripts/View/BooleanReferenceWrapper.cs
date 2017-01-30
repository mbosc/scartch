using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using model;

namespace view
{
	public class BooleanReferenceWrapper : ReferenceWrapper
	{

		public BooleanReference reference;

		public bool valore {
			get {
				return reference.Evaluate ();
			}
		}

		public string valoreStringa {
			get {
				return reference.EvaluateAsString ();
			}
		}
	}

}
