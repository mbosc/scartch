using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    namespace Resources
    {
        public class VRButton : RayHittable
        {
            public event System.EventHandler Pressed;

            public override void HitByBlueRay()
            {
                if (Pressed != null)
                    Pressed(this, EventArgs.Empty);
                Debug.Log("Hit by blue");
            }

            public override void HitByRedRay()
            {
                // do nothing
                Debug.Log("Hit by red");
            }
        }
    }
}
