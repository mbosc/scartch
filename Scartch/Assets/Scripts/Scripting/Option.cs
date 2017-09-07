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

        public Option(List<string> valueSet, OptionViewer viewer)
        {
            if (valueSet.Count < 1)
                throw new ArgumentException("Too few options");
            this.valueSet = valueSet;
            this.viewer = viewer;

            viewer.ValueSelected += OnViewerValueSelected;
        }

        private void OnViewerValueSelected(int obj)
        {
            Value = obj;
        }

        public void Destroy()
        {
            viewer.ValueSelected -= OnViewerValueSelected;
        }
    }
}
