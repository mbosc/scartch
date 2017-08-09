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
            if (hand.Inputs[NVRButtons.Touchpad].IsPressed && !keyboard.gameObject.activeInHierarchy)
                ActiveateKeyboard();
        }

        private void ActiveateKeyboard()
        {
            keyboard.gameObject.transform.position = this.transform.position + new Vector3(0, 0, 10);
            keyboard.gameObject.transform.eulerAngles = Vector3.zero;
            keyboard.Open();
           
        }
    }
}
