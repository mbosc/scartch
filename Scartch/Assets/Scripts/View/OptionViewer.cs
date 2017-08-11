﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    public class OptionViewer
    {
        private Resources.VRCombobox combo;

        public OptionViewer(Resources.VRCombobox combo, List<string> options)
        {
            this.combo = combo;
            combo.options = options;
            combo.SelectionChanged += OnComboSelectionChanged;
        }

        public event System.Action<int> ValueSelected;

        private void OnComboSelectionChanged(int val)
        {
            if (ValueSelected != null)
                ValueSelected(val);
        }
    }
}
