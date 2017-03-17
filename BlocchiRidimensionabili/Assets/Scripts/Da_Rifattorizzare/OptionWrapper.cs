using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;
using System.Linq;

namespace view
{

	public class OptionWrapper : ReferenceContainer
	{

        public override void SelectA()
        {
            if (showing)
                HideOptions();
            else
                ShowOptions();
        }

        private Option _option;
		public Option option {
			get { return _option; }
			set {
				_option = value;
				SetValue (0);
			}
		}
        public UnityEngine.UI.Text myText;

		public override void extend ()
		{
            
            loadOriginaryMesh ();
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
				levert [i] = new Vector3 (levert [i].x - lunghezza + 3, levert [i].y, levert [i].z);
			mesh.SetVertices (new List<Vector3> (levert));

			GameObject dropB = GameObject.Instantiate (dropButton);
			dropB.transform.position = this.transform.position + new Vector3 ((lunghezza - 2) + 0.5f, 0, 0);
			dropB.transform.SetParent (this.transform);
            dropB.GetComponent<Renderer>().material = GetComponent<Renderer>().material;
		}
			
		public GameObject dropButton;
		public GameObject dropElementPrefab;

		private List<GameObject> instancedOptions;
		public bool showing = false;
		public void ShowOptions(){
			showing = true;
			List<string> options = new List<string> ();
			option.PossibleValues.ToList().ForEach(s => options.Add(s.ToString()));
			var longest = 0;
			options.ForEach(s => {if (s.Length > longest) longest = s.Length;});
            if (longest + 1 > lunghezza)
				lunghezza = longest + 1;

			// istanzia i dropelement in giusta posizione
			instancedOptions = new List<GameObject>();
			int i = 0;
			options.ForEach (s => {
				var inst = GameObject.Instantiate(dropElementPrefab);
				inst.GetComponent<OptionWrapperDropDownElement>().Init(s, i++, lunghezza);
				inst.transform.SetParent(transform);
				inst.transform.localPosition = new Vector3(0, 0, -i * 2);
                inst.transform.localRotation = Quaternion.identity;
				instancedOptions.Add(inst);
			});
		}

		public void HideOptions(){
			showing = false;
			instancedOptions.ForEach(Destroy);
		}

		public void SetValue(int i){
			option.chosenValue = i;
			myText.text = option.PossibleValues [i].ToString ();
		}

		protected override void Start ()
		{
			
			if (option == null)
				return;
			else {
				SetValue (0);
				List<string> options = new List<string> ();
				option.PossibleValues.ToList ().ForEach (s => options.Add (s.ToString ()));
				var longest = 0;
				options.ForEach (s => {
					if (s.Length > longest)
						longest = s.Length;
				});
				if (longest + 1 > lunghezza)
					lunghezza = longest + 1;
			}
		}

//		public void SetOptions(IList<object> options){
//			option = new Option (options);
//		}
	}
}