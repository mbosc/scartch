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
		private List<MonoBehaviour> blocks = new List<MonoBehaviour>();
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

		public void AddScriptingElement(BlockWrapper b){
			blocks.Add (b);
			actor.AddBlock (b.block);
		}
		public void RemoveScriptingElement(BlockWrapper b){
			blocks.Remove (b);
			actor.RemoveBlock (b.block);
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
