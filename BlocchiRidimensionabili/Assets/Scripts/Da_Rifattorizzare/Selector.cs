using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using model;
using view;

public class Selector : MonoBehaviour
{

    public static Selector instance;
    // Use this for initialization
    protected virtual void Start()
    {
        instance = this;
    }

    public Selectable selected = null;
    public Selectable hovered = null;

    
    public static float zOffset = .25f;
    // Update is called once per frame
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
