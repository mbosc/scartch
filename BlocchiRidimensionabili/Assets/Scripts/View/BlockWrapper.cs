using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using model;

namespace view
{
    public class BlockWrapper : MonoBehaviour
    {
        public ActorWrapper ownerWrapper;
        public virtual ActorWrapper Owner {
            get { return ownerWrapper; }
            set {
				if (ownerWrapper)
					ownerWrapper.RemoveBlock (this);
                ownerWrapper = value;
				ownerWrapper.AddBlock (this);
            }
        }
        public Block block;
        [HideInInspector]
        public List<ReferenceContainer> slotVariabili;
        [HideInInspector]
        public BlockWrapper next;
        [HideInInspector]
        public BlockWrapperCog dente;
        public virtual List<BlockWrapperCog> denti
        {
            get
            {
                var r = new List<BlockWrapperCog>();
                r.Add(dente);
                return r;
            }
        }
        [HideInInspector]
        public BlockWrapperCogHole spazioDente;


        [HideInInspector]
        public string testo;
        [HideInInspector]
        public UnityEngine.UI.Text campoTesto;
        public virtual int size
        {
            get { return 1; }
        }
        protected Mesh mesh;
        protected float nextBlockOffsetX = 0;
        protected float nextBlockOffsetY = -2;
        protected float prevBlockOffsetX = 0;
        protected float prevBlockOffsetY = 2;
        protected float dongExt;

        protected float deformConst = 1;
        protected Vector3[] originaryVertices;
        [HideInInspector]
        public bool lastBlock = false;

        public virtual string EvaluateMe(string tabs)
        {
            var output = tabs + testo + "\n";
            if (next)
                output += next.EvaluateMe(tabs);
            return output;
        }

        public override string ToString()
        {
            return testo;
        }

        public virtual void setNext(BlockWrapper candidateNext)
        {
            Debug.Log(testo + ".setNext(" + candidateNext.testo + ")");

            var selectionList = new Dictionary<GameObject, Vector2>();
            candidateNext.linkedBlocks.ForEach(s => selectionList.Add(s.gameObject, new Vector2(candidateNext.gameObject.transform.position.x - s.gameObject.transform.position.x, candidateNext.gameObject.transform.position.y - s.gameObject.transform.position.y)));
            selectionList.Add(candidateNext.gameObject, new Vector2(0, 0));



            var selectionVariables = new Dictionary<GameObject, Vector2>();
            candidateNext.linkedVariables.ForEach(s => selectionVariables.Add(s.gameObject, new Vector2(candidateNext.gameObject.transform.position.x - s.gameObject.transform.position.x, candidateNext.gameObject.transform.position.y - s.gameObject.transform.position.y)));

            selectionList.Keys.ToList().ForEach(k =>
            {
                k.transform.position = this.transform.position + new Vector3(nextBlockOffsetX - selectionList[k].x, nextBlockOffsetY - selectionList[k].y, 0);
                k.transform.rotation = this.transform.rotation;
            });

            selectionVariables.Keys.ToList().ForEach(k =>
            {
                Debug.Log("Sposto " + k.name);
                k.transform.position = this.transform.position + new Vector3(nextBlockOffsetX - selectionVariables[k].x, nextBlockOffsetY - selectionVariables[k].y, 0);
            });


            var oldNext = next;
            next = candidateNext;
            block.Next = candidateNext.block;

            candidateNext.spazioDente.Receiving = false;
            dente.Receiving = false;
            dente.Receiving = true;
            if (oldNext)
            {
                var blocchi = new List<BlockWrapper>();
                blocchi.Add(next);
                next.linkedBlocks.ForEach(blocchi.Add);
                blocchi.Last().setNext(oldNext);
            }
        }

        public virtual List<BlockWrapper> linkedBlocks
        {
            get
            {
                var ex = new List<BlockWrapper>();
                if (next)
                {
                    next.linkedBlocks.ForEach(s => { if (s) ex.Add(s); });
                    ex.Add(next);
                }
                return ex;
            }
        }
        public virtual List<ReferenceWrapper> linkedVariables
        {
            get
            {
                var ex = new List<ReferenceWrapper>();
                linkedBlocks.ForEach(s => { s.slotVariabili.ForEach(z => { if (z.variabile) ex.Add(z.variabile); }); });
                slotVariabili.ForEach(s => { if (s.variabile) ex.Add(s.variabile); });
                ex.ForEach(Debug.Log);
                return ex;
            }
        }
        public virtual List<BlockWrapper> directlyLinkedBlocks
        {
            get
            {
                var ex = new List<BlockWrapper>();
                ex.Add(next);
                return ex;
            }
        }
        public virtual void unsetNext(BlockWrapper thisBlocco)
        {
            Debug.Log(testo + ".unsetNext()");
            if (next)
            {
                next.spazioDente.Receiving = true;
                block.UnsetNext();
            }
            next = null;
        }

