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

        private bool inPlayMode = false;

        public event System.Action AddedVariable, AddedActor, ChangedMode;
        public event System.Action<int> RemovedVariable;

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

        private void GlobalVariablesWindow_VariableRemoved(int obj)
        {
            if (RemovedVariable != null)
                RemovedVariable(obj);
        }

        private void GlobalVariablesWindow_VariableAdded()
        {
            if (AddedVariable != null)
                AddedVariable();
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
            inPlayMode = !inPlayMode;
            if (inPlayMode)
            {
                changeModeButton.GetComponent<Renderer>().material = ScartchResourceManager.instance.playButtonMaterial;
                changeModeButtonSprite.sprite = ScartchResourceManager.instance.playButtonSprite;
            }
            else
            {
                changeModeButton.GetComponent<Renderer>().material = ScartchResourceManager.instance.editButtonMaterial;
                changeModeButtonSprite.sprite = ScartchResourceManager.instance.editButtonSprite;
            }

            if (ChangedMode != null)
                ChangedMode();
        }
    }
}
