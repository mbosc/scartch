using System.Collections;
using System.Collections.Generic;
using System;
using Model;
using View;

namespace Scripting
{
    public class VariableReference : Reference
    {
        private Variable variable;

        public Variable Variable
        {
            get { return variable; }
            set { variable = value; }
        }

        public override string Description
        {
            get
            {
                return variable.Name;
            }
        }

        public VariableReference(Actor owner, List<RefType> referenceTypes, List<Option> optionList, ScriptingType type, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer, Variable var) : base(owner, referenceTypes, optionList, type, referenceSlotViewers, viewer)
        {
            this.variable = var;
            var.RefCount++;
            variable.Destroyed += OnVariableDestroyed;
            variable.NameChanged += OnVariableNameChanged;
            variable.TypeChanged += OnVariableTypeChanged;
        }

        private void OnVariableTypeChanged(RefType obj)
        {
            viewer.RefType = obj;
        }

        private void OnVariableNameChanged(string obj)
        {
            throw new NotImplementedException();
            // TODO inoltrare al refernceviewer il cambiamento di nome
        }

        private void OnVariableDestroyed()
        {
            bool success = true;
            try
            {
                variable.Destroy();
            }
            catch (VariableAlterationException e)
            {
                success = false;
                throw e;
            }
            if (success)
            {
                Destroy();
            }
        }

        public override void Destroy()
        {
            base.Destroy();
            variable.RefCount--;
            variable.Destroyed -= OnVariableDestroyed;
        }

        public override RefType GetRefType()
        {
            return Variable.Type;
        }

        public override string Evaluate()
        {
            return Variable.Value;
        }

        //TODO: gestire il cambio di tipo delle variabili
    }
}
