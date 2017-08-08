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
            }

            public override void HitByRedRay()
            {
                // do nothing
            }
        }
    }
}
