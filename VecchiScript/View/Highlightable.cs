using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlightable : MonoBehaviour {

    public Material original, highlight;
	// Use this for initialization
	void Awake () {
        original = GetComponent<Renderer>().material;
        
	}
    private void Start()
    {
        highlight = ResourceManager.Instance.materialeSelezione;
    }
    public void Highlight(bool doing)
    {
        GetComponent<Renderer>().material = doing ? highlight : original;
    }
}
