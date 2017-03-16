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
    public blockPrefab[] motionPrototypes, lookPrototypes, soundPrototypes, controlPrototypes, sensingPrototypes, operatorsPrototypes, variablesPrototypes;
    public Sprite singleicon, doubleicon, tripleicon, haticon, boolicon, numbicon, stringicon;
    public blockPrefab[][] prototypes;

    private void Start()
    {
        Instance = this;
        prototypes = new blockPrefab[7][];
        prototypes[0] = motionPrototypes;
        prototypes[1] = lookPrototypes;
        prototypes[2] = soundPrototypes;
        prototypes[3] = controlPrototypes;
        prototypes[4] = sensingPrototypes;
        prototypes[5] = operatorsPrototypes;
        prototypes[6] = variablesPrototypes;
    }

    [Serializable]
    public class blockPrefab
    {
        public GameObject prefab;
        public Sprite icon;
    }
}
