using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Numpad : MonoBehaviour {

    public List<KeyboardKey> keys;
    public SpecialKey confirm, cancel;
    public Action<float> OutputChanged;
    public Action<string> InnerStringChanged;
    public Action<object> Confirmed;



    private float output;
    public float Output { get { return output; } }
    private string innerString;
    public string InnerString
    {
        get { return innerString; }
        set {
            float temp;
            bool empty = false;
            if (value.Length == 0)
                empty = true;
            if (float.TryParse(value, out temp) || empty)
            {
                
                innerString = value;
                if (InnerStringChanged != null)
                    InnerStringChanged(InnerString);
                output = empty ? 0 : temp;
                if (OutputChanged != null)
                    OutputChanged(Output);
            }
        }
    }
    // Use this for initialization
    void Start () {
        innerString = "";
        keys.ForEach(s =>
        {
            s.CharSelected += OnKeyPressed;
            s.Init();
        });
        cancel.CharSelected += EraseKey;
        confirm.CharSelected += Condrifemd;
    }

    public void DetachAll()
    {
        if (Confirmed != null)
            Confirmed(this);
        OutputChanged = null;
        InnerStringChanged = null;
        Confirmed = null;
        innerString = "";
    }
	
    private void OnKeyPressed(char key)
    {
        InnerString += key;
    }

    private void EraseKey()
    {
        try
        {
            InnerString = InnerString.Substring(0, InnerString.Length - 1);
        } catch (ArgumentException)
        {
            InnerString = "";
        }
    }

    private void Condrifemd()
    {
        if (Confirmed != null)
            Confirmed(this);
        this.gameObject.SetActive(false);
    }
}