        public virtual void setPrevious(BlockWrapperCog candidatePrevious)
        {
            Debug.Log(testo + ".setPrevious(" + candidatePrevious.transform.parent.GetComponent<BlockWrapper>().testo + ")");

            candidatePrevious.setNext(this);
            spazioDente.Receiving = false;
        }

        protected virtual void extendToMatchText()
        {
            dongExt = lunghezzaTesto * deformConst;

            if (dongExt < 1)
                dongExt = 1;


            List<int> verticesToEdit = new List<int>();

            int maxX = int.MinValue;
            for (int i = 0; i < originaryVertices.Length; i++)
            {
                var vertex = originaryVertices[i];
                if (Mathf.RoundToInt(vertex.x) > maxX)
                    maxX = Mathf.RoundToInt(vertex.x);
            }

            for (int i = 0; i < originaryVertices.Length; i++)
            {
                var vertex = originaryVertices[i];
                if (Mathf.RoundToInt(vertex.x) == maxX)
                    verticesToEdit.Add(i);
            }

            var levert = mesh.vertices;
            foreach (var i in verticesToEdit)
                levert[i] = new Vector3(originaryVertices[i].x + dongExt, levert[i].y, levert[i].z);
            mesh.SetVertices(new List<Vector3>(levert));
        }

        protected virtual void loadOriginaryMesh()
        {
            mesh = GetComponent<MeshFilter>().mesh;
            originaryVertices = mesh.vertices;
        }


        protected virtual int lunghezzaTesto
        {
            get
            {
                return campoTesto.text.Length;
            }
        }

        [HideInInspector]
        public GameObject bucoVarPrefab,
        bucoVarAngPrefab,
            bucoVarCircPrefab,
            bucoVarDDPrefab;

        public virtual void reExtendToMatchText()
        {
            adaptText();
            extendToMatchText();
        }

