using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
using View.Resources;

namespace View
{
    public class VariableEntry
    {
        private string name, value;
        private RefType type;
        private VRTextbox nameBox;
        private VRTextbox valueBox;
        private VRCombobox typeBox;
        public event System.Action<string> NameChanged, ValueChanged;
        public event System.Action<RefType> TypeChanged;

        public VariableEntry(string name, string value, RefType type, VRTextbox nameBox, VRTextbox valueBox, VRCombobox typeBox)
        {
            this.name = name;
            this.type = type;
            this.value = value;
            this.nameBox = nameBox;
            nameBox.Text = name;
            this.valueBox = valueBox;
            valueBox.Text = value;
            this.typeBox = typeBox;
            typeBox.Selected = (int)type;

            nameBox.TextChanged += OnNameChanged;
            valueBox.TextChanged += OnValChanged;
            typeBox.SelectionChanged += OnTypeChanged;
        }

        private void OnTypeChanged(int obj)
        {
            type = (RefType)obj;
            if (TypeChanged != null)
                TypeChanged(type);
        }

        private void OnValChanged(object sender, System.EventArgs e)
        {
            value = (sender as VRTextbox).Text;
            if (ValueChanged != null)
                ValueChanged(value);
        }

        private void OnNameChanged(object sender, System.EventArgs e)
        {
            name = (sender as VRTextbox).Text;
            if (NameChanged != null)
                NameChanged(name);
        }

        public void Destroy()
        {
            nameBox.TextChanged -= OnNameChanged;
            valueBox.TextChanged -= OnValChanged;
            typeBox.SelectionChanged -= OnTypeChanged;
        }
    }
}
