﻿using System.Collections;
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
            set
            {
                variable = value;
                if (!sample)
                {
                    variable.RefCount++;
                    variable.Destroyed += OnVariableDestroyed;
                    viewer.RefType = variable.Type;
                }
            }
        }

        public override string Description
        {
            get
            {
                return variable.Name;
            }
        }

        private bool sample;
        public VariableReference(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer, bool sample) : base(owner, optionList, ScriptingType.variable, referenceSlotViewers, viewer, sample)
        {
            this.sample = sample;
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
            if (!sample)
            {
                variable.RefCount--;
                variable.Destroyed -= OnVariableDestroyed;
            }
        }

        public override RefType GetRefType()
        {
            return Variable.Type;
        }

        public override string Evaluate()
        {
            return Variable.Value;
        }
        
    }
}
