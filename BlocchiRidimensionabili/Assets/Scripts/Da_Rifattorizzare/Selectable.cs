﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using model;
using view;
using NewtonVR;

public class Selectable : MonoBehaviour
{
    
    protected NVRInteractable interactable;

    // Use this for initialization
    protected virtual void Start()
    {
        interactable = this.gameObject.AddComponent<NVRInteractableItem>();
        interactable.EnableGravityOnDetach = false;
        interactable.EnableKinematicOnDetach = true;
        interactable.DisableKinematicOnAttach = true;
        interactable.Selected += OnSelection;
        interactable.Unselected += OnDeselection;
        interactable.StaySelected += OnStaySelected;
    }

    //private void PointerOver(object sender, PointerEventArgs e)
    //{
    //    if (e.target.Equals(this.transform))
    //    {
    //        selector.hovered = this;
    //        OnMouseOverActions();
    //    }
    //}

    //private void PointerExit(object sender, PointerEventArgs e)
    //{
    //    //       if (selector.hovered == this)
    //    //           selector.hovered = null;
    //}

    private void OnDestroy()
    {
        interactable.Selected -= OnSelection;
        interactable.Unselected -= OnDeselection;
        interactable.StaySelected -= OnStaySelected;
        Destroy(interactable);
    }

    //  protected virtual void OnMouseOver()
    //  {
    //      selector.hovered = this;
    //OnMouseOverActions ();
    //  }

    //  protected virtual void OnMouseExit()
    //  {

    //      if (selector.hovered == this)
    //          selector.hovered = null;
    //  }

    public virtual void OnSelection()
    {

    }
    public virtual void OnDeselection()
    {

    }
    public virtual void OnStaySelected()
    {

    }
}
