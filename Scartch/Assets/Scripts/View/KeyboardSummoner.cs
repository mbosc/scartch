using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace View.Resources
{
    public class KeyboardSummoner : MonoBehaviour
    {

        private NVRHand hand;
        public VRKeyboard keyboard;

        void Start()
        {
            hand = this.GetComponent<NVRHand>();
            keyboard = VRKeyboard.Instance;
        }

        void Update()
        {
            if (hand.Inputs[NVRButtons.Touchpad].IsPressed && !keyboard.gameObject.activeInHierarchy)
                keyboard.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.K))
            {
                Debug.Log("Provo ad attivare la tastiera");
                keyboard.gameObject.SetActive(true);
            }
        }
    }
}
