using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {

    public GameObject bucoVarSquare, bucoVarAng, bucoVarDD, bucoVarCrc;
	public Material bloccoMovimento, bloccoAspetto, bloccoSuono, bloccoSensori, bloccoOperatori, bloccoVariabili, bloccoControllo;
    public static ResourceManager Instance;
    public Material materialeSelezione;

    private void Start()
    {
        Instance = this;
    }
}
