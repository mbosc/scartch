using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using model;

namespace view
{
    public class BlockWrapper : ScriptingElementWrapper
    {
        public override ScriptingElement ScriptingElement
        {
            get
            {
                return block;
            }
        }
        public ActorWrapper ownerWrapper;
        public virtual ActorWrapper Owner {
            get { return ownerWrapper; }
            set {
				if (ownerWrapper)
					ownerWrapper.AddScriptingElement (this);
                ownerWrapper = value;
				ownerWrapper.AddScriptingElement (this);
            }
        }
        public Block block;
        [HideInInspector]
        public List<ReferenceContainer> slotVariabili;
        [HideInInspector]
        public List<int> slotDisplacement;
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
        protected float nextBlockSpinZ = 0;
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

        protected bool compacted = false;
        public bool Compacted { get { return compacted; } }
        //metodi per passare da/al raccoglimento in gerarchia dei prossimi blocchi (TODO variabili);
        public virtual void Compact()
        {
            Debug.Log("Compatto " + name);
            compacted = true;
            directlyLinkedBlocks.ForEach(s => {
                if (s)
                {
                    s.transform.SetParent(this.transform);
                    s.Compact();
                }
            });
            directlyLinkedVariables.ForEach(s => {
                if (s)
                    s.transform.SetParent(this.transform);
                    s.Compact();
                });
        }
        public virtual void Uncompact()
        {

            Debug.Log("Decompatto " + name);
            compacted = false;
            directlyLinkedBlocks.ForEach(s => { if (s) { s.transform.SetParent(null); s.Uncompact(); } });
            directlyLinkedVariables.ForEach(s => { if(s) s.transform.SetParent(null); s.Uncompact(); });
        }

        protected void moveUnderMe(BlockWrapper candidateNext, Vector3 positionOffset, Vector3 rotationOffset)
        {
            var contextualCompacting = !candidateNext.compacted;
            if (contextualCompacting)
                candidateNext.Compact();
            candidateNext.transform.SetParent(this.transform);
            candidateNext.transform.localPosition = positionOffset;
            candidateNext.transform.localEulerAngles = rotationOffset;
            candidateNext.transform.SetParent(null);
            if (contextualCompacting)
               candidateNext.Uncompact();
        }

        public virtual void setNext(BlockWrapper candidateNext)
        {
            Debug.Log(testo + ".setNext(" + candidateNext.testo + ")");

            moveUnderMe(candidateNext, new Vector3(nextBlockOffsetX, 0, nextBlockOffsetY), new Vector3(0, 0, nextBlockSpinZ));

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
        public virtual List<ReferenceWrapper> directlyLinkedVariables
        {
            get
            {
                var ex = new List<ReferenceWrapper>();
                slotVariabili.ForEach(s => { if (s.variabile) { ex.Add(s.variabile); s.variabile.linkedVariables.ForEach(ex.Add); } });
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

        private List<float> originalXses;
        private List<float> translatedXses;
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
                          //  Debug.Log("Sposto variabile numero " + (o) + " displacement:" + translatedXses[o]);
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
            campoTesto.text = builder.ToString();

            for (int i = 0; i < slotVariabili.Count; i++)
            {
                slotVariabili[i].transform.localPosition = new Vector3(
                    originalXses[i] + translatedXses[i],
                    slotVariabili[i].transform.localPosition.y,
                    slotVariabili[i].transform.localPosition.z
                    );
                if (slotVariabili[i].variabile)
                    slotVariabili[i].variabile.transform.position = slotVariabili[i].transform.position;
            }
        }

        private char[] varStarts = new char[] { '[', '<', '(', '{' };
        private char[] varEnds = new char[] { ']', '>', ')', '}' };

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
                    curBucoVar.transform.position = this.transform.position + new Vector3(posBaseX + i, posBaseY, 0);
                    
                    curBucoVar.GetComponent<Renderer> ().material = this.GetComponent<Renderer> ().material;
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
                    curBucoVar.transform.position = this.transform.position + new Vector3(posBaseX + i, posBaseY, 0);
              
                    curBucoVar.GetComponent<Renderer> ().material = this.GetComponent<Renderer> ().material;
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
                    curBucoVar.transform.position = this.transform.position + new Vector3(posBaseX + i, posBaseY, 0);
         
                    curBucoVar.GetComponent<Renderer> ().material = this.GetComponent<Renderer> ().material;
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
           
                    curBucoVar.GetComponent<Renderer> ().material = this.GetComponent<Renderer> ().material;
                    slotDisplacement.Add(i);
                    (curBucoVar as OptionWrapper).option = block.GetOption(countOpt);
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
            slotDisplacement = new List<int>();
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
            }
        }
    }
}