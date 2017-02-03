﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;

namespace view
{
    public class ActorWrapper : MonoBehaviour
    {
        public Actor actor;// TODO set private
        private Vector3 baseScale;
        public Vector3 centerOffset;
		public List<MonoBehaviour> blocks;
        private GameObject highlight;

        // Use this for initialization
        void Start()
        {
            // prevedere modello corretto
			//DEBUG
			if (actor == null)
				actor = new Actor (new Model ("wewe"));

            highlight = this.transform.GetChild(0).gameObject;
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

		public void ShowBlocks(){
            if (highlight)
                highlight.SetActive(true);
			blocks.ForEach (b => b.gameObject.SetActive (true));
		}
		public void HideBlocks(){
            if (highlight)
                highlight.SetActive(false);
			blocks.ForEach (b => b.gameObject.SetActive (false));
		}
    }
}