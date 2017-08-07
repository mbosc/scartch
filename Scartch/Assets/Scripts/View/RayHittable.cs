using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    namespace Resources
    {
        public interface RayHittable
        {
            void HitByBlueRay();
            void HitByRedRay();
        }   
    }
}
