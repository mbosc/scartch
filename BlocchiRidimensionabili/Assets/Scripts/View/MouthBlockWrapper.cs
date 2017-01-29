using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;
using System.Linq;

namespace view
{
    public class MouthBlockWrapper : BlockWrapper
    {
        // This property controls the access to its predecessor's block
        public new MouthBlock block
        {
            get { return base.block as MouthBlock; }
            set { base.block = value; }
        }
        public BlockWrapper internalNext;
        public BlockWrapperCog denteInterno;
        protected float nextBlockInternoOffsetX = 1;
        protected float nextBlockInternoOffsetY = -2;
        public override int size
        {
            get
            {
                return 2 + stretchSize;
            }
        }
        public override List<BlockWrapperCog> denti
        {
            get
            {
                var r = new List<BlockWrapperCog>();
                r.Add(dente);
                r.Add(denteInterno);
                return r;
            }
        }
        public virtual void setNextInterno(BlockWrapper candidateNext)
        {
            Debug.Log(testo + ".setNextInterno(" + candidateNext.testo + ")");


            var selectionList = new Dictionary<GameObject, Vector2>();
            candidateNext.linkedBlocks.ForEach(s => selectionList.Add(s.gameObject, new Vector2(candidateNext.gameObject.transform.position.x - s.gameObject.transform.position.x, candidateNext.gameObject.transform.position.y - s.gameObject.transform.position.y)));
            selectionList.Add(candidateNext.gameObject, new Vector2(0, 0));
            var selectionVariables = new Dictionary<GameObject, Vector2>();
            candidateNext.linkedVariables.ForEach(s => selectionVariables.Add(s.gameObject, new Vector2(candidateNext.gameObject.transform.position.x - s.gameObject.transform.position.x, candidateNext.gameObject.transform.position.y - s.gameObject.transform.position.y)));

            selectionList.Keys.ToList().ForEach(k =>
            {
                k.transform.position = this.transform.position + new Vector3(nextBlockInternoOffsetX - selectionList[k].x, nextBlockInternoOffsetY - selectionList[k].y, 0);
                k.transform.rotation = this.transform.rotation;
            });
            selectionVariables.Keys.ToList().ForEach(k =>
            {
                Debug.Log("Sposto " + k.name);
                k.transform.position = this.transform.position + new Vector3(nextBlockOffsetX - selectionVariables[k].x, nextBlockOffsetY - selectionVariables[k].y, 0);
            });

            var oldNext = internalNext;
            internalNext = candidateNext;
            block.InnerNext = candidateNext.block;

            internalNext.linkedBlocks.ForEach(AumentaLunghezza);
            AumentaLunghezza(internalNext);
            candidateNext.spazioDente.Receiving = false;
            denteInterno.Receiving = false;
            denteInterno.Receiving = true;
            if (oldNext)
            {
                internalNext.setNext(oldNext);
            }
        }
        public override List<BlockWrapper> directlyLinkedBlocks
        {
            get
            {

                var ex = new List<BlockWrapper>();
                ex.Add(next);
                ex.Add(internalNext);
                return ex;

            }
        }
        public void AumentaLunghezza(BlockWrapper next)
        {

            var lung = 0;
            BlockWrapper catena = internalNext;
            while (catena)
            {
                lung += catena.size;
                catena = catena.next;
            }
            stretchSize = lung;
            extendToMatchContent();
            next.denti.ForEach(d => d.setNext += AumentaLunghezza);
            Debug.Log("Invocation list di " + next + ".setNext: " + next.dente.setNext.GetInvocationList().Length);
            next.denti.ForEach(d => d.unsetNext += DiminuisciLunghezza);

        }

        public void DiminuisciLunghezza(BlockWrapper next)
        {
            var lung = 0;
            BlockWrapper catena = next;
            while (catena)
            {
                catena.denti.ForEach(d => d.setNext -= AumentaLunghezza);
                Debug.Log("Invocation list di " + catena + ".setNext: " + catena.dente.setNext.GetInvocationList().Length);
                catena.denti.ForEach(d => d.unsetNext -= DiminuisciLunghezza);
                catena = catena.next;
            }
            catena = internalNext;
            while (catena)
            {
                lung += catena.size;
                catena = catena.next;
            }
            stretchSize = lung;

            extendToMatchContent();
        }

        public override List<BlockWrapper> linkedBlocks
        {
            get
            {
                var ex = new List<BlockWrapper>();
                if (internalNext)
                {
                    internalNext.linkedBlocks.ForEach(ex.Add);
                    ex.Add(internalNext);
                }
                if (next)
                {
                    next.linkedBlocks.ForEach(ex.Add);
                    ex.Add(next);
                }
                return ex;
            }
        }

        public virtual void unsetNextInterno(BlockWrapper next)
        {

            BlockWrapper catena = internalNext;
            while (catena)
            {

                catena.denti.ForEach(d => d.setNext -= AumentaLunghezza);
                Debug.Log("Invocation list di " + catena + ".setNext: " + catena.dente.setNext.GetInvocationList().Length);
                catena.denti.ForEach(d => d.unsetNext -= DiminuisciLunghezza);
                catena = catena.next;
            }
            internalNext.spazioDente.Receiving = true;
            internalNext = null;
            stretchSize = 0;
            extendToMatchContent();
        }

        public int stretchSize = 1;

        protected virtual void extendToMatchContent()
        {

            if (stretchSize < 1)
                stretchSize = 1;

            List<int> verticesToStretch = new List<int>();
            for (int i = 0; i < originaryVertices.Length; i++)
            {
                var vertex = originaryVertices[i];
                if (Mathf.RoundToInt(vertex.z) <= -3)
                    verticesToStretch.Add(i);
            }

            var levert = mesh.vertices;
            foreach (var i in verticesToStretch)
                levert[i] = new Vector3(levert[i].x, levert[i].y, originaryVertices[i].z - (stretchSize - 1) * 2);
            dente.gameObject.transform.localPosition = new Vector3(dente.gameObject.transform.localPosition.x, dente.gameObject.transform.localPosition.y, 0.5f - 4 - stretchSize * 2);
            mesh.SetVertices(new List<Vector3>(levert));
            if (GetComponent<MeshCollider>())
                Destroy(GetComponent<MeshCollider>());
            initialised = false;
            nextBlockOffsetY = -4 - stretchSize * 2;

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

        protected override void Start()
        {
            offsetTestoBaseX = 2;
            base.Start();
            denteInterno.setNext = setNextInterno;
            denteInterno.unsetNext = unsetNextInterno;
            extendToMatchContent();
            nextBlockOffsetY = -4 - stretchSize * 2;

        }

    }
}