using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectablePreviousPage : LaserSelectable
{

    private SelectionMenu menu;

    public override void Select()
    {
        menu.PrevScreen();
    }

    public void ScreenChanged(int i)
    {
        if (menu.HasPreviousScreen)
        {
            this.GetComponent<Renderer>().enabled = true;
            this.GetComponent<Collider>().enabled = true;
        }
        else
        {
            this.GetComponent<Renderer>().enabled = false;
            this.GetComponent<Collider>().enabled = false;
        }
    }

    void Start()
    {
        menu = GetComponentInParent<SelectionMenu>();
        menu.screenChanged += ScreenChanged;
    }

    private void OnDestroy()
    {
        menu.screenChanged -= ScreenChanged;
    }
}
