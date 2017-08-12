using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    public class InputViewer : MonoBehaviour
    {
        public event System.Action<Vector3, Vector3> PosRotUpdated;
        public string qualifier;
        private void Start()
        {
            Controller.InputDeviceController.RegisterDevice(new Model.InputDevice(), this, qualifier);
        }

        private void Update()
        {
            if (PosRotUpdated != null)
                PosRotUpdated(this.transform.position, this.transform.eulerAngles);
        }
    }
}
