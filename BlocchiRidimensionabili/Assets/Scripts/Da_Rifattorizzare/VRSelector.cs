using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using model;
using view;

public class VRSelector : Selector
{
    protected override void Start()
    {
        Debug.Log("Start " + name);

        zOffset = 0;
        instance = this;
    }
    


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && hovered && selected == null)
        {
            selected = hovered;
			selected.OnSelection ();
        }



        if (Input.GetMouseButtonUp(0) && selected)
        {
            var oldSelected = selected;
            selected = null;
			oldSelected.OnDeselection ();

        }

        if (selected)
        {
			selected.OnStaySelected ();
        }
    }
}
