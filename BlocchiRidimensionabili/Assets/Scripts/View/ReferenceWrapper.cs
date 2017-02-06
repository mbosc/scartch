using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using model;
using System.Linq;

namespace view
{
	public abstract class ReferenceWrapper : ScriptingElementWrapper
	{
        private ActorWrapper ownerWrapper;
        public virtual ActorWrapper Owner
        {
            get { return ownerWrapper; }
            set
            {
                if (ownerWrapper)
                    ownerWrapper.AddScriptingElement(this);
                ownerWrapper = value;
                ownerWrapper.AddScriptingElement(this);
            }
        }

        public override ScriptingElement ScriptingElement
        {
            get
            {
                return reference;
            }
        }

        protected bool compacted = false;
        public bool Compacted { get { return compacted; } }
        public virtual void Compact()
        {
            compacted = true;
            linkedVariables.ForEach(s => { if (s) { s.transform.SetParent(this.transform); s.Compact(); } });
        }
        public virtual void Uncompact()
        {
            compacted = false;
            linkedVariables.ForEach(s => { if (s) {
                    s.transform.SetParent(null);
                    s.Uncompact(); }
            });
        }


        public int lunghezza;
		protected string testo;
		public UnityEngine.UI.Text myText;
		protected Mesh mesh;
		protected Vector3[] originaryVertices;
        public Reference reference;

        protected virtual int lunghezzaTesto
        {
            get
            {
                return myText.text.Length;
            }
        }

        public virtual void extend ()
		{
			lunghezza = lunghezzaTesto;
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
				levert [i] = new Vector3 (originaryVertices [i].x - lunghezza + 2, levert [i].y, levert [i].z);
			mesh.SetVertices (new List<Vector3> (levert));
            //myText.text = testo;
            //if (GetComponent<MeshCollider>())
            //    Destroy(GetComponent<MeshCollider>());
            //gameObject.AddComponent<MeshCollider>();
            //GetComponent<MeshCollider>().convex = true;
            //GetComponent<MeshCollider>().isTrigger = true;
        }
		public ReferenceContainer currentlyHighlighted;

		protected void OnTriggerEnter (Collider collider)
		{
			//if (!Selector.instance.selected || Selector.instance.selected.gameObject != this.gameObject)
			//	return;
			var bucoCorrente = collider.GetComponent<ReferenceContainer> ();
			if (bucoCorrente && bucoCorrente.variabile == null && !assigned && Compatible(bucoCorrente)) {
				if (currentlyHighlighted) {
					currentlyHighlighted.setHighlightVisible (false);
				}
				currentlyHighlighted = bucoCorrente;
				bucoCorrente.setHighlightVisible (true);
			}
		}

        protected abstract bool Compatible(ReferenceContainer bucoCorrente);

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
                assigned = false;
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
			//extend ();
		}

        

        public virtual void Init(ActorWrapper ownerWrapper, Reference reference)
        {
            loadOriginaryMesh();
            bucoVarPrefab = ResourceManager.Instance.bucoVarSquare;
            bucoVarAngPrefab = ResourceManager.Instance.bucoVarAng;
            bucoVarCircPrefab = ResourceManager.Instance.bucoVarCrc;
            bucoVarDDPrefab = ResourceManager.Instance.bucoVarDD;
            slotVariabili = new List<ReferenceContainer>();
            slotDisplacement = new List<int>();
            this.reference = reference;
            this.testo = reference.Name;
            this.name = reference.Name;
            this.Owner = ownerWrapper;
            assigned = false;
            evaluateVars(testo, 0, 0);
            reExtendToMatchText();
        }
        private List<float> originalXses;
        private List<float> translatedXses;
        public bool assigned;
        protected virtual void adaptText()
        {
            translatedXses = new List<float>();
            originalXses.ForEach(s => translatedXses.Add(0));
            var builder = new System.Text.StringBuilder();
            char c = testo[0];
            var goingThrough = false;
            var varIndex = 0;
            for (int i = 0; i < testo.Length; i++)
            {
               

                c = testo[i];
                if (varStarts.Contains(c))
                {
                    var offset = slotVariabili[varIndex++].lunghezza;
                    builder.Append(new string(' ', offset));

                    for (int o = 0; o < slotVariabili.Count; o++)
                        if (slotDisplacement[o] > i)
                        {
                            translatedXses[o] += slotVariabili[varIndex - 1].lunghezza - slotVariabili[varIndex - 1].lunghezzaOriginale;
                           // Debug.Log("Sposto variabile numero " + (o) + " displacement:" + translatedXses[o]);
                        }

                    goingThrough = true;
                }
                else if (varEnds.Contains(c))
                {
                    goingThrough = false;
                }
                else if (!goingThrough)
                {
                    builder.Append(c);
                }
            }
            myText.text = builder.ToString();

            for (int i = 0; i < slotVariabili.Count; i++)
            {
                slotVariabili[i].transform.localPosition = new Vector3(
                    originalXses[i] - translatedXses[i],
                    slotVariabili[i].transform.localPosition.y,
                    slotVariabili[i].transform.localPosition.z
                    );
                if (slotVariabili[i].variabile)
                    slotVariabili[i].variabile.transform.position = slotVariabili[i].transform.position;
            }
        }

