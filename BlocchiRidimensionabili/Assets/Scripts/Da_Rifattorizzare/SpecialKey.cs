using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialKey : LaserSelectable {


    public event Action CharSelected;

    public override void Select()
    {
        if (CharSelected != null)
            CharSelected();
    }
}
