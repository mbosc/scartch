using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;

namespace view
{
    public class ActorWrapper : MonoBehaviour
    {
        private Actor actor;
        private Vector3 baseScale;
        public Vector3 centerOffset;

        // Use this for initialization
        void Start()
        {
            // prevedere modello corretto
            actor = new Actor(new Model());
            this.transform.localPosition = actor.Position + centerOffset;
            this.transform.localEulerAngles = actor.Rotation;
            baseScale = transform.localScale;
            this.transform.localScale = actor.Scale * baseScale;
            actor.ActorChanged += UpdateAppearance;
        }

        private void OnDestroy()
        {
            actor.ActorChanged -= UpdateAppearance;
        }

        private void UpdateAppearance()
        {
            this.transform.localPosition = actor.Position + centerOffset;
            this.transform.localEulerAngles = actor.Rotation;
            this.transform.localScale = actor.Scale * baseScale;
        }

        // Update is called once per frame
        void Update()
        {
            //Debug
            if (Input.GetKeyDown(KeyCode.R))
                actor.Position += new Vector3(0, 0, 10);
        }
    }
}
