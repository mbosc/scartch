using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DebugBlocco : MonoBehaviour
{

    public GameObject dente, corpo;
    private int lung = 1, baseoffset = 1, lettersPerUnit = 4;
    public UnityEngine.UI.Text textBox;
    public string text;

    private DebugBlocco nearest;
    public DebugBlocco Nearest
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
    public DebugBlocco next = null;
    public DebugBlocco Next
    {
        get { return next; }
        set
        {
            //unsubscribe old
            if (next != null)
                next.Grabbed -= Detach;

            //assign it
            next = value;

            //align it
            if (next != null)
            {
                next.transform.SetParent(this.transform);
                next.transform.localEulerAngles = Vector3.zero;
                next.transform.localPosition = new Vector3(0, -2 /** this.transform.localScale.x*/, 0);
                next.transform.SetParent(null);
            }

            //subscribe new
            if (next != null)
                next.Grabbed += Detach;
        }
    }

    private void Detach(object sender, EventArgs e)
    {
        this.Next = null;
    }

    public int Lung
    {
        get { return lung; }
        set
        {
            lung = Math.Max(1, value);
            AggiustaLunghezza(lung);
        }
    }

    private void AggiustaLunghezza(int value)
    {
        corpo.transform.localPosition = new Vector3(baseoffset + value, 0, 0);
        corpo.transform.localScale = new Vector3(2 * value, 2, 2);
    }


    private int color = 0;

    private void Update()
    {
        if (text != textBox.text)
        {
            Lung = 1 + (text.Length - 1) / lettersPerUnit;
            textBox.text = text;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            var r = color > 4 ? color = 0 : color++;
            var mat = ScartchResourceManager.instance.blockTypeMaterials[(Scripting.ScriptingType)color];
            corpo.GetComponent<Renderer>().material = mat;
            dente.GetComponent<Renderer>().material = mat;
            var deltawhite = Vector3.Distance(new Vector3(1, 1, 1), new Vector3(mat.color.r, mat.color.g, mat.color.b));
            var deltablack = Vector3.Distance(new Vector3(0, 0, 0), new Vector3(mat.color.r, mat.color.g, mat.color.b));
            textBox.color = deltawhite > deltablack ? Color.white : Color.black;
            Debug.Log("DW: " + deltawhite + "; DB: " + deltablack);
        }

        if (searchingNearest)
            Nearest = FindNearest();

    }


    public void Highlight(bool doing)
    {
        Material mat;
        if (doing)
            mat = ScartchResourceManager.instance.textBoxHighlighted;
        else
            mat = ScartchResourceManager.instance.textBoxNotHighlighted;
        corpo.GetComponent<Renderer>().material = mat;
        dente.GetComponent<Renderer>().material = mat;
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
                Nearest.Next = this;

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

    public void Ricapitola()
    {
        if (Next != null)
        {
            Next.Ricapitola();
            Next.transform.SetParent(this.transform);
        }
    }

    public void Estrinseca()
    {
        if (Next != null)
        {
            Next.Estrinseca();
            Next.transform.SetParent(null);

        }
    }


    public event System.EventHandler Grabbed;
    public void Grab()
    {
        if (Grabbed != null)
        {
            Grabbed(this, EventArgs.Empty);
        }
    }

    public float threshold = 2.2f;

    private DebugBlocco FindNearest()
    {
        var min = float.PositiveInfinity;
        DebugBlocco result = null;
        //coseno positivo

        var compatibleBlocchi = FindObjectsOfType<DebugBlocco>().ToList().Where(x => !x.Equals(this) && Mathf.Cos(Mathf.PI / 180 * Vector3.Angle(this.transform.up, x.transform.up)) > 0 &&
            Mathf.Abs(Vector3.Angle(-this.transform.up, (x.transform.position - this.transform.position).normalized)) > 90 && x.Next == null);
        Debug.DrawRay(this.transform.position, this.transform.up, Color.blue);

        //FindObjectsOfType<DebugBlocco>().ToList().ForEach(x => Debug.Log("Examining blocco " + x.gameObject.name + " angle " + Vector3.Angle(this.transform.up, x.transform.up) + "; cosine " + Mathf.Cos(Mathf.PI / 180 * Vector3.Angle(this.transform.up, x.transform.up))));
        compatibleBlocchi.ToList().ForEach(x => { if (Vector3.Distance(this.transform.position, x.transform.position) < min) { result = x; min = Vector3.Distance(this.transform.position, x.transform.position); } });
        compatibleBlocchi.ToList().ForEach(x => Debug.DrawRay(this.transform.position, (x.transform.position - this.transform.position).normalized, Color.white));
        //Debug.Log("Nearest: " + result.name + "; Dist: " + min);
        if (min < threshold * this.transform.localScale.x)
        {
            Debug.DrawRay(this.transform.position, (
                result.transform.position - this.transform.position).normalized, Color.red);
            return result;
        }
        else return null;
    }
}
