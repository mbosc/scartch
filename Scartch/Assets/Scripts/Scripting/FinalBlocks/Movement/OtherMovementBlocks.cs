using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using View;

namespace Scripting
{
    public class AlterPositionBlock : Block
    {
        public AlterPositionBlock(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, BlockViewer viewer, bool sample) : base(owner, optionList, ScriptingType.movement, referenceSlotViewers, viewer, sample)
        {
        }

        public static string description = "Add (  ) to position {x|y|z}";

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override void Execute()
        {
            var axis = GetOption(0).Value;
            var value = ReferenceList[0].FloatEval;
            switch (axis)
            {
                case 0: owner.Position += new Vector3(value, 0, 0); break;
                case 1: owner.Position += new Vector3(0, value, 0); break;
                case 2: owner.Position += new Vector3(value, 0, value); break;
                default: throw new ArgumentException("Bad axis");
            }
            
            Flux.current.CurrentBlock = Next;
        }
    }

    public class AlterRotationBlock : Block
    {
        public AlterRotationBlock(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, BlockViewer viewer, bool sample) : base(owner, optionList, ScriptingType.movement, referenceSlotViewers, viewer, sample)
        {
        }

        public static string description = "Add (  ) to rotation {x|y|z}";

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override void Execute()
        {
            var axis = GetOption(0).Value;
            var value = ReferenceList[0].FloatEval;
            switch (axis)
            {
                case 0: owner.Rotation += new Vector3(value, 0, 0); break;
                case 1: owner.Rotation += new Vector3(0, value, 0); break;
                case 2: owner.Rotation += new Vector3(value, 0, value); break;
                default: throw new ArgumentException("Bad axis");
            }

            Flux.current.CurrentBlock = Next;
        }
    }

    public class SetPositionBlock : Block
    {
        public SetPositionBlock(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, BlockViewer viewer, bool sample) : base(owner, optionList, ScriptingType.movement, referenceSlotViewers, viewer, sample)
        {
        }

        public static string description = "Set position {x|y|z} to (  )";

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override void Execute()
        {
            var axis = GetOption(0).Value;
            var value = ReferenceList[0].FloatEval;
            switch (axis)
            {
                case 0: owner.Position = new Vector3(value, owner.Position.y, owner.Position.z); break;
                case 1: owner.Position = new Vector3(owner.Position.x, value, owner.Position.z); break;
                case 2: owner.Position = new Vector3(owner.Position.x, owner.Position.y, value); break;
                default: throw new ArgumentException("Bad axis");
            }

            Flux.current.CurrentBlock = Next;
        }
    }

    public class SetRotationBlock : Block
    {
        public SetRotationBlock(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, BlockViewer viewer, bool sample) : base(owner, optionList, ScriptingType.movement, referenceSlotViewers, viewer, sample)
        {
        }

        public static string description = "Set rotation {x|y|z} to (  )";

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override void Execute()
        {
            var axis = GetOption(0).Value;
            var value = ReferenceList[0].FloatEval;
            switch (axis)
            {
                case 0: owner.Rotation = new Vector3(value, owner.Rotation.y, owner.Rotation.z); break;
                case 1: owner.Rotation = new Vector3(owner.Rotation.x, value, owner.Rotation.z); break;
                case 2: owner.Rotation = new Vector3(owner.Rotation.x, owner.Rotation.y, value); break;
                default: throw new ArgumentException("Bad axis");
            }

            Flux.current.CurrentBlock = Next;
        }
    }

    public class XPositionReference : ExpressionReference
    {

        public static string description = "Position x";

        public XPositionReference(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer, bool sample) : base(owner, optionList, ScriptingType.movement, referenceSlotViewers, viewer, RefType.numberType, sample)
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
            float val = owner.Position.x;
            return val.ToStdStr();
        }
    }
    public class YPositionReference : ExpressionReference
    {

        public static string description = "Position y";

        public YPositionReference(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer, bool sample) : base(owner, optionList, ScriptingType.movement, referenceSlotViewers, viewer, RefType.numberType, sample)
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
            float val = owner.Position.y;
            return val.ToStdStr();
        }
    }
    public class ZPositionReference : ExpressionReference
    {

        public static string description = "Position z";

        public ZPositionReference(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer, bool sample) : base(owner, optionList, ScriptingType.movement, referenceSlotViewers, viewer, RefType.numberType, sample)
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
            float val = owner.Position.z;
            return val.ToStdStr();
        }
    }

    public class XRotationReference : ExpressionReference
    {

        public static string description = "Rotation x";

        public XRotationReference(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer, bool sample) : base(owner, optionList, ScriptingType.movement, referenceSlotViewers, viewer, RefType.numberType, sample)
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
            float val = owner.Rotation.x;
            return val.ToStdStr();
        }
    }
    public class YRotationReference : ExpressionReference
    {

        public static string description = "Rotation y";

        public YRotationReference(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer, bool sample) : base(owner, optionList, ScriptingType.movement, referenceSlotViewers, viewer, RefType.numberType, sample)
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
            float val = owner.Rotation.y;
            return val.ToStdStr();
        }
    }
    public class ZRotationReference : ExpressionReference
    {

        public static string description = "Rotation z";

        public ZRotationReference(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer, bool sample) : base(owner, optionList, ScriptingType.movement, referenceSlotViewers, viewer, RefType.numberType, sample)
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
            float val = owner.Rotation.z;
            return val.ToStdStr();
        }
    }


}
