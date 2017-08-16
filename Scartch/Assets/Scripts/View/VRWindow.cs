using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace View
{
    namespace Resources
    {
        [RequireComponent(typeof(VRWindowRayRec))]
        [RequireComponent(typeof(Rigidbody))]
        public class VRWindow : NewtonVR.NVRInteractableItem
        {
            protected override void Start()
            {
                base.Start();
                CanAttach = true;
                AllowTwoHanded = false;
                DisableKinematicOnAttach = true;
                EnableKinematicOnDetach = true;
                EnableGravityOnDetach = false;
            }

            protected override void Awake()
            {
                base.Awake();
                visible = true;
            }
            
            

            private bool visible;

            public bool Visible
            {
                get { return visible; }
                set
                {
                    visible = value;
                    gameObject.SetActive(value);
                }
            }


            public virtual void Close()
            {
                Visible = false;
            }

            public virtual void Open()
            {
                Visible = true;
            }
        }
    }
}
