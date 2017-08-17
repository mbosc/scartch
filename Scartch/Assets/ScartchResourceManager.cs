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

        controlElements = new List<ScriptingElement>
        {
            new IfBlock(null, null, null, null, true),
            new WhileBlock(null, null, null, null, true),
            new ForeverBlock(null, null, null, null, true),
            new RepeatBlock(null, null, null, null, true),
            new IfElseBlock(null, null, null, null, true),
            new UntilBlock(null, null, null, null, true),
            new WaitUntilBlock(null, null, null, null, true),
            new StartedPMBlock(null, null, null, null, true),
            new WaitSecondsBlock(null, null, null, null, true)
        };
        lookElements = new List<ScriptingElement>
        {
            new SayBlock(null, null, null, null, true)
        };
        movementElements = new List<ScriptingElement>
        {
            new TakeStepsBlock(null, null, null, null, true),
            new RotateBlock(null, null, null, null, true),
            new GoToBlock(null, null, null, null, true),
            new BounceOnBorderBlock(null, null, null, null, true)
        };
        sensorElements = new List<ScriptingElement>
        {
            new TimeResetBlock(null, null, null, null, true)
        };
        soundElements = new List<ScriptingElement>
        {
            new SetVolumeBlock(null, null, null, null, true)
        };
        operatorsElements = new List<ScriptingElement>()
        {
            new SumExpression(null, null, null, null, true),
            new MinusExpression(null, null, null, null, true),
            new TimesExpression(null, null, null, null, true),
            new DivExpression(null, null, null, null, true),
            new GTExpression(null, null, null, null, true),
            new LTExpression(null, null, null, null, true),
            new GEExpression(null, null, null, null, true),
            new LEExpression(null, null, null, null, true),
            new EQExpression(null, null, null, null, true),
            new ANDExpression(null, null, null, null, true),
            new ORExpression(null, null, null, null, true),
            new NOTExpression(null, null, null, null, true),
            new RandomNumExpression(null, null, null, null, true),
            new STREQExpression(null, null, null, null, true),
            new STRCATExpression(null, null, null, null, true),
            new STRLENExpression(null, null, null, null, true),
            new MATHFOPSExpression(null, null, null, null, true),
            new ModExpression(null, null, null, null, true),
            new RoundExpression(null, null, null, null, true)
        };
        variableElements = new List<ScriptingElement>();

        scriptingElements = new List<List<ScriptingElement>>();
        scriptingElements.Add(controlElements);
        scriptingElements.Add(lookElements);
        scriptingElements.Add(movementElements);
        scriptingElements.Add(sensorElements);
        scriptingElements.Add(soundElements);
        scriptingElements.Add(operatorsElements);
        scriptingElements.Add(variableElements);
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
    public GameObject referenceSlotViewer;

    [Header("Snapping Dynamics")]
    public float referenceSnapThreshold = 1.0f;
    public float blockSnapThreshold = 2.2f;

    [Header("Comboboxes")]
    public VRComboboxVoice comboboxVoice;
    public VRCombobox combobox;

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
    public List<ScriptingElement> controlElements, lookElements, movementElements, sensorElements, variableElements, operatorsElements, soundElements;
    
    [Header("Utility Windows")]
    public VariableWindow variableWindow;
    public TimerWindow timerWindow;
    public VariableMonitorWindow variableMonitorWindow;
    public ChooseModelWindow chooseModelWindow;
    public ChooseScriptingElementWindow chooseBlockWindow;
    public ActorViewer actorViewer;
    public ActorWindow actorWindow;
    public Transform windowSpawn;

    [Header("Reference Prefabs")]
    public GameObject blockViewer;
    public GameObject mouthBlockViewer, doubleMouthBlockViewer, referenceViewer, hatViewer;
    
    [HideInInspector]
    public NVRLaserPointer lastRayCaster;
}
