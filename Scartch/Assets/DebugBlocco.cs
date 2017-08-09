using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugBlocco : MonoBehaviour
{

    public GameObject dente, corpo;
    private int lung = 1, baseoffset = 1, lettersPerUnit = 4;
    public UnityEngine.UI.Text textBox;
    public string text;

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
            var mat = ScartchResourceManager.instance.BlockTypeMaterials[(Scripting.ScriptingType)color];
            corpo.GetComponent<Renderer>().material = mat;
            dente.GetComponent<Renderer>().material = mat;
            var deltawhite = Vector3.Distance(new Vector3(1, 1, 1), new Vector3(mat.color.r, mat.color.g, mat.color.b));
            var deltablack = Vector3.Distance(new Vector3(0, 0, 0), new Vector3(mat.color.r, mat.color.g, mat.color.b));
            textBox.color = deltawhite > deltablack ? Color.white : Color.black;
            Debug.Log("DW: " + deltawhite + "; DB: " + deltablack);
        }
    }
}
