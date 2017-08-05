using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Closable : LaserSelectable {

    public event Action Closed;

    public override void SelectA()
    {
        if (Closed != null)
            Closed();
    }

}
