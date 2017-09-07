using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    public class VariableMonitorWindow : Resources.VRWindow
    {
        public UnityEngine.UI.Text vName, type, scope, value;

        private Model.Variable variable;

        public Model.Variable Variable
        {
            get { return variable; }
            set {
                if (variable != null)
                {
                    // Unsubscribe
                    variable.NameChanged -= OnVariableNameChanged;
                    variable.TypeChanged -= OnVariableTypeChanged;
                    variable.ValueChanged -= OnVariableValueChanged;
                    variable.Destroyed -= OnVariableDestroyed;
                    variable.Owner.NameChanged -= OnOwnerNameChanged;
                }
                variable = value;
                if (variable != null)
                {
                    // Subscribe
                    variable.NameChanged += OnVariableNameChanged;
                    variable.TypeChanged += OnVariableTypeChanged;
                    variable.ValueChanged += OnVariableValueChanged;
                    variable.Destroyed += OnVariableDestroyed;
                    variable.Owner.NameChanged += OnOwnerNameChanged;

                    this.value.text = variable.Value;
                    scope.text = "Scope: <color=red>" + (variable.Owner == null ? "global</color>" : ("local</color> to <color=green>" + variable.Owner.Name + "</color>"));
                    type.text = "Type: <color=red>" + Model.RefTypeHelper.Name(variable.Type) + "</color>";
                    vName.text = variable.Name;
                }
            }
        }

        private void OnOwnerNameChanged(string obj)
        {
            scope.text = "Scope: <color=red>" + (variable.Owner == null ? "global</color>" : ("local</color> to <color=green>" + variable.Owner.Name + "</color>"));
        }

        private void OnVariableDestroyed()
        {
            Close();
            Destroy(this.gameObject);
        }

        private void OnVariableValueChanged(string obj)
        {
            value.text = obj;
        }

        private void OnVariableTypeChanged(Model.RefType obj)
        {
            type.text = "Type: <color=red>" + Model.RefTypeHelper.Name(obj) +"</color>";
        }

        private void OnVariableNameChanged(string obj)
        {
            vName.text = obj;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (variable != null)
            {
                // Unsubscribe
                variable.NameChanged -= OnVariableNameChanged;
                variable.TypeChanged -= OnVariableTypeChanged;
                variable.ValueChanged -= OnVariableValueChanged;
                variable.Destroyed -= OnVariableDestroyed;
                variable.Owner.NameChanged -= OnOwnerNameChanged;
            }
        }

        public override void Close()
        {
            base.Close();
            Destroy(this.gameObject);
        }
    }
}
