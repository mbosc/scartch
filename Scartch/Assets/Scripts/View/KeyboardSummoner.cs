using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;
using System;

namespace View.Resources
{
    public class KeyboardSummoner : MonoBehaviour
    {

        private NVRHand hand;
        private VRKeyboard keyboard;

        void Start()
        {
            hand = this.GetComponent<NVRHand>();
            keyboard = VRKeyboard.Instance;
        }

        void Update()
        {
            if (hand.Inputs[NVRButtons.Touchpad].PressDown)
                if (!keyboard.gameObject.activeInHierarchy)
                    ActiveateKeyboard();
                else
                    keyboard.Close();
        }

        private void ActiveateKeyboard()
        {
            keyboard.Open();
            keyboard.gameObject.transform.position = this.transform.position + this.transform.forward * 10;
            keyboard.gameObject.transform.eulerAngles = Vector3.zero;
            var angle = Vector3.Angle(this.transform.forward, keyboard.transform.forward);
            keyboard.gameObject.transform.eulerAngles -= new Vector3(0, angle, 0);
        }
    }
}
