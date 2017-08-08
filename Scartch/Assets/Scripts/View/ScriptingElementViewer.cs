using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    namespace Resources
    {
        public class ScriptingElementViewer : RayHittable
        {
            private bool visible;

            public bool Visible
            {
                get { return visible; }
                set { visible = value; }
            }

            private Scripting.ScriptingElement scriptingElement;

            public event Action Grabbed;
            public event EventHandler Deleted;

            public override void HitByBlueRay()
            {
                // Do nothing
            }

            public override void HitByRedRay()
            {
                Delete();
            }

            public void Move(Vector3 pos, Vector3 rot)
            {
                //TODO
                // Sposta con interpolazione
            }

            public void Delete()
            {
                if (Deleted != null)
                    Deleted(this, EventArgs.Empty);
            }

            public void Grab()
            {
                //TODO
                // de-snappare tutto quello che e' sopra
            }

            public void Release()
            {
                //TODO
                // verificare se sei in possibile posizione per snappare qualcosa sotto
            }

        }
    }
}
