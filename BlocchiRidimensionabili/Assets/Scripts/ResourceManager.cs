using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceManager : MonoBehaviour {

    public GameObject bucoVarSquare, bucoVarAng, bucoVarDD, bucoVarCrc;
	public Material bloccoMovimento, bloccoAspetto, bloccoSuono, bloccoSensori, bloccoOperatori, bloccoVariabili, bloccoControllo;
    public static ResourceManager Instance;
    public Material materialeSelezione;
    public Numpad numpad;
    public Keyboard keypad;
    public Boolpad boolpad;
    public GameObject varviewer;
    public blockPrefab[] prototypes;
    public Sprite singleicon, doubleicon, tripleicon;

    private void Start()
    {
        Instance = this;
    }

    [Serializable]
    public class blockPrefab
    {
        public GameObject prefab;
        public Sprite icon;
    }
}