        public virtual List<ReferenceWrapper> linkedVariables
        {
            get
            {
                var ex = new List<ReferenceWrapper>();
                slotVariabili.ForEach(s => {
                    if (s.variabile)
                    {
                        ex.Add(s.variabile);
                        s.variabile.linkedVariables.ForEach(ex.Add);
                    }
                });
                //ex.ForEach(Debug.Log);
                return ex;
            }
        }

        private char[] varStarts = new char[] { '[', '<', '(', '{' };
        private char[] varEnds = new char[] { ']', '>', ')', '}' };
        [HideInInspector]
        public List<ReferenceContainer> slotVariabili;
        [HideInInspector]
        public List<int> slotDisplacement;
        protected virtual void evaluateVars(string testo, int posBaseX, int posBaseY)
        {
            originalXses = new List<float>();
            ReferenceContainer curBucoVar = null;
            int inizioBucoVar = 0;
            int countVars = 0, countOpt = 0;
            for (int i = 0; i < testo.Length; i++)
            {
                if (testo[i].Equals('['))
                {
                    curBucoVar = GameObject.Instantiate(bucoVarPrefab).GetComponent<ReferenceContainer>();
                    curBucoVar.transform.position = this.transform.position + new Vector3(posBaseX + i, posBaseY, -.4f);
                    curBucoVar.GetComponent<Renderer>().material = this.GetComponent<Renderer>().material;
                    slotDisplacement.Add(i);
                    inizioBucoVar = i;
                }
                else if (testo[i].Equals(']'))
                {
                    curBucoVar.lunghezzaOriginale = (i - inizioBucoVar + 1);
                    curBucoVar.Init(countVars++);
                    curBucoVar.extend();
                    curBucoVar.transform.SetParent(this.transform);
                    originalXses.Add(curBucoVar.transform.localPosition.x);
                    curBucoVar.OnLunghezzaChange += reExtendToMatchText;
                    slotVariabili.Add(curBucoVar);
                }
                else if (testo[i].Equals('<'))
                {
                    curBucoVar = GameObject.Instantiate(bucoVarAngPrefab).GetComponent<ReferenceContainer>();
                    curBucoVar.transform.position = this.transform.position + new Vector3(posBaseX + i, posBaseY, -.4f);
                    curBucoVar.GetComponent<Renderer>().material = this.GetComponent<Renderer>().material;
                    slotDisplacement.Add(i);
                    inizioBucoVar = i;
                }
                else if (testo[i].Equals('>'))
                {
                    curBucoVar.lunghezzaOriginale = (i - inizioBucoVar + 1);
                    curBucoVar.Init(countVars++);
                    curBucoVar.extend();
                    curBucoVar.transform.SetParent(this.transform);
                    originalXses.Add(curBucoVar.transform.localPosition.x);
                    curBucoVar.OnLunghezzaChange += reExtendToMatchText;
                    slotVariabili.Add(curBucoVar);
                }
                else if (testo[i].Equals('('))
                {
                    curBucoVar = GameObject.Instantiate(bucoVarCircPrefab).GetComponent<ReferenceContainer>();
                    curBucoVar.transform.position = this.transform.position + new Vector3(posBaseX + i, posBaseY, -.4f);
                    curBucoVar.GetComponent<Renderer>().material = this.GetComponent<Renderer>().material;
                    slotDisplacement.Add(i);
                    inizioBucoVar = i;
                }
                else if (testo[i].Equals(')'))
                {
                    curBucoVar.lunghezzaOriginale = (i - inizioBucoVar + 1);
                    curBucoVar.Init(countVars++);
                    curBucoVar.extend();
                    curBucoVar.transform.SetParent(this.transform);
                    originalXses.Add(curBucoVar.transform.localPosition.x);
                    curBucoVar.OnLunghezzaChange += reExtendToMatchText;
                    slotVariabili.Add(curBucoVar);
                }
                else if (testo[i].Equals('{'))
                {
                    curBucoVar = GameObject.Instantiate(bucoVarDDPrefab).GetComponent<OptionWrapper>();

                    curBucoVar.transform.position = this.transform.position + new Vector3(posBaseX + i, posBaseY, 0);
                    curBucoVar.GetComponent<Renderer>().material = this.GetComponent<Renderer>().material;
                    slotDisplacement.Add(i);
                    (curBucoVar as OptionWrapper).option = expression.GetOption(countOpt);
                    inizioBucoVar = i;
                }
                else if (testo[i].Equals('}'))
                {
                    curBucoVar.lunghezzaOriginale = (i - inizioBucoVar + 1);
                    curBucoVar.Init(countOpt++);
                    curBucoVar.extend();
                    curBucoVar.transform.SetParent(this.transform);
                    originalXses.Add(curBucoVar.transform.localPosition.x);
                    curBucoVar.OnLunghezzaChange += reExtendToMatchText;
                    slotVariabili.Add(curBucoVar);
                }
            }
        }
        public virtual void reExtendToMatchText()
        {
            adaptText();
            extend();
        }

        public Expression expression
        {
            get { return reference as Expression; }
        }
        //public new Reference reference
        //{
        //    get { return base.reference; }
        //    set
        //    {
        //        if (value is Expression)
        //            base.reference = value;
        //        else throw new ArgumentException("reference has to be an expression");
        //    }
        //}

        [HideInInspector]
        public GameObject bucoVarPrefab,
        bucoVarAngPrefab,
            bucoVarCircPrefab,
            bucoVarDDPrefab;
        
    }
}