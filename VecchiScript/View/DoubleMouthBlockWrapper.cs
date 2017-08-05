using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using model;

namespace view
{
    public class DoubleMouthBlockWrapper : BlockWrapper
    {
        public new DoubleMouthBlock block
        {
            get { return base.block as DoubleMouthBlock; }
            set { base.block = value; }
        }
        public int firstStretchSize = 1;
        public int secondStretchSize = 1;
        public string secondoTesto;
        public UnityEngine.UI.Text secondoCampoTesto;

        public BlockWrapper upperInternalNext;
        public BlockWrapper lowerInternalNext;
        public BlockWrapperCog denteInternoSuperiore, denteInternoInferiore;
        protected float nextBlockInternoSuperioreOffsetX = 1;
        protected float nextBlockInternoSuperioreOffsetY = -2;
        protected float nextBlockInternoInferioreOffsetX = 1;
        protected float nextBlockInternoInferioreOffsetY
        {
            get
            {
                return -4 - firstStretchSize * 2;
            }
        }
        public override int size
        {
            get
            {
                return 3 + firstStretchSize + secondStretchSize;
            }
        }
        public override List<BlockWrapperCog> denti
        {
            get
            {
                var r = new List<BlockWrapperCog>();
                r.Add(dente);
                r.Add(denteInternoInferiore);
                r.Add(denteInternoSuperiore);
                return r;
            }
        }

        public virtual void setNextInternoSuperiore(BlockWrapper candidateNext)
        {
            Debug.Log(testo + ".setNextInternoSuperiore(" + candidateNext.testo + ")");


            moveUnderMe(candidateNext, new Vector3(nextBlockInternoSuperioreOffsetX, 0, nextBlockInternoSuperioreOffsetY), new Vector3(0, 0, nextBlockSpinZ));



            var oldNext = upperInternalNext;
            upperInternalNext = candidateNext;
            block.FirstInnerNext = candidateNext.block;
            upperInternalNext.linkedBlocks.ForEach(AumentaLunghezzaSuperiore);
            AumentaLunghezzaSuperiore(upperInternalNext);

            candidateNext.spazioDente.Receiving = false;
            denteInternoSuperiore.Receiving = false;
            denteInternoSuperiore.Receiving = true;
            if (oldNext)
            {
                upperInternalNext.setNext(oldNext);
            }
        }

        public void AumentaLunghezzaSuperiore(BlockWrapper next)
        {
            var lung = 0;
            BlockWrapper catena = upperInternalNext;
            while (catena)
            {
                lung += catena.size;
                catena = catena.next;
            }
            firstStretchSize = lung;
            extendToMatchContent();
            next.denti.ForEach(d => d.setNext += AumentaLunghezzaSuperiore);
            Debug.Log("Invocation list di " + next + ".setNext: " + next.dente.setNext.GetInvocationList().Length);
            next.denti.ForEach(d => d.unsetNext += DiminuisciLunghezzaSuperiore);

            RiposizionaBloccoInferiore();
        }

        private void RiposizionaBloccoInferiore()
        {
            //Aggiornamento lunghezza inferiore
            if (lowerInternalNext)
            {
                var selectionList = new Dictionary<GameObject, Vector2>();
                if (lowerInternalNext.linkedBlocks.Count > 0)
                    lowerInternalNext.linkedBlocks.ForEach(s => selectionList.Add(s.gameObject, new Vector2(lowerInternalNext.gameObject.transform.position.x - s.gameObject.transform.position.x, lowerInternalNext.gameObject.transform.position.y - s.gameObject.transform.position.y)));
                selectionList.Add(lowerInternalNext.gameObject, new Vector2(0, 0));

                selectionList.Keys.ToList().ForEach(k =>
                {
                    k.transform.position = this.transform.position + new Vector3(nextBlockInternoInferioreOffsetX - selectionList[k].x, nextBlockInternoInferioreOffsetY - selectionList[k].y, 0);
                    k.transform.rotation = this.transform.rotation;
                });
            }
        }

        public void DiminuisciLunghezzaSuperiore(BlockWrapper next)
        {
            var lung = 0;
            BlockWrapper catena = next;
            while (catena)
            {
                catena.denti.ForEach(d => d.setNext -= AumentaLunghezzaSuperiore);
                Debug.Log("Invocation list di " + catena + ".setNext: " + catena.dente.setNext.GetInvocationList().Length);
                catena.denti.ForEach(d => d.unsetNext -= DiminuisciLunghezzaSuperiore);
                catena = catena.next;
            }
            catena = upperInternalNext;
            while (catena)
            {
                lung += catena.size;
                catena = catena.next;
            }
            firstStretchSize = lung;


            extendToMatchContent();
            RiposizionaBloccoInferiore();
        }

