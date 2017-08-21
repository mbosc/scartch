using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
using Scripting;
using View.Resources;
using View;
using System.Linq;
using NewtonVR.Example;

public static class Vector3Extensions
{
    public static float SignedAngle(this Vector3 vec, Vector3 from, Vector3 to)
    {
        var angle = Vector3.Angle(from, to);
        var cross = Vector3.Cross(from, to);
        if (cross.y < 0) angle = -angle;
        return -angle;
    }
}

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
            { ScriptingType.operators, operatorsMaterial },
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
            new StartedPMHat(null, null, null, null, true),
            new WaitSecondsBlock(null, null, null, null, true),
            new PauseExecutionBlock(null, null, null, null, true),
            new BroadcastBlock(null, null, null, null, true),
            new ReceivedMessageHat(null, null, null, null, true),
            new StopPlayModeBlock(null, null, null, null, true)
        };
        lookElements = new List<ScriptingElement>
        {
            new SayBlock(null, null, null, null, true),
            new SayForSecondsBlock(null, null, null, null, true),
            new HideMessageBlock(null, null, null, null, true),
            new SetScaleToBlock(null, null, null, null, true),
            new SetScaleToPercentageBlock(null, null, null, null, true),
            new ModelChangeBlock(null, null, null, null, true),
            new ScaleReference(null, null, null, null, true)
        };
        movementElements = new List<ScriptingElement>
        {
            new TakeStepsBlock(null, null, null, null, true),
            new RotateBlock(null, null, null, null, true),
            new GoToBlock(null, null, null, null, true),
            new BounceOnBorderBlock(null, null, null, null, true),

            new AlterPositionBlock(null, null, null, null, true),
            new AlterRotationBlock(null, null, null, null, true),
            new SetPositionBlock(null, null, null, null, true),
            new SetRotationBlock(null, null, null, null, true),
            new XPositionReference(null, null, null, null, true),
            new YPositionReference(null, null, null, null, true),
            new ZPositionReference(null, null, null, null, true),
            new XRotationReference(null, null, null, null, true),
            new YRotationReference(null, null, null, null, true),
            new ZRotationReference(null, null, null, null, true)
        };
        sensorElements = new List<ScriptingElement>
        {
            new TimeResetBlock(null, null, null, null, true),
            new TimerReference(null, null, null, null, true),
            new ControllerPositionReference(null, null, null, null, true),
            new ControllerRotationReference(null, null, null, null, true),
            new HeadPositionReference(null, null, null, null, true),
            new HeadRotationReference(null, null, null, null, true),
            new ButtonReference(null, null, null, null, true),
            new ButtonPressedHat(null, null, null, null, true)
        };
        soundElements = new List<ScriptingElement>
        {
            new SetVolumeBlock(null, null, null, null, true),
            new PlaySoundBlock(null, null, null, null, true),
            new StopAllSoundsBlock(null, null, null, null, true),
            new VolumeReference(null, null, null, null, true)
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
        variableElements = new List<ScriptingElement>()
        {
            new SetBoolVarBlock(null, null, null, null, true),
            new SetNumberVarBlock(null, null, null, null, true),
            new SetStringVarBlock(null, null, null, null, true),
            new IncreaseNumberVarBlock(null, null, null, null, true)
        };

        scriptingElements = new List<List<ScriptingElement>>();
        scriptingElements.Add(controlElements);
        scriptingElements.Add(lookElements);
        scriptingElements.Add(movementElements);
        scriptingElements.Add(sensorElements);
        scriptingElements.Add(soundElements);
        scriptingElements.Add(operatorsElements);
        scriptingElements.Add(variableElements);

        windowSpawns = new List<List<Transform>>();
        windowSpawns.Add(windowSpawnsLev0);
        windowSpawns.Add(windowSpawnsLev1);
        levelIndexes = new List<int>();
        levelIndexes.Add(0);
        levelIndexes.Add(0);
    }

    [Header("VRTextboxes materials")]
    public Material textBoxHighlighted;
    public Material textBoxNotHighlighted;

    [Header("Scripting Type materials")]
    public Material controlMaterial;
    public Material lookMaterial, movementMaterial, sensorMaterial, soundMaterial, operatorsMaterial, variableMaterial;
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
    public Vector3 varWindowButtonPoint = new Vector3(0.03f, 0.036f, -0.785f);
    public Vector3 varWindowEntryPoint = new Vector3(-0.804f, 0f, -.59f);
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

    [Header("Sound clips")]
    public List<AudioClip> sounds;

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
    public List<Transform> windowSpawnsLev0;
    public List<Transform> windowSpawnsLev1;
    public List<Transform> blockSpawns;
    private List<List<Transform>> windowSpawns;

    private List<int> levelIndexes;
    public Transform GetWindowSpawn(int level)
    {
        var output = windowSpawns[level][levelIndexes[level]++];
        if (levelIndexes[level] >= windowSpawns[level].Count)
            levelIndexes[level] = 0;
        return output;
    }
    private int blockIndex = 0;
    public Transform GetBlockSpawn()
    {
        var output = blockSpawns[blockIndex++];
        if (blockIndex >= blockSpawns.Count)
            blockIndex = 0;
        return output;
    }

    [Header("Reference Prefabs")]
    public GameObject blockViewer;
    public GameObject mouthBlockViewer, doubleMouthBlockViewer, referenceViewer, hatViewer;
    

    [HideInInspector]
    public NVRLaserPointer lastRayCaster;
}
