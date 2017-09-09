using System.Collections;
using System.Collections.Generic;
using System;
using Model;
using UnityEngine;
using View;
using Scripting;

namespace Controller
{
    public class EnvironmentController
    {
        private List<ActorController> actors;
        private List<VariableController> globalVariableControllers;
        private EnvironmentViewer viewer;
        public event Action<bool> ModeChanged;
        public event Action InitiatingPlayMode;
        public event Action<Broadcaster, string> BroadcastMessage;
        public List<Variable> globalVariables;

        public void Broadcast(Broadcaster source, string message)
        {
            if (BroadcastMessage != null)
                BroadcastMessage(source, message);
        }

        private EnvironmentController(EnvironmentViewer viewer)
        {
            actors = new List<ActorController>();
            globalVariables = new List<Variable>();
            globalVariableControllers = new List<VariableController>();
            this.viewer = viewer;
            viewer.AddedActor += AddActor;
            viewer.AddedVariable += AddGlobalVariable;
            viewer.ChangedMode += ChangeModeEv;
            viewer.RemovedVariable += RemoveGlobalVariable;
            ModeChanged += viewer.OnControllerModeChanged;

            //FAST INIT
            //AddVariable(new Variable()
            //{
            //    Owner = null,
            //    Name = "TRU",
            //    Type = RefType.boolType,
            //    Value = "TRUE"
            //});
            //AddVariable(new Variable()
            //{
            //    Owner = null,
            //    Name = "TEN",
            //    Type = RefType.numberType,
            //    Value = "10"
            //});
        }

        public void ChangeModeEv()
        {
            ChangeMode();
        }

        public void AddVariable(Variable var)
        {
            globalVariables.Add(var);
        }

        public void RemoveVariable(Variable var)
        {
            var.Destroy();
            globalVariables.Remove(var);
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
            globalVariableControllers.Add(VariableController.AddVariable(entr, null));
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
            globalVariableControllers[num].Remove();
        }

        private bool running = false;
        public bool InPlayMode
        {
            get { return running; }
        }


        public void ChangeMode(bool test = false)
        {
            if (!test)
                running = !running;
            else
            {
                if (running)
                    return;
                else
                    running = true;
            }
            if (ModeChanged != null)
                ModeChanged(running);
            if (running && !test && InitiatingPlayMode != null)
                InitiatingPlayMode();
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
