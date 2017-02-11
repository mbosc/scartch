using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : MonoBehaviour
{

    public List<KeyboardKey> keys;
    public SpecialKey confirm, cancel, shift;
    private bool shifted = false;
    private string output = "";
    public string Output
    {
        get { return output; }
        set
        {
            output = value;

            if (OutputChanged != null)
                OutputChanged(Output);
        }
    }
    public Action<string> OutputChanged;

    protected void Start()
    {
        shifted = false;
        keys.ForEach(s =>
        {
            s.mychar = char.ToLower(s.mychar);
            s.CharSelected += OnKeyPressed;
            s.Init();
        });
        cancel.CharSelected += EraseKey;
        confirm.CharSelected += Confirmed;
        shift.CharSelected += ShiftKeys;
    }

    private void ShiftKeys()
    {
        shifted = !shifted;
        keys.ForEach(s =>
        {
            s.mychar = shifted ? char.ToUpper(s.mychar) : char.ToLower(s.mychar);
            s.Init();
        });
    }

    private void Confirmed()
    {

    }

    private void OnKeyPressed(char key)
    {
        Output += key;
    }

    private void EraseKey()
    {
        Output = Output.Substring(0, Output.Length - 1);
    }

}
