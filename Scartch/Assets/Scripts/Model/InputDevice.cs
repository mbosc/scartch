using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public class InputDevice
    {
        private Vector3 position;

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

        public InputDevice()
        {

        }
    }
}