using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;
using System;
using System.Linq;

namespace view
{
    public class ActorWrapper : MonoBehaviour
    {
        private Actor actor;// TODO set private
        public Actor Actor { get { return actor; } }
        private Vector3 baseScale;
        public Vector3 centerOffset;
		private List<ScriptingElementWrapper> scriptingElements = new List<ScriptingElementWrapper>();
        private GameObject highlight;
		public ActorMessage message;
        // Use this for initialization
        public void Init(Actor actor)
        {
            this.actor = actor;
            this.name = actor.Name;
            highlight = this.transform.GetChild(0).gameObject;
			message = this.transform.GetChild (1).GetComponent<ActorMessage> ();
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
			for (int i = 2; i < this.transform.childCount; i++) {
				try {
					this.transform.GetChild (i).gameObject.GetComponent<Renderer> ().enabled = !actor.Hidden;
				} catch (MissingComponentException) {
				}
			}

			this.message.Text = actor.Message;
			this.message.gameObject.SetActive (actor.IsMessageVisible);
        }

        // Update is called once per frame
        void Update()
        {
            
        }

		public void AddScriptingElement(ScriptingElementWrapper b){
			scriptingElements.Add (b);
			actor.AddScriptingElement (b.ScriptingElement);
		}
		public void RemoveScriptingElement(ScriptingElementWrapper b){
			scriptingElements.Remove (b);
			actor.RemoveScriptingElement (b.ScriptingElement);
		}

		public void ShowBlocks(){
            Highlight(true);
			scriptingElements.ForEach (b => b.gameObject.SetActive (true));
		}

        private void Highlight(bool doing)
        {
            GetComponentsInChildren<Highlightable>().ToList().ForEach(s => s.Highlight(doing));
        }

        public void HideBlocks(){
            Highlight(false);
            scriptingElements.ForEach (b => b.gameObject.SetActive (false));
		}
    }
}
