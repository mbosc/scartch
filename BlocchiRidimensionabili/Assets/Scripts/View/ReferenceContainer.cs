using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;
using System;

namespace view
{
    public abstract class ReferenceContainer : LaserSelectable
    {
        protected int _lunghezza = 1;
        public System.Action OnLunghezzaChange;
        public int lunghezza
        {
            get
            {
                return _lunghezza;
            }
            set
            {
                _lunghezza = value;
                if (OnLunghezzaChange != null)
                    OnLunghezzaChange();
            }
        }
        public int lunghezzaOriginale;
        protected Mesh mesh;
        protected Vector3[] originaryVertices;
        public ReferenceWrapper variabile;
        protected bool initialised = false;

        public virtual void Init(int number)
        {
            this.number = number;
            _lunghezza = lunghezzaOriginale;
            loadOriginaryMesh();
            def = GetComponent<Renderer>().material.color;
        }

        protected int number;
        public bool reversed = false;
        public virtual void extend()
        {
            if (lunghezza < 1)
                lunghezza = 1;

            List<int> verticesToEdit = new List<int>();

            for (int i = 0; i < originaryVertices.Length; i++)
            {
                var vertex = originaryVertices[i];
                if (!reversed)
                {
                    if (vertex.x < 0)
                        verticesToEdit.Add(i);
                } else if (vertex.x > 0)
                    verticesToEdit.Add(i);
            }

            var levert = mesh.vertices;
            foreach (var i in verticesToEdit)
                if (!reversed)
                {
                    levert[i] = new Vector3(originaryVertices[i].x - lunghezza + 2, levert[i].y, levert[i].z);
                } else
                {
                    levert[i] = new Vector3(originaryVertices[i].x + lunghezza - 2, levert[i].y, levert[i].z);
                }
            mesh.SetVertices(new List<Vector3>(levert));

            //recompute mesh collider
            //var oldparent = transform.parent;
            
            if (GetComponent<BoxCollider>() == null)
                gameObject.AddComponent<BoxCollider>();



            var boxcol = GetComponent<BoxCollider>();
            var reverseFact = reversed ? 1 : -1;
            boxcol.center = new Vector3(reverseFact*(lunghezza / 2 - 1), reverseFact*1.5f, 0);
            boxcol.size = new Vector3(lunghezza, 1, 2);
            boxcol.isTrigger = true;

            
            
            //GetComponent<BoxCollider>().isTrigger = true;
            //transform.SetParent(oldparent);
        }

        protected virtual void loadOriginaryMesh()
        {
            mesh = GetComponent<MeshFilter>().mesh;
            originaryVertices = mesh.vertices;
        }

        private Color def;
        public virtual void setHighlightVisible(bool v)
        {
            GetComponent<MeshRenderer>().enabled = true;
            Color col = v ? Color.green : def;
            GetComponent<Renderer>().material.color = col;
        }



        public virtual void CompletaCon(ReferenceWrapper variabile)
        {
            //verificare correttezza di tipo;
            if (variabile.assigned)
            {
                variabile.transform.position = this.transform.position;
                return;
            }

            variabile.assigned = true;
            lunghezza = variabile.lunghezza;

            var icompct = !variabile.Compacted;
            if (icompct)
                variabile.Compact();
            variabile.transform.SetParent(this.transform);
            variabile.transform.localPosition = Vector3.zero;
            variabile.transform.localEulerAngles = Vector3.zero;
            variabile.transform.SetParent(null);
            Debug.Log("Riempi " + transform.parent.name);
            this.variabile = variabile;
            if (icompct)
                variabile.Uncompact();


            GetComponent<MeshRenderer>().enabled = false;


            //TODO: questo e' molto approssimativo: fare meglio la logica di accoppiamento
            //degli indici alle reference.
            var d = transform.parent.gameObject.GetComponent<BlockWrapper>();
            if (d == null)
            {
                var z = transform.parent.gameObject.GetComponent<ReferenceWrapper>();
                if (innerVariable != null)
                {
                    z.expression.RemoveReference(number);
                    innerVariable = null;
                    ImmediateText.text = "";
                }
                z.expression.AddReference(number, variabile.reference);
            }
            else
            {
                if (innerVariable != null)
                {
                    d.block.RemoveReference(number);
                    innerVariable = null;
                    ImmediateText.text = "";
                }
                d.block.AddReference(number, variabile.reference);
            }
        }

        protected Variable innerVariable;

        public virtual void Svuota()
        {
            Debug.Log("Svuota " + transform.parent.name);
            lunghezza = lunghezzaOriginale;
            //		extend ();
            this.variabile = null;
            if (transform.parent.gameObject.GetComponent<BlockWrapper>())
                transform.parent.gameObject.GetComponent<BlockWrapper>().block.RemoveReference(number);
            else
                transform.parent.gameObject.GetComponent<ReferenceWrapper>().expression.RemoveReference(number);

        }

        protected UnityEngine.UI.Text ImmediateText;

        // Use this for initialization
        protected virtual void Start()
        {
            ImmediateText = GetComponentInChildren<UnityEngine.UI.Text>();
            ImmediateText.text = "";
        }
       

        
    }

}
