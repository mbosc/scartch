using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardKey : LaserSelectable {

    public bool ignoreText = false;
    public char mychar;
    private UnityEngine.UI.Text text;

    public override void Select()
    {
        if (CharSelected != null)
            CharSelected(mychar);
    }

    public virtual void Init()
    {
        if (!ignoreText)
        {
            text = transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>();
            text.text = mychar + "";
        }
    }

    public virtual void Init(char c)
    {
        mychar = c;
        Init();
    }

    public event Action<char> CharSelected;
}
