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

        void Start()
        {
            hand = this.GetComponent<NVRHand>();
        }

        void Update()
        {
            if (hand.Inputs[NVRButtons.Touchpad].PressDown)
			if (!VRKeyboard.Instance.gameObject.activeInHierarchy)
                    ActiveateKeyboard();
                else
				VRKeyboard.Instance.Close();
        }

        private void ActiveateKeyboard()
        {
			VRKeyboard.Instance.Open();
			VRKeyboard.Instance.gameObject.transform.position = this.transform.position + this.transform.forward * 10;
			VRKeyboard.Instance.gameObject.transform.eulerAngles = Vector3.zero;
			var angle = Vector3.Angle(this.transform.forward, VRKeyboard.Instance.transform.forward);
			VRKeyboard.Instance.gameObject.transform.eulerAngles -= new Vector3(0, angle, 0);
        }
    }
}