        public virtual void unsetNextInternoSuperiore(BlockWrapper next)
        {

            BlockWrapper catena = upperInternalNext;
            while (catena)
            {

                catena.denti.ForEach(d => d.setNext -= AumentaLunghezzaSuperiore);
                Debug.Log("Invocation list di " + catena + ".setNext: " + catena.dente.setNext.GetInvocationList().Length);
                catena.denti.ForEach(d => d.unsetNext -= DiminuisciLunghezzaSuperiore);
                catena = catena.next;
            }
            upperInternalNext.spazioDente.Receiving = true;
            upperInternalNext = null;
            block.UnsetFirstInnerNext();
            firstStretchSize = 0;

            extendToMatchContent();
            RiposizionaBloccoInferiore();
        }

        public virtual void setNextInternoInferiore(BlockWrapper candidateNext)
        {
            Debug.Log(testo + ".setNextInternoInferiore(" + candidateNext.testo + ")");


            moveUnderMe(candidateNext, new Vector3(nextBlockInternoInferioreOffsetX, 0, nextBlockInternoInferioreOffsetY), new Vector3(0, 0, nextBlockSpinZ));

            var oldNext = lowerInternalNext;
            lowerInternalNext = candidateNext;
            block.SecondInnerNext = candidateNext.block;
            lowerInternalNext.linkedBlocks.ForEach(AumentaLunghezzaInferiore);
            AumentaLunghezzaInferiore(lowerInternalNext);
            candidateNext.spazioDente.Receiving = false;
            denteInternoInferiore.Receiving = false;
            denteInternoInferiore.Receiving = true;
            if (oldNext)
            {
                lowerInternalNext.setNext(oldNext);
            }
        }

        public void AumentaLunghezzaInferiore(BlockWrapper next)
        {
            var lung = 0;
            BlockWrapper catena = lowerInternalNext;
            while (catena)
            {
                lung += catena.size;
                catena = catena.next;
            }
            secondStretchSize = lung;
            extendToMatchContent();
            next.denti.ForEach(d => d.setNext += AumentaLunghezzaInferiore);
            Debug.Log("Invocation list di " + next + ".setNext: " + next.dente.setNext.GetInvocationList().Length);
            next.denti.ForEach(d => d.unsetNext += DiminuisciLunghezzaInferiore);

        }

        public void DiminuisciLunghezzaInferiore(BlockWrapper next)
        {
            var lung = 0;
            BlockWrapper catena = next;
            while (catena)
            {
                catena.denti.ForEach(d => d.setNext -= AumentaLunghezzaInferiore);
                Debug.Log("Invocation list di " + catena + ".setNext: " + catena.dente.setNext.GetInvocationList().Length);
                catena.denti.ForEach(d => d.unsetNext -= DiminuisciLunghezzaInferiore);
                catena = catena.next;
            }
            catena = lowerInternalNext;
            while (catena)
            {
                lung += catena.size;
                catena = catena.next;
            }
            secondStretchSize = lung;

            extendToMatchContent();
        }

        public virtual void unsetNextInternoInferiore(BlockWrapper next)
        {

            BlockWrapper catena = lowerInternalNext;
            while (catena)
            {

                catena.denti.ForEach(d => d.setNext -= AumentaLunghezzaInferiore);
                Debug.Log("Invocation list di " + catena + ".setNext: " + catena.dente.setNext.GetInvocationList().Length);
                catena.denti.ForEach(d => d.unsetNext -= DiminuisciLunghezzaInferiore);
                catena = catena.next;
            }
            lowerInternalNext.spazioDente.Receiving = true;
            lowerInternalNext = null;
            block.UnsetSecondInnerNext();
            secondStretchSize = 0;
            extendToMatchContent();
        }


        public override List<BlockWrapper> directlyLinkedBlocks
        {
            get
            {
                var ex = new List<BlockWrapper>();
                ex.Add(next);
                ex.Add(upperInternalNext);
                ex.Add(lowerInternalNext);
                return ex;

            }
        }
        public override List<BlockWrapper> linkedBlocks
        {
            get
            {
                var ex = new List<BlockWrapper>();
                if (upperInternalNext)
                {
                    upperInternalNext.linkedBlocks.ForEach(ex.Add);
                    ex.Add(upperInternalNext);
                }
                if (lowerInternalNext)
                {
                    lowerInternalNext.linkedBlocks.ForEach(ex.Add);
                    ex.Add(lowerInternalNext);
                }
                if (next)
                {
                    next.linkedBlocks.ForEach(ex.Add);
                    ex.Add(next);
                }
                return ex;
            }
        }


