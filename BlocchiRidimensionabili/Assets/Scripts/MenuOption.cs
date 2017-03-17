using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using view;

public class MenuOption : LaserSelectable
{

    public UnityEngine.UI.Image icon;
    public UnityEngine.UI.Text text;
    private ResourceManager.MenuVoiceBlockPrefab contained;

    private void SetVisible(bool visible)
    {
        this.GetComponent<Renderer>().enabled = visible;
        this.GetComponent<Collider>().enabled = visible;
        icon.gameObject.SetActive(visible);
        text.gameObject.SetActive(visible);
    }

    public override void SelectA()
    {
        var actor = SelectableActor.selectedActor;
        if (actor == null)
            return;
        bool isBlock = contained.prefab != null;

        if (!isBlock)
        {
            var w = contained as ResourceManager.MenuVoiceVariablePrefab;
            if (contained.Icon.Equals(ResourceManager.Instance.numbicon))
            {
                var reference = Instantiate(FindObjectOfType<Demo>().varcirc);
                reference.GetComponent<NumberReferenceWrapper>().Init(SelectableActor.selectedActor, w.reference);
            }
            else if (contained.Icon.Equals(ResourceManager.Instance.boolicon))
            {
                var reference = Instantiate(FindObjectOfType<Demo>().varAng);
                reference.GetComponent<BooleanReferenceWrapper>().Init(SelectableActor.selectedActor, w.reference);
            }
            else if (contained.Icon.Equals(ResourceManager.Instance.stringicon))
            {
                var reference = Instantiate(FindObjectOfType<Demo>().varSqr);
                reference.GetComponent<StringReferenceWrapper>().Init(SelectableActor.selectedActor, w.reference);
            }
        }
        else
        {
            view.BlockWrapper c = contained.prefab.GetComponent<view.BlockWrapper>();
            object blockType = null;
            if (contained.Icon.Equals(ResourceManager.Instance.singleicon))
            {
                blockType = FindObjectOfType<Demo>().block;
            }
            else if (contained.Icon.Equals(ResourceManager.Instance.doubleicon))
            {
                blockType = FindObjectOfType<Demo>().mblock;
            }
            else if (contained.Icon.Equals(ResourceManager.Instance.tripleicon))
            {
                blockType = FindObjectOfType<Demo>().mmblock;
            }
            else if (contained.Icon.Equals(ResourceManager.Instance.haticon))
            {
                blockType = FindObjectOfType<Demo>().hat;
            }
            MethodInfo method = typeof(Demo).GetMethod("InstantiateWithComponent");
            MethodInfo generic = method.MakeGenericMethod(c.GetType());
            var block = (view.BlockWrapper)generic.Invoke(null, new object[] { blockType });

            block.Init(actor);
        }


    }

    public ResourceManager.MenuVoiceBlockPrefab Contained
    {
        get { return contained; }
        set
        {
            if (value == null)
                SetVisible(false);
            else
            {
                icon.sprite = value.Icon;
                text.text = value.Name;
                SetVisible(true);
            }
            contained = value;
        }
    }
}