        protected virtual void adaptText()
        {
            var builder = new System.Text.StringBuilder();
            char c = testo[0];
            var goingThrough = false;
            var varIndex = 0;
            for (int i = 0; i < testo.Length; i++)
            {
                c = testo[i];
                if (varStarts.Contains(c))
                {
                    builder.Append(new string(' ', slotVariabili[varIndex++].lunghezza));
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
            campoTesto.text = builder.ToString();
        }

        private char[] varStarts = new char[] { '[', '<', '(', '{' };
        private char[] varEnds = new char[] { ']', '>', ')', '}' };

        protected virtual void evaluateVars(string testo, int posBaseX, int posBaseY)
        {
            ReferenceContainer curBucoVar = null;
            int inizioBucoVar = 0;
            for (int i = 0; i < testo.Length; i++)
            {
                if (testo[i].Equals('['))
                {
                    curBucoVar = GameObject.Instantiate(bucoVarPrefab).GetComponent<ReferenceContainer>();
                    curBucoVar.transform.position = this.transform.position + new Vector3(posBaseX + i, posBaseY, 0);
					curBucoVar.GetComponent<Renderer> ().material = this.GetComponent<Renderer> ().material;
                    inizioBucoVar = i;
                }
                else if (testo[i].Equals(']'))
                {
                    curBucoVar.lunghezzaOriginale = (i - inizioBucoVar + 1);
                    curBucoVar.extend();
                    curBucoVar.transform.SetParent(this.transform);
                    slotVariabili.Add(curBucoVar);
                }
                else if (testo[i].Equals('<'))
                {
                    curBucoVar = GameObject.Instantiate(bucoVarAngPrefab).GetComponent<ReferenceContainer>();
                    curBucoVar.transform.position = this.transform.position + new Vector3(posBaseX + i, posBaseY, 0);
					curBucoVar.GetComponent<Renderer> ().material = this.GetComponent<Renderer> ().material;
                    inizioBucoVar = i;
                }
                else if (testo[i].Equals('>'))
                {
                    curBucoVar.lunghezzaOriginale = (i - inizioBucoVar + 1);
                    curBucoVar.extend();
                    curBucoVar.transform.SetParent(this.transform);
                    curBucoVar.OnLunghezzaChange += reExtendToMatchText;
                    slotVariabili.Add(curBucoVar);
                }
                else if (testo[i].Equals('('))
                {
                    curBucoVar = GameObject.Instantiate(bucoVarCircPrefab).GetComponent<ReferenceContainer>();
                    curBucoVar.transform.position = this.transform.position + new Vector3(posBaseX + i, posBaseY, 0);
					curBucoVar.GetComponent<Renderer> ().material = this.GetComponent<Renderer> ().material;
                    inizioBucoVar = i;
                }
                else if (testo[i].Equals(')'))
                {
                    curBucoVar.lunghezzaOriginale = (i - inizioBucoVar + 1);
                    curBucoVar.extend();
                    curBucoVar.transform.SetParent(this.transform);
                    slotVariabili.Add(curBucoVar);
                }
                else if (testo[i].Equals('{'))
                {
                    curBucoVar = GameObject.Instantiate(bucoVarDDPrefab).GetComponent<ReferenceContainer>();
                    curBucoVar.transform.position = this.transform.position + new Vector3(posBaseX + i, posBaseY, 0);
					curBucoVar.GetComponent<Renderer> ().material = this.GetComponent<Renderer> ().material;
                    inizioBucoVar = i;
                }
                else if (testo[i].Equals('}'))
                {
                    curBucoVar.lunghezzaOriginale = (i - inizioBucoVar + 1);
                    curBucoVar.extend();
                    curBucoVar.transform.SetParent(this.transform);
                    slotVariabili.Add(curBucoVar);
                }
            }
        }

        protected virtual void evaluateLastBlock()
        {
            List<int> verticesToEdit = new List<int>();

            int minZ = int.MaxValue;
            for (int i = 0; i < originaryVertices.Length; i++)
            {
                var vertex = originaryVertices[i];
                if (Mathf.RoundToInt(vertex.z) < minZ)
                    minZ = Mathf.RoundToInt(vertex.z);
            }

            for (int i = 0; i < originaryVertices.Length; i++)
            {
                var vertex = originaryVertices[i];
                if (Mathf.RoundToInt(vertex.z) == minZ)
                    verticesToEdit.Add(i);
            }

            var levert = mesh.vertices;
            foreach (var i in verticesToEdit)
                levert[i] = new Vector3(levert[i].x, levert[i].y, levert[i].z + 1);
            mesh.SetVertices(new List<Vector3>(levert));
        }

        protected int offsetTestoBaseX = 1;



        // Use this for initialization
		public virtual void Init(ActorWrapper ownerWrapper, bool autoinit = true)
        {
			this.Owner = ownerWrapper;
            bucoVarPrefab = ResourceManager.Instance.bucoVarSquare;
            bucoVarAngPrefab = ResourceManager.Instance.bucoVarAng;
            bucoVarCircPrefab = ResourceManager.Instance.bucoVarCrc; 
            bucoVarDDPrefab = ResourceManager.Instance.bucoVarDD;
            slotVariabili = new List<ReferenceContainer>();
            try
            {
                campoTesto = transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>();
                dente = transform.GetChild(1).GetComponent<BlockWrapperCog>();
                spazioDente = transform.GetChild(2).GetComponent<BlockWrapperCogHole>();
            }
            catch
            {
                Debug.Log("Problemi inizializzazione per " + name);
            }
            if (lastBlock)
                Destroy(dente);
            else
            {
                dente.setNext = setNext;
                dente.unsetNext = unsetNext;
            }
            spazioDente.setPrevious = setPrevious;
            name = testo;
            loadOriginaryMesh();
            campoTesto.text = testo;
            
            if (lastBlock) evaluateLastBlock();
            evaluateVars(testo, offsetTestoBaseX, 0);
            reExtendToMatchText();
			initialised = autoinit;

        }

		protected bool initialised = false;
        protected bool meshCalculated = false;
        // Update is called once per frame
        protected virtual void Update()
        {
			if (!initialised)
				return;
			if (!meshCalculated)
            {
				meshCalculated = true;
                this.gameObject.AddComponent<MeshCollider>();
            }
        }
    }
}