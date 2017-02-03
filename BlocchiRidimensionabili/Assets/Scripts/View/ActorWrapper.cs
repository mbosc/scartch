using System.Collections;
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
		private List<ScriptingElementWrapper> scriptingElements = new List<ScriptingElementWrapper>();
        private GameObject highlight;

        // Use this for initialization
        void Start()
        {
			
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
            if (highlight)
                highlight.SetActive(true);
			scriptingElements.ForEach (b => b.gameObject.SetActive (true));
		}
		public void HideBlocks(){
            if (highlight)
                highlight.SetActive(false);
			scriptingElements.ForEach (b => b.gameObject.SetActive (false));
		}
    }
}
