using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        GameObject.Instantiate(contained.prefab, this.transform.position, Quaternion.identity);
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
