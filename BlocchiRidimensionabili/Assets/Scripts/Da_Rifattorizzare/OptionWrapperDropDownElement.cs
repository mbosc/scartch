using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;

namespace view
{

	public class OptionWrapperDropDownElement : Selectable
	{
		public UnityEngine.UI.Text myText;
		protected Mesh mesh;
		protected Vector3[] originaryVertices;
		public int lunghezza;

		public void extend ()
		{
			mesh = GetComponent<MeshFilter> ().mesh;
			originaryVertices = mesh.vertices;
			if (lunghezza < 1)
				lunghezza = 1;

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
			if (GetComponent<MeshCollider>())
				Destroy(GetComponent<MeshCollider>());
			gameObject.AddComponent<MeshCollider> ();
		}

		public override void OnDeselection(){
			transform.parent.GetComponent<OptionWrapper> ().SetValue (number);
		}

		private int number;
		public void Init(string text, int number, int length)
		{
			this.name = text;
			this.myText.text = text;
			this.lunghezza = length;
			this.number = number;
			extend ();
		}

		
	}
}
