using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    public class BlockAttachPoint : MonoBehaviour
    {
        public event System.Action<BlockViewer> Attached;
        public event System.Action Detached;
        public List<GameObject> highlightElements;

        private void Start()
        {
            Free = true;
            this.transform.localScale = Vector3.one / 2;
        }

        public void Highlight(bool doing)
        {
            highlightElements.ForEach(x => x.SetActive(doing));
        }

        public bool Free { get; private set; }

        public void Attach(BlockViewer bl)
        {
            Free = false;
            if (Attached != null)
                Attached(bl);
        }
        public void Detach()
        {
            Free = true;
            if (Detached != null)
                Detached();
        }
    }
}
