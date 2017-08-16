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
            timerWindow = GameObject.Instantiate(ScartchResourceManager.instance.timerWindow);

            changeModeButton.Pressed += ChangeModeButton_Pressed;
            addActorButton.Pressed += AddActorButton_Pressed;
            showTimerButton.Pressed += ShowTimerButton_Pressed;
            listVarButton.Pressed += ListVarButton_Pressed;
            globalVariablesWindow.VariableAdded += GlobalVariablesWindow_VariableAdded;
            globalVariablesWindow.VariableRemoved += GlobalVariablesWindow_VariableRemoved;

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

        private void GlobalVariablesWindow_VariableRemoved(int obj)
        {
            if (RemovedVariable != null)
                RemovedVariable(obj);
        }

        private void GlobalVariablesWindow_VariableAdded(VariableEntry entr)
        {
            if (AddedVariable != null)
                AddedVariable(entr);
        }

        private void OnDestroy()
        {
            changeModeButton.Pressed -= ChangeModeButton_Pressed;
            addActorButton.Pressed -= AddActorButton_Pressed;
            showTimerButton.Pressed -= ShowTimerButton_Pressed;
            listVarButton.Pressed -= ListVarButton_Pressed;
        }

        private void ListVarButton_Pressed(object sender, System.EventArgs e)
        {
            if (globalVariablesWindow.Visible)
                globalVariablesWindow.Close();
            else
                globalVariablesWindow.Open();
        }

        private void ShowTimerButton_Pressed(object sender, System.EventArgs e)
        {
            if (timerWindow.Visible)
                timerWindow.Close();
            else
                timerWindow.Open();
        }

        private void AddActorButton_Pressed(object sender, System.EventArgs e)
        {
            if (AddedActor != null)
                AddedActor();
        }

        private void ChangeModeButton_Pressed(object sender, System.EventArgs e)
        {
            if (ChangedMode != null)
                ChangedMode();
        }
    }
}
