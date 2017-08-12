using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    public class ActorViewer : Resources.RayHittable
    {
        private Model.Actor Actor;

        public override void HitByBlueRay()
        {
            throw new NotImplementedException();
        }

        public override void HitByRedRay()
        {
            throw new NotImplementedException();
        }
    }
}
