using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace model{
public class Option {
		public int chosenValue;
		public IList<System.Object> possibleValues;

		public Option(IList<System.Object> options){
			possibleValues = options;
			chosenValue = 0;
		}
		public Option() : this(new List<System.Object>()){ }
	}
}
