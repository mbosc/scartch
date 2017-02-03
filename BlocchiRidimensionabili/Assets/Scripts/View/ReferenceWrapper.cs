using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using model;

namespace view
{
	public abstract class ReferenceWrapper : MonoBehaviour
	{
        private ActorWrapper ownerWrapper;
        public virtual ActorWrapper Owner
        {
            get { return ownerWrapper; }
            set
            {
                ownerWrapper = value;
				ownerWrapper.AddBlock
            }
        }

        public int lunghezza;
		public string testo;
		public UnityEngine.UI.Text myText;
		protected Mesh mesh;
		protected Vector3[] originaryVertices;
        public Reference reference;

		public virtual void extend ()
		{
			loadOriginaryMesh ();
			lunghezza = testo.ToString ().Length;
			if (lunghezza < 2)
				lunghezza = 2;
		
			List<int> verticesToEdit = new List<int> ();

			for (int i = 0; i < originaryVertices.Length; i++) {
				var vertex = originaryVertices [i];
				if (vertex.x < 0)
					verticesToEdit.Add (i);
			}

			var levert = mesh.vertices;
			foreach (var i in verticesToEdit)
				levert [i] = new Vector3 (levert [i].x - lunghezza + 2, levert [i].y, levert [i].z);
			mesh.SetVertices (new List<Vector3> (levert));
			myText.text = testo;
            if (GetComponent<MeshCollider>())
                Destroy(GetComponent<MeshCollider>());
			gameObject.AddComponent<MeshCollider> ();
			GetComponent<MeshCollider> ().convex = true;
			GetComponent<MeshCollider> ().isTrigger = true;
		}
		public ReferenceContainer currentlyHighlighted;

		protected void OnTriggerEnter (Collider collider)
		{
			//if (!Selector.instance.selected || Selector.instance.selected.gameObject != this.gameObject)
			//	return;
			var bucoCorrente = collider.GetComponent<ReferenceContainer> ();
			if (bucoCorrente && bucoCorrente.variabile == null) {
				if (currentlyHighlighted) {
					currentlyHighlighted.setHighlightVisible (false);
				}
				currentlyHighlighted = bucoCorrente;
				bucoCorrente.setHighlightVisible (true);
			}
		}

		protected void OnTriggerStay (Collider collider)
		{
			//if (!Selector.instance.selected || Selector.instance.selected.gameObject != this.gameObject)
			//	return;
			var bucoCorrente = collider.GetComponent<ReferenceContainer> ();
			try {
				if (bucoCorrente.Equals (currentlyHighlighted))
					bucoCorrente.setHighlightVisible (true);
			} catch (NullReferenceException) {
			}
		}

		void OnTriggerExit (Collider collider)
		{
			var bucoCorrente = collider.GetComponent<ReferenceContainer> ();
			if (bucoCorrente && bucoCorrente.variabile && bucoCorrente.variabile.Equals (this)) {
				bucoCorrente.Svuota ();
			}
			ExitTrigger (collider);
		}

		public void ExitTrigger (Collider collider)
		{
			var bucoCorrente = collider.GetComponent<ReferenceContainer> ();
			if (bucoCorrente && bucoCorrente == currentlyHighlighted) {
				bucoCorrente.setHighlightVisible (false);
				currentlyHighlighted = null;
			}
		}

		protected virtual void loadOriginaryMesh ()
		{
			mesh = GetComponent<MeshFilter> ().mesh;
			originaryVertices = mesh.vertices;
		}

		// Use this for initialization
		protected virtual void Start ()
		{
			extend ();
		}

	}
}