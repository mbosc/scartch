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
        private Model.Variable variable;
        public VRTextbox nameBox;
        public VRTextbox valueBox;
        public VRCombobox typeBox;
        public VRButton monitorButton, deleteButton;
        public event System.Action MonitorPressed;
        public event System.EventHandler DeletePressed;
        public event System.Action<string> NameChanged, ValueChanged;
        public event System.Action<RefType> TypeChanged;

        public void Init(Variable var)
        {
            this.variable = var;
            nameBox.Type = RefType.stringType;
            valueBox.Type = variable.Type;
            nameBox.Text = variable.Name;
            valueBox.Text = variable.Value;
            typeBox.options = new List<string>();
            foreach (var t in Enum.GetValues(typeof(RefType)))
                typeBox.options.Add(RefTypeHelper.Name((RefType)t));
            typeBox.Selected = (int)variable.Type;
            typeBox.Init();

            nameBox.TextChanged += OnNameChanged;
            valueBox.TextChanged += OnValChanged;
            typeBox.SelectionChanged += OnTypeChanged;
            monitorButton.Pressed += OnMonitorPressed;
            deleteButton.Pressed += OnDeletePressed;

            variable.NameChanged += Variable_NameChanged;
            variable.TypeChanged += Variable_TypeChanged;
            variable.ValueChanged += Variable_ValueChanged;

            inited = true;
        }

        private void Variable_ValueChanged(string obj)
        {
            if (valueBox.Text != obj)
                valueBox.Text = obj;
        }

        private void Variable_TypeChanged(RefType obj)
        {

            valueBox.Type = obj;
            valueBox.Text = RefTypeHelper.Default(obj);

        }

        private void Variable_NameChanged(string obj)
        {
            if (nameBox.Text != obj)
                nameBox.Text = obj;
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
            if (variable.Type == (RefType)obj)
                return;
            if (TypeChanged != null)
                TypeChanged((RefType)obj);
        }

        private void OnValChanged(object sender, System.EventArgs e)
        {
            if (ValueChanged != null)
                ValueChanged((sender as VRTextbox).Text);
        }

        private void OnNameChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (NameChanged != null)
                    NameChanged((sender as VRTextbox).Text);
            }
            catch (VariableAlterationException e2)
            {
                ScartchResourceManager.instance.lastRayCaster.Alert(e2.Message);
            }
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
                variable.NameChanged -= Variable_NameChanged;
                variable.TypeChanged -= Variable_TypeChanged;
                variable.ValueChanged -= Variable_ValueChanged;

            }
        }
    }
}
