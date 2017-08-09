using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugReferenceSlot : MonoBehaviour {
    private DebugReference filler;
    public List<GameObject> scomparsa;
    public DebugReference Filler { get { return filler; } set
        {
            //unsubscribe old
            if (Filler != null)
                Filler.Grabbed -= Detach;

            //assign it
            filler = value;

            //align it
            if (filler != null)
            {
                //QUESTO VA RIVISTO
                filler.transform.SetParent(this.transform);
                filler.transform.localEulerAngles = Vector3.zero;
                filler.transform.localPosition = new Vector3(0, 0, 0);
                filler.transform.SetParent(null);
            }

            //subscribe new
            if (filler != null)
                filler.Grabbed += Detach;

            scomparsa.ForEach(x => x.SetActive(value == null));
        }
    }

    private void Detach(object sender, EventArgs e)
    {
        Filler = null;
    }
    public GameObject debugray;
    private void Start()
    {
        debugray.transform.localScale = transform.localScale.x * new Vector3(ScartchResourceManager.instance.referenceSnapThreshold, ScartchResourceManager.instance.referenceSnapThreshold, ScartchResourceManager.instance.referenceSnapThreshold);
    }
    public void Highlight(bool doing)
    {
        Material mat;
        if (doing)
            mat = ScartchResourceManager.instance.textBoxHighlighted;
        else
            mat = ScartchResourceManager.instance.textBoxNotHighlighted;
        scomparsa.ForEach(x =>x.GetComponent<Renderer>().material = mat);
    }
}
