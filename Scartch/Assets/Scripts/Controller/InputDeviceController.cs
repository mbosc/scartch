using System.Collections;
using System.Collections.Generic;
using System;
using Model;
using View;

namespace Controller
{
    public class InputDeviceController 
    {
        private InputDevice device;
        private InputViewer viewer;
        public InputDevice Device
        {
            get { return device; }
        }

        public static Dictionary<string, InputDeviceController> controllers = new Dictionary<string, InputDeviceController>();
        public static InputDeviceController GetController(string qualifier)
        {
            if (controllers.ContainsKey(qualifier))
                return controllers[qualifier];
            else
            {
                throw new ArgumentException("No device with this name");
            }
        }

        public static void RegisterDevice(InputDevice device, InputViewer viewer, string qualifier)
        {
            controllers.Add(qualifier, new InputDeviceController(device, viewer));
        }

        private InputDeviceController(InputDevice device, InputViewer viewer)
        {
            this.device = device;
            this.viewer = viewer;
            viewer.PosRotUpdated += OnViewerUpdated;
            viewer.KeyPressed += OnViewerKeyPressed;
            viewer.KeyReleased += OnViewerKeyReleased;
        }

        private void OnViewerKeyReleased(int obj)
        {
            device.SetButtonPressed(obj, false);
        }

        private void OnViewerKeyPressed(int obj)
        {
            device.SetButtonPressed(obj, true);
        }

        private void OnViewerUpdated(UnityEngine.Vector3 arg1, UnityEngine.Vector3 arg2)
        {
            device.Position = arg1;
            device.Rotation = arg2;
        }
    }
}
