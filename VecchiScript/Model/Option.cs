using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace model{
public class Option {
		public int chosenValue;
        private IList<System.Object> possibleValues;
        public IList<System.Object> PossibleValues
        {
            get
            {
                if (updateValues != null)
                    possibleValues = updateValues();
                return possibleValues;
            }
        }
        private System.Func<IList<System.Object>> updateValues;

		public Option(IList<System.Object> options){
            updateValues = null;
            possibleValues = options;
			chosenValue = 0;
		}
		public Option() : this(new List<System.Object>()){ }

        public Option(System.Func<IList<System.Object>> updateValues) : this()
        {
            this.updateValues = updateValues;
            chosenValue = 0;
        }
    }
}
