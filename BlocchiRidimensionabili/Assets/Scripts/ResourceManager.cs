using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {

    public GameObject bucoVarSquare, bucoVarAng, bucoVarDD, bucoVarCrc;
    public static ResourceManager Instance;
    private void Start()
    {
        Instance = this;
    }
}
