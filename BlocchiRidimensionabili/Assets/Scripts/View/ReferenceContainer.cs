using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;

namespace view
{
    public class ReferenceContainer : MonoBehaviour
    {
        public string type;
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

        private int number;
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
            if (GetComponent<MeshCollider>())
                Destroy(GetComponent<MeshCollider>());
            gameObject.AddComponent<MeshCollider>();
            GetComponent<MeshCollider>().convex = true;
            GetComponent<MeshCollider>().isTrigger = true;
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

            variabile.Compact();
            variabile.transform.SetParent(this.transform);
            variabile.transform.localPosition = Vector3.zero;
            variabile.transform.localEulerAngles = Vector3.zero;
            variabile.transform.SetParent(null);
            variabile.Uncompact();

            this.variabile = variabile;
            GetComponent<MeshRenderer>().enabled = false;


            //TODO: questo e' molto approssimativo: fare meglio la logica di accoppiamento
            //degli indici alle reference.
            var d = transform.parent.gameObject.GetComponent<BlockWrapper>();
            if (d == null)
            {
                var z = transform.parent.gameObject.GetComponent<ReferenceWrapper>();

                z.expression.AddReference(number, variabile.reference);
            }
            else
            {

                d.block.AddReference(number, variabile.reference);
            }
        }

        public virtual void Svuota()
        {
            lunghezza = lunghezzaOriginale;
            //		extend ();
            this.variabile = null;
            if (transform.parent.gameObject.GetComponent<BlockWrapper>())
                transform.parent.gameObject.GetComponent<BlockWrapper>().block.RemoveReference(number);
            else
                transform.parent.gameObject.GetComponent<ReferenceWrapper>().expression.RemoveReference(number);

        }

        // Use this for initialization
        protected virtual void Start()
        {

        }
    }

}
