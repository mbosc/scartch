using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View.Resources;

namespace View
{
    public class EnvironmentViewer : MonoBehaviour
    {
        public VRButton changeModeButton, addActorButton, showTimerButton, listVarButton;
        private VariableWindow globalVariablesWindow;
        private TimerWindow timerWindow;
        public SpriteRenderer changeModeButtonSprite;

        public event System.Action AddedActor, ChangedMode;
        public event System.Action<int> RemovedVariable;
        public event System.Action<VariableEntry> AddedVariable;

        private void Start()
        {
            globalVariablesWindow = GameObject.Instantiate(ScartchResourceManager.instance.variableWindow);
            globalVariablesWindow.name = "Global Variables Window";
            timerWindow = GameObject.Instantiate(ScartchResourceManager.instance.timerWindow);
            timerWindow.name = "Global Timer Window";

            changeModeButton.Pressed += OnChangeModeButtonPressed;
            addActorButton.Pressed += OnAddActorButtonPressed;
            showTimerButton.Pressed += OnShowTimerButtonPressed;
            listVarButton.Pressed += OnListVarButtonPressed;
            globalVariablesWindow.VariableAdded += OnGlobalVariablesWindowVariableAdded;
            globalVariablesWindow.VariableRemoved += OnGlobalVariablesWindowVariableRemoved;

            globalVariablesWindow.Init(null);
            timerWindow.Init();

            globalVariablesWindow.Close();
            timerWindow.Close();
        }

        public void OnControllerModeChanged(bool obj)
        {
            if (!obj)
            {
                changeModeButton.GetComponent<Renderer>().material = ScartchResourceManager.instance.playButtonMaterial;
                changeModeButtonSprite.sprite = ScartchResourceManager.instance.playButtonSprite;
            }
            else
            {
                changeModeButton.GetComponent<Renderer>().material = ScartchResourceManager.instance.editButtonMaterial;
                changeModeButtonSprite.sprite = ScartchResourceManager.instance.editButtonSprite;
            }
        }

        private void OnGlobalVariablesWindowVariableRemoved(int obj)
        {
            if (RemovedVariable != null)
                RemovedVariable(obj);
        }

        private void OnGlobalVariablesWindowVariableAdded(VariableEntry entr)
        {
            if (AddedVariable != null)
                AddedVariable(entr);
        }

        private void OnDestroy()
        {
            changeModeButton.Pressed -= OnChangeModeButtonPressed;
            addActorButton.Pressed -= OnAddActorButtonPressed;
            showTimerButton.Pressed -= OnShowTimerButtonPressed;
            listVarButton.Pressed -= OnListVarButtonPressed;
        }

        private void OnListVarButtonPressed(object sender, System.EventArgs e)
        {
            if (globalVariablesWindow.Visible)
                globalVariablesWindow.Close();
            else
                globalVariablesWindow.Open();
        }

        private void OnShowTimerButtonPressed(object sender, System.EventArgs e)
        {
            if (timerWindow.Visible)
                timerWindow.Close();
            else
                timerWindow.Open();
        }

        private void OnAddActorButtonPressed(object sender, System.EventArgs e)
        {
            if (AddedActor != null)
                AddedActor();
        }

        private void OnChangeModeButtonPressed(object sender, System.EventArgs e)
        {
            if (ChangedMode != null)
                ChangedMode();
        }
    }
}
