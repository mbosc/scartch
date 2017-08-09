using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
using Scripting;

public class ScartchResourceManager : MonoBehaviour
{
    public static ScartchResourceManager instance;

    // Use this for initialization
    void Start()
    {
        if (instance == null)
            instance = this;
        else Destroy(this.gameObject);

        BlockTypeMaterials = new Dictionary<Scripting.ScriptingType, Material>
        {
            { ScriptingType.control, controlMaterial },
            { ScriptingType.look, lookMaterial },
            { ScriptingType.movement, movementMaterial },
            { ScriptingType.sensor, sensorMaterial },
            { ScriptingType.sound, soundMaterial },
            { ScriptingType.variable, variableMaterial }
        };
    }

    [Header("VRTextboxes materials")]
    public Material textBoxHighlighted;
    public Material textBoxNotHighlighted;

    [Header("Scripting Type materials")]
    public Material controlMaterial;
    public Material lookMaterial, movementMaterial, sensorMaterial, soundMaterial, variableMaterial;
    public Dictionary<ScriptingType, Material> BlockTypeMaterials;

}
