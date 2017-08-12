using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripting;
using Model;
using View;

namespace Controller
{
    public class VariableController 
    {
        private Variable variable;
        private VariableEntry entry;

        public static VariableController AddVariable(VariableEntry entr, Actor owner)
        {
            var var = new Variable()
            {
                Owner = owner,
                Name = "VAR",
                Type = RefType.stringType,
                Value = RefTypeHelper.Default(RefType.stringType)
            };
            if (owner != null)
                owner.AddVariable(var);
            entr.Init(var.Name, var.Value, var.Type);
            var controller = new VariableController(var, entr);
            return controller;
        }

        private VariableController(Variable var, VariableEntry ent)
        {
            this.variable = var;
            this.entry = ent;

            ent.NameChanged += OnEntryNameChanged;
            ent.ValueChanged += OnEntryValueChanged;
            ent.TypeChanged += OnEntryTypeChanged;
        }

        public void SetName(string name)
        {
            variable.Name = name;
        }
        
        public void SetValue(string value)
        {
            variable.Value = value;
        }

        public void SetType(RefType type)
        {
            variable.Type = type;
        }

        public void Remove()
        {
            variable.Destroy();
        }

        private void OnEntryTypeChanged(RefType obj)
        {
            SetType(obj);
        }

        private void OnEntryValueChanged(string obj)
        {
            SetValue(obj);
        }

        private void OnEntryNameChanged(string obj)
        {
            SetName(obj);
        }
    }
}
