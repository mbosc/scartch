using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using View;

namespace Scripting
{
    public class SetBoolVarBlock : Block
    {
        public SetBoolVarBlock(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, BlockViewer viewer, bool sample) : base(owner, optionList, ScriptingType.variable, referenceSlotViewers, viewer, sample)
        {
        }

        public static string description = "Set <  > to <  >";

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override void Execute()
        {
            try
            {
                (ReferenceList[0] as VariableReference).Variable.Value = ReferenceList[1].StringEval;
            }
            catch (NullReferenceException)
            {
                ScartchResourceManager.instance.lastRayCaster.Alert("ERROR: the first argument of " + description + " must be a BOOLEAN VARIABLE REFERENCE\nExecution will halt");
                ExecutionController.Instance.Stop();
            }
            Flux.current.CurrentBlock = Next;
        }
    }
    public class SetNumberVarBlock : Block
    {
        public SetNumberVarBlock(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, BlockViewer viewer, bool sample) : base(owner, optionList, ScriptingType.variable, referenceSlotViewers, viewer, sample)
        {
        }

        public static string description = "Set (  ) to (  )";

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override void Execute()
        {
            try
            {
                (ReferenceList[0] as VariableReference).Variable.Value = ReferenceList[1].FloatEval.ToStdStr();
                Debug.Log("Variable " + (ReferenceList[0] as VariableReference).Variable.Name + " set to " + (ReferenceList[0] as VariableReference).Variable.Value);
            }
            catch (NullReferenceException)
            {
                ScartchResourceManager.instance.lastRayCaster.Alert("ERROR: the first argument of " + description + " must be a NUMBER VARIABLE REFERENCE\nExecution will halt");
                ExecutionController.Instance.Stop();
            }
            Flux.current.CurrentBlock = Next;
        }
    }
    public class SetStringVarBlock : Block
    {
        public SetStringVarBlock(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, BlockViewer viewer, bool sample) : base(owner, optionList, ScriptingType.variable, referenceSlotViewers, viewer, sample)
        {
        }

        public static string description = "Set [  ] to [  ]";

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override void Execute()
        {
            try
            {
                (ReferenceList[0] as VariableReference).Variable.Value = ReferenceList[1].StringEval;
            }
            catch (NullReferenceException)
            {
                ScartchResourceManager.instance.lastRayCaster.Alert("ERROR: the first argument of " + description + " must be a STIRNG VARIABLE REFERENCE\nExecution will halt");
                ExecutionController.Instance.Stop();
            }
            Flux.current.CurrentBlock = Next;
        }
    }
    public class IncreaseNumberVarBlock : Block
    {
        public IncreaseNumberVarBlock(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, BlockViewer viewer, bool sample) : base(owner, optionList, ScriptingType.variable, referenceSlotViewers, viewer, sample)
        {
        }

        public static string description = "Increase (  ) by (  )";

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override void Execute()
        {
            try
            {
                (ReferenceList[0] as VariableReference).Variable.Value = (ReferenceList[0].FloatEval + ReferenceList[1].FloatEval).ToStdStr();
                Debug.Log("Variable " + (ReferenceList[0] as VariableReference).Variable.Name + " set to " + (ReferenceList[0] as VariableReference).Variable.Value);
            }
            catch (NullReferenceException)
            {
                ScartchResourceManager.instance.lastRayCaster.Alert("ERROR: the first argument of " + description + " must be a NUMBER VARIABLE REFERENCE\nExecution will halt");
                ExecutionController.Instance.Stop();
            }
            Flux.current.CurrentBlock = Next;
        }
    }
}
