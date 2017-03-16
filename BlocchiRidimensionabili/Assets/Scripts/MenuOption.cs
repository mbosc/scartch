﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using view;

public class MenuOption : LaserSelectable {

    public UnityEngine.UI.Image icon;
    public UnityEngine.UI.Text text;
    private ResourceManager.blockPrefab contained;

    private void SetVisible(bool visible)
    {
        this.GetComponent<Renderer>().enabled = visible;
        this.GetComponent<Collider>().enabled = visible;
        icon.gameObject.SetActive(visible);
        text.gameObject.SetActive(visible);
    }

    public override void Select()
    {
        var actor = SelectableActor.selectedActor;
        if (actor == null)
            return;
        view.BlockWrapper c = contained.prefab.GetComponent<view.BlockWrapper>();
        
        if (contained.prefab.GetComponent<view.ReferenceWrapper>() != null)
        {
            if (contained.icon.Equals(ResourceManager.Instance.numbicon))
            {
                var reference = Instantiate(FindObjectOfType<Demo>().varcirc);
                reference.GetComponent<NumberReferenceWrapper>().Init(SelectableActor.selectedActor, new model.NumberVariable("three", 3));
            }
            //var reference = Instantiate(TIPO DI VARIABILE)
            //reference.GetComponent<TIPO DI VARIABILE>().Init(actor2, new model.MODELLO CORRISPONDENTE(NOME, VALORE));
        } else if (c != null)
        {
            object blockType = null;
            if (contained.icon.Equals(ResourceManager.Instance.singleicon))
            {
                blockType = FindObjectOfType<Demo>().block;
            } else if (contained.icon.Equals(ResourceManager.Instance.doubleicon))
            {
                blockType = FindObjectOfType<Demo>().mblock;
            }
            else if (contained.icon.Equals(ResourceManager.Instance.tripleicon))
            {
                blockType = FindObjectOfType<Demo>().mmblock;
            } else if (contained.icon.Equals(ResourceManager.Instance.haticon))
            {
                blockType = FindObjectOfType<Demo>().hat;
            }
            MethodInfo method = typeof(Demo).GetMethod("InstantiateWithComponent");
            MethodInfo generic = method.MakeGenericMethod(c.GetType());
            var block = (view.BlockWrapper) generic.Invoke(null, new object[] { blockType });
            
            block.Init(actor);
        }
        

    }

    public ResourceManager.blockPrefab Contained
    {
        get { return contained; }
        set
        {
            if (value == null)
                SetVisible(false);
            else
            {
                icon.sprite = value.icon;
                text.text = value.prefab.name;
                SetVisible(true);
            }
            contained = value;
        }
    }
}
