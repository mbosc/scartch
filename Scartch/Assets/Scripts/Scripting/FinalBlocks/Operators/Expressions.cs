using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using View;

namespace Scripting
{
    public class SumExpression : ExpressionReference
    {

        public static string description = "(  ) + (  )";

        public SumExpression(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer, bool sample) : base(owner, optionList, ScriptingType.operators, referenceSlotViewers, viewer, RefType.numberType, sample)
        {
        }

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override string Evaluate()
        {
            float val = ReferenceList[0].FloatEval + ReferenceList[1].FloatEval;
            return val.ToStdStr();
        }
    }

    public class MinusExpression : ExpressionReference
    {

        public static string description = "(  ) - (  )";

        public MinusExpression(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer, bool sample) : base(owner, optionList, ScriptingType.operators, referenceSlotViewers, viewer, RefType.numberType, sample)
        {
        }

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override string Evaluate()
        {
            float val = ReferenceList[0].FloatEval - ReferenceList[1].FloatEval;
            return val.ToStdStr();
        }
    }

    public class TimesExpression : ExpressionReference
    {

        public static string description = "(  ) × (  )";

        public TimesExpression(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer, bool sample) : base(owner, optionList, ScriptingType.operators, referenceSlotViewers, viewer, RefType.numberType, sample)
        {
        }

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override string Evaluate()
        {
            float val = ReferenceList[0].FloatEval * ReferenceList[1].FloatEval;
            return val.ToStdStr();
        }
    }

    public class DivExpression : ExpressionReference
    {

        public static string description = "(  ) ÷ (  )";

        public DivExpression(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer, bool sample) : base(owner, optionList, ScriptingType.operators, referenceSlotViewers, viewer, RefType.numberType, sample)
        {
        }

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override string Evaluate()
        {
            float val = ReferenceList[0].FloatEval / ReferenceList[1].FloatEval;
            return val.ToStdStr();
        }
    }

    public class GTExpression : ExpressionReference
    {

        public static string description = "(  ) > (  )";

        public GTExpression(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer, bool sample) : base(owner, optionList, ScriptingType.operators, referenceSlotViewers, viewer, RefType.boolType, sample)
        {
        }

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override string Evaluate()
        {
            bool val = ReferenceList[0].FloatEval > ReferenceList[1].FloatEval;
            return val.ToString().ToUpper();
        }
    }

    public class LTExpression : ExpressionReference
    {

        public static string description = "(  ) < (  )";

        public LTExpression(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer, bool sample) : base(owner, optionList, ScriptingType.operators, referenceSlotViewers, viewer, RefType.boolType, sample)
        {
        }

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override string Evaluate()
        {
            bool val = ReferenceList[0].FloatEval < ReferenceList[1].FloatEval;
            return val.ToString().ToUpper();
        }
    }

    public class GEExpression : ExpressionReference
    {

        public static string description = "(  ) ≥ (  )";

        public GEExpression(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer, bool sample) : base(owner, optionList, ScriptingType.operators, referenceSlotViewers, viewer, RefType.boolType, sample)
        {
        }

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override string Evaluate()
        {
            bool val = ReferenceList[0].FloatEval >= ReferenceList[1].FloatEval;
            return val.ToString().ToUpper();
        }
    }

    public class LEExpression : ExpressionReference
    {

        public static string description = "(  ) ≤ (  )";

        public LEExpression(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer, bool sample) : base(owner, optionList, ScriptingType.operators, referenceSlotViewers, viewer, RefType.boolType, sample)
        {
        }

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override string Evaluate()
        {
            bool val = ReferenceList[0].FloatEval <= ReferenceList[1].FloatEval;
            return val.ToString().ToUpper();
        }
    }

    public class EQExpression : ExpressionReference
    {

        public static string description = "(  ) = (  )";

        public EQExpression(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer, bool sample) : base(owner, optionList, ScriptingType.operators, referenceSlotViewers, viewer, RefType.boolType, sample)
        {
        }

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override string Evaluate()
        {
            bool val = ReferenceList[0].FloatEval == ReferenceList[1].FloatEval;
            return val.ToString().ToUpper();
        }
    }

    public class ANDExpression : ExpressionReference
    {

        public static string description = "<  > AND <  >";

        public ANDExpression(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer, bool sample) : base(owner, optionList, ScriptingType.operators, referenceSlotViewers, viewer, RefType.boolType, sample)
        {
        }

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override string Evaluate()
        {
            bool val = ReferenceList[0].BoolEval && ReferenceList[1].BoolEval;
            return val.ToString().ToUpper();
        }
    }

    public class ORExpression : ExpressionReference
    {

        public static string description = "<  > OR <  >";

