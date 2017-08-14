using System.Collections;
using System.Collections.Generic;
using System;
using View;

namespace Scripting
{
    public class Option
    {
        private int _value;

        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }

        private List<string> valueSet;

        public List<string> ValueSet
        {
            get
            {
                return valueSet;
            }
        }

        private OptionViewer viewer;
        public OptionViewer Viewer
        {
            get { return viewer; }
        }
        //TODO manca questa parte di controllo

        public Option(List<string> valueSet, OptionViewer viewer)
        {
            if (valueSet.Count < 2)
                throw new ArgumentException("Too few options");
            this.valueSet = valueSet;
            this.viewer = viewer;

            //TODO
            // subscribe a eventi
        }

    }
}
