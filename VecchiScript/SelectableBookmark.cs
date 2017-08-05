using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableBookmark : LaserSelectable {

    private SelectionMenu menu;
    public int page;

    public override void SelectA()
    {
        menu.ChangePage(page);
    }

    void Start () {
        menu = GetComponentInParent<SelectionMenu>();
	}
	
	
}