        public ORExpression(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer, bool sample) : base(owner, optionList, ScriptingType.operators, referenceSlotViewers, viewer, RefType.boolType, sample)
        {
        }

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override string Evaluate()
        {
            bool val = ReferenceList[0].BoolEval || ReferenceList[1].BoolEval;
            return val.ToString().ToUpper();
        }
    }

    public class NOTExpression : ExpressionReference
    {

        public static string description = "NOT <  >";

        public NOTExpression(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer, bool sample) : base(owner, optionList, ScriptingType.operators, referenceSlotViewers, viewer, RefType.boolType, sample)
        {
        }

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override string Evaluate()
        {
            bool val = !ReferenceList[0].BoolEval;
            return val.ToString().ToUpper();
        }
    }

    public class RandomNumExpression : ExpressionReference
    {

        public static string description = "Random number between (  ) and (  )";

        public RandomNumExpression(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer, bool sample) : base(owner, optionList, ScriptingType.operators, referenceSlotViewers, viewer, RefType.numberType, sample)
        {
        }

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override string Evaluate()
        {
            float num1 = ReferenceList[0].FloatEval;
            float num2 = ReferenceList[1].FloatEval;
            if (num2 < num1)
            {
                ScartchResourceManager.instance.lastRayCaster.Alert("Random instruction ignored: invalid bounds!");
                return 0f.ToStdStr();
            }
            else
            {
                float val = UnityEngine.Random.Range(num1, num2);
                return val.ToStdStr();
            }
        }
    }

    public class STREQExpression : ExpressionReference
    {

        public static string description = "[  ] = [  ]";

        public STREQExpression(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer, bool sample) : base(owner, optionList, ScriptingType.operators, referenceSlotViewers, viewer, RefType.boolType, sample)
        {
        }

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override string Evaluate()
        {
            bool val = ReferenceList[0].StringEval.Equals(ReferenceList[1].StringEval);
            return val.ToString().ToUpper();
        }
    }

    public class STRCATExpression : ExpressionReference
    {

        public static string description = "Union of [  ] and [  ]";

        public STRCATExpression(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer, bool sample) : base(owner, optionList, ScriptingType.operators, referenceSlotViewers, viewer, RefType.stringType, sample)
        {
        }

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override string Evaluate()
        {            
            return ReferenceList[0].StringEval + ReferenceList[1].StringEval;
        }
    }

    public class STRLENExpression : ExpressionReference
    {

        public static string description = "Length of [  ]";

        public STRLENExpression(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer, bool sample) : base(owner, optionList, ScriptingType.operators, referenceSlotViewers, viewer, RefType.numberType, sample)
        {
        }

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override string Evaluate()
        {
            return ReferenceList[0].StringEval.Length.ToString();
        }
    }

    public class MATHFOPSExpression : ExpressionReference
    {

        public static string description = "{abs|sqrt|sin|cos|tan|asin|acos|atan|ln|log|e^|10^} of ( )";

        public MATHFOPSExpression(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer, bool sample) : base(owner, optionList, ScriptingType.operators, referenceSlotViewers, viewer, RefType.numberType, sample)
        {
        }

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override string Evaluate()
        {
            int sel = GetOption(0).Value;
            float num = ReferenceList[0].FloatEval;
            switch (sel)
            {
                case 0: return Mathf.Abs(num).ToStdStr();
                case 1: return Mathf.Sqrt(num).ToStdStr();
                case 2: return Mathf.Sin(num).ToStdStr();
                case 3: return Mathf.Cos(num).ToStdStr();
                case 4: return Mathf.Tan(num).ToStdStr();
                case 5: return Mathf.Asin(num).ToStdStr();
                case 6: return Mathf.Acos(num).ToStdStr();
                case 7: return Mathf.Atan(num).ToStdStr();
                case 8: return Mathf.Log(num).ToStdStr();
                case 9: return Mathf.Log10(num).ToStdStr();
                case 10: return Mathf.Exp(num).ToStdStr();
                case 11: return Mathf.Pow(10, num).ToStdStr();
                default: return 0f.ToStdStr();
            }
        }
    }

    public class ModExpression : ExpressionReference
    {

        public static string description = "(  ) mod (  )";

        public ModExpression(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer, bool sample) : base(owner, optionList, ScriptingType.operators, referenceSlotViewers, viewer, RefType.numberType, sample)
        {
        }

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override string Evaluate()
        {
            float val = ReferenceList[0].FloatEval % ReferenceList[1].FloatEval;
            return val.ToStdStr();
        }
    }

    public class RoundExpression : ExpressionReference
    {

        public static string description = "Round (  )";

        public RoundExpression(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer, bool sample) : base(owner, optionList, ScriptingType.operators, referenceSlotViewers, viewer, RefType.numberType, sample)
        {
        }

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override string Evaluate()
        {
            float val = Mathf.Round(ReferenceList[0].FloatEval);
            return val.ToStdStr();
        }
    }
}
