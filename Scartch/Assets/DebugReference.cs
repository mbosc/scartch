using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DebugReference : MonoBehaviour {

    private GameObject testa, coda;
    private Dictionary<int, GameObject> corpo;
    private int lung = 1;
    public UnityEngine.UI.Text textBox;
    public string text;
    private Model.RefType type;
    private BoxCollider coll;

    private void Start()
    {
        corpo = new Dictionary<int, GameObject>();
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
        corpo.Values.ToList().ForEach(x => GameObject.Destroy(x));
        corpo = new Dictionary<int, GameObject>();
        if (coda != null) Destroy(coda);

        //istanzia nuovi elementi
        testa = GameObject.Instantiate(ScartchResourceManager.instance.referenceHeads[type]);
        coda = GameObject.Instantiate(ScartchResourceManager.instance.referenceHeads[type]);
        if (Lung > 2) Enumerable.Range(0, Lung - 2).ToList().ForEach(x => corpo.Add(x, GameObject.Instantiate(ScartchResourceManager.instance.referenceBody)));

        //imposta gerarchia
        testa.transform.SetParent(this.transform);
        coda.transform.SetParent(this.transform);
        corpo.Values.ToList().ForEach(x => x.transform.SetParent(this.transform));

        //posiziona correttamente
        testa.transform.localEulerAngles = ScartchResourceManager.instance.headRotation;
        testa.transform.localPosition = Vector3.zero;
        coda.transform.localEulerAngles = ScartchResourceManager.instance.tailRotation;
        coda.transform.localPosition = new Vector3(Lung/2.0f, 0, 0);
        corpo.ToList().ForEach(x => x.Value.transform.localPosition = new Vector3(0.5f + 0.5f * x.Key, 0, 0));
        corpo.Values.ToList().ForEach(x => x.transform.localEulerAngles = ScartchResourceManager.instance.bodyRotation);

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
            corpo.Values.ToList().ForEach(x => x.GetComponent<Renderer>().material = mat);
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
    }
}
