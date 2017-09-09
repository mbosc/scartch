using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Model
{
    public class InputDevice
    {
        public void ResetButtonStatus()
        {
            buttons.Keys.ToList().ForEach(n => SetButtonPressed(n, false));
        }

        private Vector3 position;
        private Dictionary<int, bool> buttons;
        public Vector3 Position
        {
            get { return position; }
            set
            {
                position = value;
                if (Updated != null)
                    Updated(position, rotation);
            }
        }

        private Vector3 rotation;

        public Vector3 Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                if (Updated != null)
                    Updated(position, rotation);
            }
        }

        public event System.Action<Vector3, Vector3> Updated;
        public event System.Action<int> ButtonPressed;
        public InputDevice()
        {
            buttons = new Dictionary<int, bool>();
            buttons.Add(0, false);
        }
    
        public bool IsButtonPressed(int num)
        {
            return buttons[num];
        }

        public void SetButtonPressed(int num, bool value)
        {
            buttons[num] = value;
            if (value && ButtonPressed != null)
                ButtonPressed(num);

        }
        
    }
}