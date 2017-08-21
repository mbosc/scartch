using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    public class VariableWindow : Resources.VRWindow
    {
        private List<VariableEntry> variableEntries;
        public Resources.VRButton addVarButton;
        public GameObject background;
        public UnityEngine.UI.Text title;

        private void UpdateLayout()
        {
            // Rescale background
            background.transform.localPosition = new Vector3(background.transform.localPosition.x,
                0.188f - 0.155f * variableEntries.Count, background.transform.localPosition.z);
            background.transform.localScale = new Vector3(background.transform.localScale.x,
                0.623f + 0.308f * variableEntries.Count, background.transform.localScale.z);

            // Organise Rows
            int offset = 0;
            var mgr = ScartchResourceManager.instance;
            variableEntries.ForEach(x => x.transform.localPosition = mgr.varWindowEntryPoint - new Vector3(0, mgr.varWindowStep * offset++, 0));
            addVarButton.transform.localPosition = mgr.varWindowButtonPoint - new Vector3(0, mgr.varWindowStep * offset++, 0);
        }

        public event Action<VariableEntry> VariableAdded;
        public event System.Action<int> VariableRemoved;

        public void AddVariable()
        {
            var entr = GameObject.Instantiate(ScartchResourceManager.instance.entryPrototype);
            entr.transform.SetParent(this.transform, false);
            entr.transform.localScale = ScartchResourceManager.instance.varWindowEntryScale;
            entr.DeletePressed += OnVarDeletePressed;
            entr.MonitorPressed += OnVarMonitorPressed;
            variableEntries.Add(entr);
            UpdateLayout();

            if (VariableAdded != null)
                VariableAdded(entr);
        }

        private void OnVarMonitorPressed(Model.Variable obj)
        {
            var window = GameObject.Instantiate(ScartchResourceManager.instance.variableMonitorWindow).GetComponent<VariableMonitorWindow>();
            window.Variable = obj;
            window.Open();
        }
        

        private void OnVarDeletePressed(object sender, EventArgs e)
        {
            RemoveVariable(variableEntries.IndexOf(sender as VariableEntry));
        }

        public void RemoveVariable(int num)
        {
            try
            {
                if (VariableRemoved != null)
                    VariableRemoved(num);

                var element = variableEntries[num];
                element.DeletePressed -= OnVarDeletePressed;
                element.MonitorPressed -= OnVarMonitorPressed;
                variableEntries.Remove(element);
                Destroy(element.gameObject);
                UpdateLayout();
            }
            catch (Model.VariableAlterationException e)
            {
                ScartchResourceManager.instance.lastRayCaster.Alert(e.Message);
            }
        }

        public void Init(Model.Actor owner)
        {
            if (owner == null)
                title.text = "Global Variables";
            else
                title.text = "Local Variables for " + owner.Name;
            variableEntries = new List<VariableEntry>();
            UpdateLayout();
        }

        protected override void Start()
        {
            base.Start();
            addVarButton.Pressed += OnAddButtonPressed;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            addVarButton.Pressed -= OnAddButtonPressed;
        }

        private void OnAddButtonPressed(object sender, EventArgs e)
        {
            AddVariable();
        }
    }
}