        protected virtual void extendToMatchContent()
        {
            secondoCampoTesto.text = secondoTesto;
            if (firstStretchSize < 1)
                firstStretchSize = 1;
            if (secondStretchSize < 1)
                secondStretchSize = 1;

            List<int> verticesToStretch = new List<int>();
            List<int> verticesToStretchTwice = new List<int>();
            for (int i = 0; i < originaryVertices.Length; i++)
            {
                var vertex = originaryVertices[i];
                if (Mathf.RoundToInt(vertex.z) <= -3)
                    verticesToStretch.Add(i);
                if (Mathf.RoundToInt(vertex.z) <= -7)
                    verticesToStretchTwice.Add(i);
            }

            var levert = mesh.vertices;
            foreach (var i in verticesToStretch)
                levert[i] = new Vector3(levert[i].x, levert[i].y, originaryVertices[i].z - (firstStretchSize - 1) * 2);
            denteInternoInferiore.gameObject.transform.localPosition = new Vector3(denteInternoInferiore.gameObject.transform.localPosition.x, denteInternoInferiore.gameObject.transform.localPosition.y, 0.5f - 4 - firstStretchSize * 2);
            foreach (var i in verticesToStretchTwice)
                levert[i] = new Vector3(levert[i].x, levert[i].y, levert[i].z - (secondStretchSize - 1) * 2);
            dente.gameObject.transform.localPosition = new Vector3(dente.gameObject.transform.localPosition.x, dente.gameObject.transform.localPosition.y, 0.5f - 6 - (firstStretchSize + secondStretchSize) * 2);
            mesh.SetVertices(new List<Vector3>(levert));
   
            initialised = false;
            nextBlockOffsetY = -6 - firstStretchSize * 2 - secondStretchSize * 2;
            secondoCampoTesto.transform.parent.transform.localPosition = posizioneBaseSecondoCampoTesto + new Vector3(0, 0, -(firstStretchSize - 1) * 2);

            if (next)
            {
                // align next blocks
                var selectionList = new Dictionary<GameObject, Vector2>();
                next.linkedBlocks.ForEach(s => selectionList.Add(s.gameObject, new Vector2(next.gameObject.transform.position.x - s.gameObject.transform.position.x, next.gameObject.transform.position.y - s.gameObject.transform.position.y)));
                selectionList.Add(next.gameObject, new Vector2(0, 0));

                selectionList.Keys.ToList().ForEach(k =>
                {
                    k.transform.position = this.transform.position + new Vector3(nextBlockOffsetX - selectionList[k].x, nextBlockOffsetY - selectionList[k].y, 0);
                    k.transform.rotation = this.transform.rotation;
                });
            }
        }

        protected override int lunghezzaTesto
        {
            get
            {
                return Mathf.Max(campoTesto.text.Length, secondoCampoTesto.text.Length);
            }
        }

        private Vector3 posizioneBaseSecondoCampoTesto;

		public override void Init(ActorWrapper wrapper, bool autoinit = true)
        {
			denteInternoSuperiore = this.gameObject.transform.GetChild (3).gameObject.GetComponent<BlockWrapperCog> ();
			secondoCampoTesto = this.gameObject.transform.GetChild (4).GetChild(0).gameObject.GetComponent<UnityEngine.UI.Text> ();
			denteInternoInferiore = this.gameObject.transform.GetChild (5).gameObject.GetComponent<BlockWrapperCog> ();
            offsetTestoBaseX = 2;
			base.Init(wrapper, false);
			posizioneBaseSecondoCampoTesto = secondoCampoTesto.transform.parent.localPosition;
            denteInternoSuperiore.setNext = setNextInternoSuperiore;
            denteInternoSuperiore.unsetNext = unsetNextInternoSuperiore;
            denteInternoInferiore.setNext = setNextInternoInferiore;
            denteInternoInferiore.unsetNext = unsetNextInternoInferiore;
            extendToMatchContent();
            nextBlockOffsetY = -6 - firstStretchSize * 2 - secondStretchSize * 2;

            name = testo + " " + secondoTesto;
			initialised = autoinit;
        }

    }
}
