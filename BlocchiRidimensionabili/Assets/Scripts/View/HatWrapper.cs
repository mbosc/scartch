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

            var selectionList = new Dictionary<GameObject, Vector2>();
            candidateNext.linkedBlocks.ForEach(s => selectionList.Add(s.gameObject, new Vector2(candidateNext.gameObject.transform.position.x - s.gameObject.transform.position.x, candidateNext.gameObject.transform.position.y - s.gameObject.transform.position.y)));
            selectionList.Add(candidateNext.gameObject, new Vector2(0, 0));
            var selectionVariables = new Dictionary<GameObject, Vector2>();
            candidateNext.linkedVariables.ForEach(s => selectionVariables.Add(s.gameObject, new Vector2(candidateNext.gameObject.transform.position.x - s.gameObject.transform.position.x, candidateNext.gameObject.transform.position.y - s.gameObject.transform.position.y)));

            selectionList.Keys.ToList().ForEach(k =>
                {
                    k.transform.position = this.transform.position + new Vector3(nextBlockOffsetX - selectionList[k].x, nextBlockOffsetY - selectionList[k].y, 0);
                    k.transform.eulerAngles = this.transform.eulerAngles + new Vector3(0, 0, 180);
                });
            selectionVariables.Keys.ToList().ForEach(k =>
            {
                Debug.Log("Sposto " + k.name);
                k.transform.position = this.transform.position + new Vector3(nextBlockOffsetX - selectionVariables[k].x, nextBlockOffsetY - selectionVariables[k].y, 0);
            });

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
        
		public override void Init(ActorWrapper wrapper, bool autoinit = true){
			
			this.Owner = wrapper;
				try {
					campoTesto = transform.GetChild (0).GetChild (0).GetComponent<UnityEngine.UI.Text> ();
					dente = transform.GetChild (1).GetComponent<BlockWrapperCog> ();
				} catch {
					Debug.Log ("Problemi inizializzazione per " + name);
				}
				dente.setNext = setNext;
				dente.unsetNext = unsetNext;
				name = testo;
				campoTesto.text = testo;
				if (lastBlock)
					evaluateLastBlock ();
				nextBlockOffsetX = 2;
				offsetTestoBaseX = 0;
				initialised = autoinit;
			}
		}
}