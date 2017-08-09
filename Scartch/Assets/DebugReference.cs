using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DebugReference : MonoBehaviour
{

    private GameObject testa, coda, corpo;
    private int lung = 1;
    public UnityEngine.UI.Text textBox;
    public string text;
    private Model.RefType type;
    private BoxCollider coll;

    private void Start()
    {
        
        coll = gameObject.GetComponent<BoxCollider>();
    }

    public int Lung
    {
        get { return lung; }
        set
        {
            lung = Math.Max(2, value);
            AggiustaLunghezza();
        }
    }

    private void AggiustaLunghezza()
    {
        //distruggi vecchi elementi
        if (testa != null) Destroy(testa);
        if (corpo != null) Destroy(corpo);
        if (coda != null) Destroy(coda);

        //istanzia nuovi elementi
        testa = GameObject.Instantiate(ScartchResourceManager.instance.referenceHeads[type]);
        coda = GameObject.Instantiate(ScartchResourceManager.instance.referenceHeads[type]);
        corpo = GameObject.Instantiate(ScartchResourceManager.instance.referenceBody);

        //imposta gerarchia
        testa.transform.SetParent(this.transform);
        coda.transform.SetParent(this.transform);
        corpo.transform.SetParent(this.transform);

        //posiziona correttamente
        testa.transform.localEulerAngles = ScartchResourceManager.instance.headRotation;
        testa.transform.localPosition = Vector3.zero;
        testa.transform.localScale = new Vector3(1, 1, 1);
        coda.transform.localEulerAngles = ScartchResourceManager.instance.tailRotation;
        coda.transform.localPosition = new Vector3(Lung / 2.0f, 0, 0);
        coda.transform.localScale = new Vector3(1, 1, 1);
        corpo.transform.localEulerAngles = ScartchResourceManager.instance.bodyRotation;
        corpo.transform.localPosition = new Vector3(Lung / 4.0f, 0, 0);
        corpo.transform.localScale = new Vector3(Lung-2, 1, 1);

        //ridefinisci bound del collider
        coll.center = new Vector3(.25f * Lung, 0, -.25f);
        coll.size = new Vector3(.5f * Lung, 2, .5f);
    }


    private int color = 0;

    private void Update()
    {
        if (text != textBox.text)
        {
            Lung = text.Length;
            textBox.text = text;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            var r = color > 4 ? color = 0 : color++;
            var mat = ScartchResourceManager.instance.blockTypeMaterials[(Scripting.ScriptingType)color];
            corpo.GetComponent<Renderer>().material = mat;
            testa.GetComponent<Renderer>().material = mat;
            coda.GetComponent<Renderer>().material = mat;
            var deltawhite = Vector3.Distance(new Vector3(1, 1, 1), new Vector3(mat.color.r, mat.color.g, mat.color.b));
            var deltablack = Vector3.Distance(new Vector3(0, 0, 0), new Vector3(mat.color.r, mat.color.g, mat.color.b));
            textBox.color = deltawhite > deltablack ? Color.white : Color.black;
            Debug.Log("DW: " + deltawhite + "; DB: " + deltablack);


        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (type == Model.RefType.stringType)
                type = 0;
            else
                type++;
            AggiustaLunghezza();
        }

        if (searchingNearest)
            Nearest = FindNearest();
    }

        private DebugReferenceSlot nearest;
    public DebugReferenceSlot Nearest
    {
        get { return nearest; }
        set
        {
            if (nearest != null)
                nearest.Highlight(false);
            nearest = value;
            if (nearest != null)
                nearest.Highlight(true);
        }
    }

    

    private bool searchingNearest = false;
    public bool SearchingNearest
    {
        get { return searchingNearest; }
        set
        {
            searchingNearest = value;
            if (!value && Nearest != null)
            {
                Nearest.Filler = this;

                Nearest = null;
            }
        }
    }
    public void StartSearchingNearest()
    {
        SearchingNearest = true;
    }
    public void StopSearchingNearest()
    {
        SearchingNearest = false;

    }

    public event System.EventHandler Grabbed;
    public void Grab()
    {
        if (Grabbed != null)
        {
            Grabbed(this, EventArgs.Empty);
        }
    }

    

    private DebugReferenceSlot FindNearest()
    {
        var min = float.PositiveInfinity;
        DebugReferenceSlot result = null;
        //coseno positivo

        var compatibleBlocchi = FindObjectsOfType<DebugReferenceSlot>().ToList().Where(x => x.Filler == null);

        compatibleBlocchi.ToList().ForEach(x => { if (Vector3.Distance(this.transform.position, x.transform.position + x.transform.right - x.transform.forward*0.5f) < min) { result = x; min = Vector3.Distance(this.transform.position, x.transform.position); } });
        
        if (min < ScartchResourceManager.instance.referenceSnapThreshold * this.transform.localScale.x)
            return result;
        
        else return null;
    }
}
