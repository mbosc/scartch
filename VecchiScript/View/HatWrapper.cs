using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using model;

namespace view
{
    // TODO valutare se questa derivazione sia sconveniente
    public class HatWrapper : BlockWrapper
    {
        public Hat hat;
        public override List<BlockWrapperCog> denti
        {
            get
            {
                var r = new List<BlockWrapperCog>();
                r.Add(dente);
                return r;
            }
        }

        public override string ToString()
        {
            return testo;
        }

        public override void setNext(BlockWrapper candidateNext)
        {
            Debug.Log(testo + ".setNext(" + candidateNext.testo + ")");

            moveUnderMe(candidateNext, new Vector3(nextBlockOffsetX, 0, nextBlockOffsetY), new Vector3(0, 0, nextBlockSpinZ));

            var oldNext = next;
            next = candidateNext;
            hat.Next = candidateNext.block;

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

        public override void unsetNext(BlockWrapper thisBlocco)
        {
            Debug.Log(testo + ".unsetNext()");
            if (next)
            {
                next.spazioDente.Receiving = true;
                hat.UnsetNext();
            }
            next = null;
        }

        protected override void Update()
        {
            if (!initialised)
                return;
        }

        public override void Init(ActorWrapper wrapper, bool autoinit = true)
        {
            this.Owner = wrapper;
            try
            {
                campoTesto = transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>();
                dente = transform.GetChild(1).GetComponent<BlockWrapperCog>();
            }
            catch
            {
                Debug.Log("Problemi inizializzazione per " + name);
            }
            dente.setNext = setNext;
            dente.unsetNext = unsetNext;
            name = testo;
            campoTesto.text = testo;
            if (lastBlock)
                evaluateLastBlock();
            nextBlockOffsetX = -2;
            nextBlockSpinZ = 180;
            offsetTestoBaseX = 0;
            initialised = autoinit;
        }
    }
}