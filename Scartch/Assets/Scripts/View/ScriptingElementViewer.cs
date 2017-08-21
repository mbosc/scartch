using System;
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
                Controller.EnvironmentController.Instance.ModeChanged += UpdateAttach;
                AllowTwoHanded = false;
                DisableKinematicOnAttach = true;
                EnableKinematicOnDetach = true;
                EnableGravityOnDetach = false;
            }

            protected override void OnDestroy()
            {
                base.OnDestroy();
                Controller.EnvironmentController.Instance.ModeChanged -= UpdateAttach;
            }

            private void UpdateAttach(bool obj)
            {
                CanAttach = !obj;
            }

            protected override void Awake()
            {
                base.Awake();
                var spawn = ScartchResourceManager.instance.GetBlockSpawn();
                this.transform.position = spawn.position;
                var headpos = new Vector3(Controller.InputDeviceController.GetController("head").Device.Position.x, this.transform.position.y, Controller.InputDeviceController.GetController("head").Device.Position.z);
                this.transform.Rotate(Vector3.up, Vector3.up.SignedAngle((this.transform.position - headpos).normalized, this.transform.forward));

                //var line = gameObject.AddComponent<LineRenderer>();
                //line.SetVertexCount(3);
                //line.SetPositions(new Vector3[]
                //{
                //    this.transform.position+this.transform.forward*-60, this.transform.position, headpos
                //});
                
                visible = true;
            }

            private bool visible;

            public bool Visible
            {
                get { return visible; }
                set { visible = value;
                    this.transform.gameObject.SetActive(value);
                }
            }

            public void Init(Scripting.ScriptingElement scriptingElement)
            {
                this.scriptingElement = scriptingElement;
            }

            private Scripting.ScriptingElement scriptingElement;

            public Scripting.ScriptingElement ScriptingElement
            {
                get
                {
                    return scriptingElement;
                }
            }

            public event Action Grabbed;
            public event EventHandler Deleted;

            public abstract Scripting.ScriptingType Type { set; get; }

            public virtual void Delete()
            {
                Grab();
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
