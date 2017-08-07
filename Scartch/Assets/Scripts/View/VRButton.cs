using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    namespace Resources
    {
        public class VRButton : MonoBehaviour, RayHittable
        {
            public event System.EventHandler Pressed;

            public void HitByBlueRay()
            {
                if (Pressed != null)
                    Pressed(this, EventArgs.Empty);
            }

            public void HitByRedRay()
            {
                // do nothing
            }
        }
    }
}
