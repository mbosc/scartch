﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{

	public GameObject bucoVarSquare, bucoVarAng, bucoVarDD, bucoVarCrc;
	public Material bloccoMovimento, bloccoAspetto, bloccoSuono, bloccoSensori, bloccoOperatori, bloccoVariabili, bloccoControllo;
	public static ResourceManager Instance;
	public Material materialeSelezione;
	public Numpad numpad;
	public Keyboard keypad;
	public Boolpad boolpad;
	public GameObject varviewer;
	public MenuVoiceBlockPrefab[] motionPrototypes, lookPrototypes, soundPrototypes, controlPrototypes, sensingPrototypes, operatorsPrototypes;
	public Sprite singleicon, doubleicon, tripleicon, haticon, boolicon, numbicon, stringicon;
	public MenuVoiceBlockPrefab[][] prototypes;
    private SelectionMenu selectionMenu;

	private void Start ()
	{
		Instance = this;
		prototypes = new MenuVoiceBlockPrefab[6][];
		prototypes [0] = motionPrototypes;
		prototypes [1] = lookPrototypes;
		prototypes [2] = soundPrototypes;
		prototypes [3] = controlPrototypes;
		prototypes [4] = sensingPrototypes;
		prototypes [5] = operatorsPrototypes;
        selectionMenu = FindObjectOfType<SelectionMenu>();
    }
    private void OnDestroy()
    {
        model.Environment.Instance.PlayModeStarted -= mayHideMenu;
        model.Environment.Instance.PlayModeEnded -= mayShowMenu;
    }

    [Serializable]
	public class MenuVoiceBlockPrefab
	{
		public GameObject prefab;

		public virtual String Name { get { return prefab.name; } }

		public Sprite Icon;
	}

	[Serializable]
	public class MenuVoiceVariablePrefab : MenuVoiceBlockPrefab
	{
		private String name;
        public model.Reference reference;

        public override string Name {
			get {
				return name;
			}
		}

		public MenuVoiceVariablePrefab (model.Reference refe)
		{
            reference = refe;
			name = refe.Name;
			
            base.prefab = null;
			
			if (refe is model.NumberReference)
				base.Icon = ResourceManager.Instance.numbicon;
			else if (refe is model.StringReference)
				base.Icon = ResourceManager.Instance.stringicon;
			else if (refe is model.BooleanReference)
				base.Icon = ResourceManager.Instance.boolicon;

		}

    }


    private bool activebefore = false;

    private void mayHideMenu()
    {
        activebefore = selectionMenu.gameObject.activeInHierarchy;
        if (activebefore)
            selectionMenu.gameObject.SetActive(false);
    }
    private void mayShowMenu()
    {
        if (activebefore)
            selectionMenu.gameObject.SetActive(true);
    }


    bool lateInitialised = false; 
    private void Update()
    {
        if (!lateInitialised)
        {
            model.Environment.Instance.PlayModeStarted += mayHideMenu;
            model.Environment.Instance.PlayModeEnded += mayShowMenu;
            lateInitialised = true;
        }
        if (!model.Environment.Instance.PlayMode && GameObject.Find("LeftHand").GetComponent<NewtonVR.NVRHand>().Inputs[NewtonVR.NVRButtons.X].PressDown)
        {
            selectionMenu.gameObject.SetActive(!selectionMenu.gameObject.activeInHierarchy);
        }
    }
    
}