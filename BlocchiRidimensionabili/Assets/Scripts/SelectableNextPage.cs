using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableNextPage : LaserSelectable
{

    private SelectionMenu menu;

    public override void Select()
    {
        menu.NextScreen();
    }

    public void ScreenChanged(int i)
    {
        if (menu.HasNextScreen)
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
