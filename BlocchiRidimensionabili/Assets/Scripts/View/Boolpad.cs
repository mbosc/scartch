using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boolpad : MonoBehaviour {

    public Action<bool> OutputChanged;
    public Action<string> InnerStringChanged;
    public KeyboardKey trueKey, falseKey;
    public SpecialKey confirm, cancel;
    private bool output;
    public bool Output { get { return output; } set { output = value; if (OutputChanged != null)
                OutputChanged(Output);
            InnerString = value.ToString();
        } }
    private string innerString;
    public string InnerString
    {
        get { return innerString; }
        set
        {
        

                innerString = value;
                if (InnerString != null)
                    InnerStringChanged(InnerString);

            
        }
    }
    // Use this for initialization
    void Start () {
        trueKey.CharSelected += KeyPressed;
        falseKey.CharSelected += KeyPressed;
        cancel.CharSelected += EraseKey;
        confirm.CharSelected += Confirmed;
    }

    private void KeyPressed(char obj)
    {
        if (obj == 't')
            Output = true;
        else
            Output = false;
    }

    private void Confirmed()
    {

    }
    private void EraseKey()
    {
        Output = false;
        InnerString = "";
    }
}
