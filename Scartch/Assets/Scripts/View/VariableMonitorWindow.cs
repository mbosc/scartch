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
                    variable.NameChanged -= Variable_NameChanged;
                    variable.TypeChanged -= Variable_TypeChanged;
                    variable.ValueChanged -= Variable_ValueChanged;
                    variable.Destroyed -= Variable_Destroyed;
                    variable.Owner.NameChanged -= Owner_NameChanged;
                }
                variable = value;
                if (variable != null)
                {
                    // Subscribe
                    variable.NameChanged += Variable_NameChanged;
                    variable.TypeChanged += Variable_TypeChanged;
                    variable.ValueChanged += Variable_ValueChanged;
                    variable.Destroyed += Variable_Destroyed;
                    variable.Owner.NameChanged += Owner_NameChanged;

                    this.value.text = variable.Value;
                    scope.text = "Scope: <color=red>" + (variable.Owner == null ? "global</color>" : ("local</color> to <color=green>" + variable.Owner.Name + "</color>"));
                    type.text = "Type: <color=red>" + Model.RefTypeHelper.Name(variable.Type) + "</color>";
                    vName.text = variable.Name;
                }
            }
        }

        private void Owner_NameChanged(string obj)
        {
            scope.text = "Scope: <color=red>" + (variable.Owner == null ? "global</color>" : ("local</color> to <color=green>" + variable.Owner.Name + "</color>"));
        }

        private void Variable_Destroyed()
        {
            Close();
            Destroy(this.gameObject);
        }

        private void Variable_ValueChanged(string obj)
        {
            value.text = obj;
        }

        private void Variable_TypeChanged(Model.RefType obj)
        {
            type.text = "Type: <color=red>" + Model.RefTypeHelper.Name(obj) +"</color>";
        }

        private void Variable_NameChanged(string obj)
        {
            vName.text = obj;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (variable != null)
            {
                // Unsubscribe
                variable.NameChanged -= Variable_NameChanged;
                variable.TypeChanged -= Variable_TypeChanged;
                variable.ValueChanged -= Variable_ValueChanged;
                variable.Destroyed -= Variable_Destroyed;
                variable.Owner.NameChanged -= Owner_NameChanged;
            }
        }

        public override void Close()
        {
            base.Close();
            Destroy(this.gameObject);
        }
    }
}
