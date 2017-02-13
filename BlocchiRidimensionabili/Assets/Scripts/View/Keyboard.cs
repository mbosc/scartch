using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : MonoBehaviour
{
    public Action<string> OutputChanged;
    public Action<string> InnerStringChanged;
    public Action<object> Confirmed;

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
        confirm.CharSelected += Condrimefd;
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

    private void Condrimefd()
    {
        if (Confirmed != null)
            Confirmed(this);
        this.gameObject.SetActive(false);
    }

    private void OnKeyPressed(char key)
    {
        Output += key;
    }

    private void EraseKey()
    {
        Output = Output.Substring(0, Output.Length - 1);
    }
    public void DetachAll()
    {
        if (Confirmed != null)
            Confirmed(this);
        OutputChanged = null;
        InnerStringChanged = null;
        Confirmed = null;
        output = "";
    }
}
