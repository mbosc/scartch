using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    namespace Resources
    {
        public class VRWindowRayRec : RayHittable
        {
            private VRWindow win;
            public VRWindow winOverload;

            public override void HitByBlueRay()
            {
                //do nothing
            }

            public override void HitByRedRay()
            {
                win.Close();
            }

            private void Start()
            {
                if (winOverload != null)
                    win = winOverload;
                else
                    win = this.GetComponent<VRWindow>();
                win.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            }

        }
    }
}
