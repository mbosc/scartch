﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    namespace Resources
    {
        [RequireComponent(typeof(ScriptingElementRayRec))]
        [RequireComponent(typeof(Rigidbody))]
        public abstract class ScriptingElementViewer : NewtonVR.NVRInteractableItem
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
                set { visible = value; }
            }

            private Scripting.ScriptingElement scriptingElement;

            public event Action Grabbed;
            public event EventHandler Deleted;
            
            public void Delete()
            {
                if (Deleted != null)
                    Deleted(this, EventArgs.Empty);
            }

            public virtual void Grab()
            {
                if (Grabbed != null)
                    Grabbed();
            }
            public abstract void Moving();
            public abstract void Release();

        }
    }
}
