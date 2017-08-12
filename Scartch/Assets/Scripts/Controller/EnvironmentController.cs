using System.Collections;
using System.Collections.Generic;
using System;
using Model;
using UnityEngine;
using View;

namespace Controller
{
    public class EnvironmentController
    {
        private List<ActorController> actors;
        private List<VariableController> globalVariables;
        private EnvironmentViewer viewer;

        private EnvironmentController(EnvironmentViewer viewer)
        {
            actors = new List<ActorController>();
            globalVariables = new List<VariableController>();

            viewer.AddedActor += AddActor;
            viewer.AddedVariable += AddGlobalVariable;
            viewer.ChangedMode += ChangeMode;
            viewer.RemovedVariable += RemoveGlobalVariable;
        }

        private static EnvironmentController instance;

        public static EnvironmentController Instance
        {
            get
            {
                return instance ?? (instance = new EnvironmentController(GameObject.FindObjectOfType<EnvironmentViewer>()));
            }
        }

        public void AddGlobalVariable(VariableEntry entr)
        {
            globalVariables.Add(VariableController.AddVariable(entr, null));
        }

        public void AddActor()
        {
            Actor act = new Actor()
            {
                Name = "ACT"
            };
            ActorViewer vie = GameObject.Instantiate(ScartchResourceManager.instance.actorViewer);
            ActorWindow win = GameObject.Instantiate(ScartchResourceManager.instance.actorWindow);
            ActorController cnt = new ActorController(act, vie, win);
            actors.Add(cnt);
        }
        public void RemoveGlobalVariable(int num)
        {
            globalVariables[num].Remove();
        }

        private bool running = false;

        public void ChangeMode()
        {
            running = !running;
            if (running)
                Scripting.ExecutionController.Instance.Execute();
            else
                Scripting.ExecutionController.Instance.Stop();
        }

        public void RemoveActor(ActorController actorController)
        {
            actors.Remove(actorController);
        }
    }
}
