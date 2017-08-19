using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using View;
using Controller;

namespace Scripting
{
    public class TimerReference : ExpressionReference
    {

        public static string description = "Timer";

        public TimerReference(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer, bool sample) : base(owner, optionList, ScriptingType.sensor, referenceSlotViewers, viewer, RefType.numberType, sample)
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
            float val = Timer.instance.Time;
            return val.ToStdStr();
        }
    }

    public class ControllerPositionReference : ExpressionReference
    {

        public static string description = "{left|right} controller's position {x|y|z}";

        public ControllerPositionReference(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer, bool sample) : base(owner, optionList, ScriptingType.sensor, referenceSlotViewers, viewer, RefType.numberType, sample)
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
            string controller = GetOption(0).ValueSet[GetOption(0).Value];
            int axis = GetOption(1).Value;
            float val = 0;
            switch (axis)
            {
                case 0: val = InputDeviceController.controllers[controller].Device.Position.x; break;
                case 1: val = InputDeviceController.controllers[controller].Device.Position.y; break;
                case 2: val = InputDeviceController.controllers[controller].Device.Position.z; break;
                default: throw new ArgumentException("Bad axis");
            }
            return val.ToStdStr();
        }
    }

    public class ControllerRotationReference : ExpressionReference
    {

        public static string description = "{left|right} controller's rotation {x|y|z}";

        public ControllerRotationReference(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer, bool sample) : base(owner, optionList, ScriptingType.sensor, referenceSlotViewers, viewer, RefType.numberType, sample)
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
            string controller = GetOption(0).ValueSet[GetOption(0).Value];
            int axis = GetOption(1).Value;
            float val = 0;
            switch (axis)
            {
                case 0: val = InputDeviceController.controllers[controller].Device.Rotation.x; break;
                case 1: val = InputDeviceController.controllers[controller].Device.Rotation.y; break;
                case 2: val = InputDeviceController.controllers[controller].Device.Rotation.z; break;
                default: throw new ArgumentException("Bad axis");
            }
            return val.ToStdStr();
        }
    }

    public class HeadPositionReference : ExpressionReference
    {

        public static string description = "Head's position {x|y|z}";

        public HeadPositionReference(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer, bool sample) : base(owner, optionList, ScriptingType.sensor, referenceSlotViewers, viewer, RefType.numberType, sample)
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
            int axis = GetOption(0).Value;
            float val = 0;
            switch (axis)
            {
                case 0: val = InputDeviceController.controllers["head"].Device.Position.x; break;
                case 1: val = InputDeviceController.controllers["head"].Device.Position.y; break;
                case 2: val = InputDeviceController.controllers["head"].Device.Position.z; break;
                default: throw new ArgumentException("Bad axis");
            }
            return val.ToStdStr();
        }
    }

    public class HeadRotationReference : ExpressionReference
    {

        public static string description = "Heads's rotation {x|y|z}";

        public HeadRotationReference(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer, bool sample) : base(owner, optionList, ScriptingType.sensor, referenceSlotViewers, viewer, RefType.numberType, sample)
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
            int axis = GetOption(0).Value;
            float val = 0;
            switch (axis)
            {
                case 0: val = InputDeviceController.controllers["head"].Device.Rotation.x; break;
                case 1: val = InputDeviceController.controllers["head"].Device.Rotation.y; break;
                case 2: val = InputDeviceController.controllers["head"].Device.Rotation.z; break;
                default: throw new ArgumentException("Bad axis");
            }
            return val.ToStdStr();
        }
    }

    public class ButtonReference : ExpressionReference
    {

        public static string description = "{left|right} controller's {button A/X}";

        public ButtonReference(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, ReferenceViewer viewer, bool sample) : base(owner, optionList, ScriptingType.sensor, referenceSlotViewers, viewer, RefType.boolType, sample)
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
            string controller = GetOption(0).ValueSet[GetOption(0).Value];
            int button = GetOption(1).Value;
            bool val = Controller.InputDeviceController.controllers[controller].Device.IsButtonPressed(button);
            return val.ToString().ToUpper();
        }
    }

    public class ButtonPressedHat : Hat
    {

        public static string description = "When {button A/X} is pressed";
        private bool sample = false;

        public ButtonPressedHat(Actor owner, List<Option> optionList, List<ReferenceSlotViewer> referenceSlotViewers, HatViewer viewer, bool sample) : base(owner, optionList, ScriptingType.sensor, referenceSlotViewers, viewer, sample)
        {
            this.sample = sample;
            if (!sample)
            {
                Controller.InputDeviceController.controllers["left"].Device.ButtonPressed += RegisterMe;
                Controller.InputDeviceController.controllers["right"].Device.ButtonPressed += RegisterMe;
            }
        }

        private void RegisterMe(int num)
        {
            if (EnvironmentController.Instance.InPlayMode == false)
                return;
            if (num == GetOption(0).Value)
            {
                if (Next != null)
                {
                    ExecutionController.Instance.AddFlux(new Flux(this)
                    {
                        CurrentBlock = Next
                    });
                };
            }
        }

        public override void Destroy()
        {
            if (!sample)
            {
                Controller.InputDeviceController.controllers["left"].Device.ButtonPressed -= RegisterMe;
                Controller.InputDeviceController.controllers["right"].Device.ButtonPressed -= RegisterMe;
            }
            base.Destroy();
        }

        public override string Description
        {
            get
            {
                return description;
            }
        }
    }
}
