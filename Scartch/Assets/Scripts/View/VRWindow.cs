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

            [Range(0, 1)]
            public int level;


            public float heightOffset;


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
                var spawn = ScartchResourceManager.instance.GetWindowSpawn(level);
                this.transform.position = spawn.position + spawn.up * heightOffset;
                var headpos = new Vector3(Controller.InputDeviceController.GetController("head").Device.Position.x, this.transform.position.y, Controller.InputDeviceController.GetController("head").Device.Position.z);
                // Debugging rotation

                //Debug.Log(Vector3.up.SignedAngle((this.transform.position - headpos).normalized, this.transform.forward));
                //var line = gameObject.AddComponent<LineRenderer>();
                //line.SetVertexCount(3);
                //line.SetPositions(new Vector3[]
                //{
                //    this.transform.position+this.transform.forward*-60, this.transform.position, headpos
                //});
                //Debug.Log("target: " + headpos);

                this.transform.Rotate(Vector3.up, Vector3.up.SignedAngle((this.transform.position - headpos).normalized, this.transform.forward));
            }
        }
    }
}
