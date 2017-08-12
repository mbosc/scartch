using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
using View;
using View.Resources;
using Scripting;
using System.Linq;

namespace Controller
{
    public class ActorController
    {
        private Actor actor;
        private ActorViewer actorViewer;
        private ActorWindow actorWindow;
        private Dictionary<ScriptingElementViewer, ScriptingElement> scriptingElementViewers;
        private List<VariableController> localVariables;

        public ActorController(Actor actor, ActorViewer viewer, ActorWindow window) : this(actor, viewer, window, new Dictionary<ScriptingElementViewer, ScriptingElement>(), new List<VariableController>())
        {
        }

        public ActorController(Actor actor, ActorViewer viewer, ActorWindow window, Dictionary<ScriptingElementViewer, ScriptingElement> viewers, List<VariableController> localVariables)
        {
            this.actor = actor;
            this.actorViewer = viewer;
            this.actorWindow = window;
            scriptingElementViewers = viewers;
            this.localVariables = localVariables;

            window.Init(actor);
            viewer.Init(actor, ScartchResourceManager.instance.actorSpawn);

            actorViewer.Selected += ActorViewer_Selected;
            actorViewer.Destroyed += ActorViewer_Destroyed;
            actorWindow.ChangedScriptingElementVisibility += ActorWindow_ChangedScriptingElementVisibility;
            actorWindow.MessageChanged += ActorWindow_MessageChanged;
            actorWindow.ModelChanged += ActorWindow_ModelChanged;
            actorWindow.NameChanged += ActorWindow_NameChanged;
            actorWindow.PositionChanged += ActorWindow_PositionChanged;
            actorWindow.RotationChanged += ActorWindow_RotationChanged;
            actorWindow.ScaleChanged += ActorWindow_ScaleChanged;
            actorWindow.ScriptingElementAdded += ActorWindow_ScriptingElementAdded;
            actorWindow.VariableAdded += ActorWindow_VariableAdded;
            actorWindow.VariableRemoved += ActorWindow_VariableRemoved;
            actorWindow.VolumeChanged += ActorWindow_VolumeChanged;

            scriptingElementViewers.Keys.ToList().ForEach(x => x.Deleted += OnScriptingElementDeleted);

            actorWindow.Close();
        }

        private void OnScriptingElementDeleted(object sender, System.EventArgs e)
        {
            var viewer = sender as ScriptingElementViewer;
            scriptingElementViewers[viewer].Destroy();
            GameObject.Destroy(viewer);
        }

        public void SetActorPosition(Vector3 position)
        {
            actor.Position = position;
        }

        public void SetActorRotation(Vector3 rotation)
        {
            actor.Rotation = rotation;
        }

        public void SetActorScale(float scale)
        {
            actor.Scale = scale;
        }
        public void SetActorVolume(float volume)
        {
            actor.Volume = volume;
        }
        public void DeleteActor()
        {
            //Prepare for detachment
            actorViewer.Selected -= ActorViewer_Selected;
            actorViewer.Destroyed -= ActorViewer_Destroyed;
            actorWindow.ChangedScriptingElementVisibility -= ActorWindow_ChangedScriptingElementVisibility;
            actorWindow.MessageChanged -= ActorWindow_MessageChanged;
            actorWindow.ModelChanged -= ActorWindow_ModelChanged;
            actorWindow.NameChanged -= ActorWindow_NameChanged;
            actorWindow.PositionChanged -= ActorWindow_PositionChanged;
            actorWindow.RotationChanged -= ActorWindow_RotationChanged;
            actorWindow.ScaleChanged -= ActorWindow_ScaleChanged;
            actorWindow.ScriptingElementAdded -= ActorWindow_ScriptingElementAdded;
            actorWindow.VariableAdded -= ActorWindow_VariableAdded;
            actorWindow.VariableRemoved -= ActorWindow_VariableRemoved;
            actorWindow.VolumeChanged -= ActorWindow_VolumeChanged;
            scriptingElementViewers.Keys.ToList().ForEach(x => x.Deleted -= OnScriptingElementDeleted);

            EnvironmentController.Instance.RemoveActor(this);
            actor.Destroy();
        }

        public void ShowScriptingElements(bool show)
        {
            scriptingElementViewers.Keys.ToList().ForEach(x => x.Visible = show);
        }

        public void AddLocalVariable(VariableEntry entr)
        {
            localVariables.Add(VariableController.AddVariable(entr, actor));
        }

        private void ActorWindow_VolumeChanged(float obj)
        {
            actor.Volume = obj;
        }

        private void ActorWindow_VariableRemoved(int obj)
        {
            try
            {
                localVariables[obj].Remove();
                this.actor.RemoveVariable(obj);
            }
            catch (System.Exception exc)
            {
                ScartchResourceManager.instance.lastRayCaster.Alert(exc.Message);
            }
        }

        private void ActorWindow_VariableAdded(VariableEntry entr)
        {
            AddLocalVariable(entr);
        }

        private void ActorWindow_ScriptingElementAdded(Scripting.ScriptingElement obj)
        {
            //TODO
            throw new System.NotImplementedException();
        }

        private void ActorWindow_ScaleChanged(float obj)
        {
            SetActorScale(obj);
        }

        private void ActorWindow_RotationChanged(Vector3 obj)
        {
            SetActorRotation(obj);
        }

        private void ActorWindow_PositionChanged(Vector3 obj)
        {
            SetActorPosition(obj);
        }

        private void ActorWindow_NameChanged(string obj)
        {
            actor.Name = obj;
        }

        private void ActorWindow_ModelChanged(ActorModel obj)
        {
            actor.Model = obj;
        }

        private void ActorWindow_MessageChanged(bool arg1, string arg2)
        {
            actor.Message = arg2;
            actor.IsMessageVisible = arg1;
        }

        private void ActorWindow_ChangedScriptingElementVisibility(bool obj)
        {
            ShowScriptingElements(obj);
        }

        private void ActorViewer_Destroyed()
        {

            DeleteActor();
        }

        private void ActorViewer_Selected()
        {
            if (Selected != null)
                Selected.actorViewer.Highlighted = false;
            Selected = this;
            Selected.actorViewer.Highlighted = true;
            actorWindow.Open();
        }

        public static ActorController Selected;
    }
}
