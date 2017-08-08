using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    namespace Resources
    {
        public abstract class RayHittable : MonoBehaviour
        {
            public abstract void HitByBlueRay();
            public abstract void HitByRedRay();
        }   
    }
}
