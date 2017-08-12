using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
using Scripting;
using View.Resources;
using View;
using System.Linq;
using NewtonVR.Example;

public class ScartchResourceManager : MonoBehaviour
{
    public static ScartchResourceManager instance;

    // Use this for initialization
    void Start()
    {
        if (instance == null)
            instance = this;
        else Destroy(this.gameObject);

        blockTypeMaterials = new Dictionary<Scripting.ScriptingType, Material>
        {
            { ScriptingType.control, controlMaterial },
            { ScriptingType.look, lookMaterial },
            { ScriptingType.movement, movementMaterial },
            { ScriptingType.sensor, sensorMaterial },
            { ScriptingType.sound, soundMaterial },
            { ScriptingType.variable, variableMaterial }
        };

        referenceHeads = new Dictionary<RefType, GameObject>
        {
            { RefType.boolType, boolHead },
            { RefType.numberType, numberHead },
            { RefType.stringType, stringHead }
        };

        //TOBEREMOVED
        scriptingElements = new List<List<ScriptingElement>>();
        for (int i = 0; i < 7; i++)
            scriptingElements.Add(new List<ScriptingElement>());
    }

    [Header("VRTextboxes materials")]
    public Material textBoxHighlighted;
    public Material textBoxNotHighlighted;

    [Header("Scripting Type materials")]
    public Material controlMaterial;
    public Material lookMaterial, movementMaterial, sensorMaterial, soundMaterial, variableMaterial;
    public Dictionary<ScriptingType, Material> blockTypeMaterials;

    [Header("References components")]
    public GameObject referenceBody;
    public GameObject boolHead, stringHead, numberHead;
    public Dictionary<RefType, GameObject> referenceHeads;
    public Vector3 headRotation, tailRotation, bodyRotation;

    [Header("Snapping Dynamics")]
    public float referenceSnapThreshold = 1.0f;
    public float blockSnapThreshold = 2.2f;

    [Header("Comboboxes")]
    public VRComboboxVoice comboboxVoice;

    [Header("Windows Dimensions")]
    public VariableEntry entryPrototype;
    public float varWindowStep = 0.307f;
    public Vector3 varWindowButtonPoint = new Vector3(0.945f, 0.036f, -0.785f);
    public Vector3 varWindowEntryPoint = new Vector3(0.107f, 0.036f, -.785f);
    public Vector3 varWindowEntryScale = new Vector3(0.037f, 0.051f, 5.5f);

    [Header("Environment Buttons")]
    public Material editButtonMaterial;
    public Material playButtonMaterial;
    public Sprite editButtonSprite;
    public Sprite playButtonSprite;

    [Header("Models Sprites")]
    public List<Sprite> modelSprites;
    public List<GameObject> modelPrefabs;
    public GameObject actorSpawn;

    [Header("ScriptChooser")]
    public Sprite iconBlock;
    public Sprite iconMBlock, iconDMBlock, iconHat, iconBool, iconString, iconNum;
    public List<Material> pages;
    public List<List<ScriptingElement>> scriptingElements;
    
    [Header("Utility Windows")]
    public VariableWindow variableWindow;
    public TimerWindow timerWindow;
    public VariableMonitorWindow variableMonitorWindow;
    public ChooseModelWindow chooseModelWindow;
    public ChooseScriptingElementWindow chooseBlockWindow;
    public ActorViewer actorViewer;
    public ActorWindow actorWindow;

    [HideInInspector]
    public NVRLaserPointer lastRayCaster;
}
