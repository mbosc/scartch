using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
using View.Resources;
using System;
using System.Linq;

namespace View
{
    public class VariableEntry : MonoBehaviour
    {
        private string vname, value;
        private RefType type;
        public VRTextbox nameBox;
        public VRTextbox valueBox;
        public VRCombobox typeBox;
        public VRButton monitorButton, deleteButton;
        public event System.Action MonitorPressed;
        public event System.EventHandler DeletePressed;
        public event System.Action<string> NameChanged, ValueChanged;
        public event System.Action<RefType> TypeChanged;

        public void Init(string name, string value, RefType type)
        {
            nameBox.Type = RefType.stringType;
            valueBox.Type = type;
            this.vname = name;
            this.type = type;
            this.value = value;
            nameBox.Text = name;
            valueBox.Text = value;
            typeBox.options = new List<string>();
            foreach (var t in Enum.GetValues(typeof(RefType)))
                typeBox.options.Add(RefTypeHelper.Name((RefType)t));
            typeBox.Selected = (int)type;
            typeBox.Init();

            nameBox.TextChanged += OnNameChanged;
            valueBox.TextChanged += OnValChanged;
            typeBox.SelectionChanged += OnTypeChanged;
            monitorButton.Pressed += OnMonitorPressed;
            deleteButton.Pressed += OnDeletePressed;
            inited = true;
        }

        private void OnDeletePressed(object sender, EventArgs e)
        {
            if (DeletePressed != null)
                DeletePressed(this, e);
        }

        private void OnMonitorPressed(object sender, EventArgs e)
        {
            if (MonitorPressed != null)
                MonitorPressed();
        }

        private void OnTypeChanged(int obj)
        {
            type = (RefType)obj;
            valueBox.Type = type;
            valueBox.Text = RefTypeHelper.Default(type);
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
            vname = (sender as VRTextbox).Text;
            if (NameChanged != null)
                NameChanged(vname);
        }

        private bool inited = false;

        public void OnDestroy()
        {
            if (inited)
            {
                nameBox.TextChanged -= OnNameChanged;
                valueBox.TextChanged -= OnValChanged;
                typeBox.SelectionChanged -= OnTypeChanged;
                monitorButton.Pressed -= OnMonitorPressed;
                deleteButton.Pressed -= OnDeletePressed;
            }
        }
    }
}
