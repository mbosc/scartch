using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
using View;
using View.Resources;
using Scripting;
using System.Linq;
using System;
using System.Reflection;

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
            actorWindow.PositionXChanged += ActorWindow_PositionXChanged;
            actorWindow.PositionYChanged += ActorWindow_PositionYChanged;
            actorWindow.PositionZChanged += ActorWindow_PositionZChanged;
            actorWindow.RotationXChanged += ActorWindow_RotationXChanged;
            actorWindow.RotationYChanged += ActorWindow_RotationYChanged;
            actorWindow.RotationZChanged += ActorWindow_RotationZChanged;
            actorWindow.ScaleChanged += ActorWindow_ScaleChanged;
            actorWindow.ScriptingElementAdded += ActorWindow_ScriptingElementAdded;
            actorWindow.VariableAdded += ActorWindow_VariableAdded;
            actorWindow.VariableRemoved += ActorWindow_VariableRemoved;
            actorWindow.VolumeChanged += ActorWindow_VolumeChanged;
            actorWindow.Closed += ActorWindow_Closed;

            scriptingElementViewers.Keys.ToList().ForEach(x => x.Deleted += OnScriptingElementDeleted);

            actorWindow.Close();
        }

        private void ActorWindow_RotationYChanged(float obj)
        {
            SetActorRotation(new Vector3(actor.Rotation.x, obj, actor.Rotation.z));
        }

        private void ActorWindow_RotationXChanged(float obj)
        {
            SetActorRotation(new Vector3(obj, actor.Rotation.y, actor.Rotation.z));
        }

        private void ActorWindow_PositionYChanged(float obj)
        {
            SetActorPosition(new Vector3(actor.Position.x, obj, actor.Position.z));
        }

        private void ActorWindow_PositionXChanged(float obj)
        {
            SetActorPosition(new Vector3(obj, actor.Position.y, actor.Position.z));
        }

        private void ActorWindow_Closed()
        {
            ShowScriptingElements(false);
            if (Selected == this)
            {
                Selected.actorViewer.Highlighted = false;
                Selected = null;
            }
            
        }

        private void OnScriptingElementDeleted(object sender, System.EventArgs e)
        {
            var viewer = sender as ScriptingElementViewer;
            scriptingElementViewers[viewer].Destroy();
            scriptingElementViewers.Remove(viewer);
            GameObject.Destroy(viewer.gameObject);
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
            actorWindow.PositionXChanged -= ActorWindow_PositionXChanged;
            actorWindow.PositionYChanged -= ActorWindow_PositionYChanged;
            actorWindow.PositionZChanged -= ActorWindow_PositionZChanged;
            actorWindow.RotationXChanged -= ActorWindow_RotationXChanged;
            actorWindow.RotationYChanged -= ActorWindow_RotationYChanged;
            actorWindow.RotationZChanged -= ActorWindow_RotationZChanged;
            actorWindow.ScaleChanged -= ActorWindow_ScaleChanged;
            actorWindow.ScriptingElementAdded -= ActorWindow_ScriptingElementAdded;
            actorWindow.VariableAdded -= ActorWindow_VariableAdded;
            actorWindow.VariableRemoved -= ActorWindow_VariableRemoved;
            actorWindow.VolumeChanged -= ActorWindow_VolumeChanged;
            scriptingElementViewers.Keys.ToList().ForEach(x => {
                x.Deleted -= OnScriptingElementDeleted;
                x.Delete();
                scriptingElementViewers[x].Destroy();
                GameObject.Destroy(x.gameObject);
            });
            if (this.Equals(Selected))
                Selected = null;
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

            localVariables[obj].Remove();
            this.actor.RemoveVariable(obj);

        }

        private void ActorWindow_VariableAdded(VariableEntry entr)
        {
            AddLocalVariable(entr);
        }

        private void ActorWindow_ScriptingElementAdded(Scripting.ScriptingElement obj)
        {
            string text = obj.Description;
            string secondText = null;
            List<ReferenceSlotViewer> refl;
            List<Option> optl;
            GameObject gameObject = null;
            if (obj is DoubleMouthBlock)
            {
                text = (string) obj.GetType().GetField("description", BindingFlags.Public | BindingFlags.Static).GetValue(null);
                secondText = (string) obj.GetType().GetField("secondDescription", BindingFlags.Public | BindingFlags.Static).GetValue(null);
                gameObject = ScartchResourceManager.instance.doubleMouthBlockViewer;
            }
            else if (obj is MouthBlock)
                gameObject = ScartchResourceManager.instance.mouthBlockViewer;
            else if (obj is Block)
                gameObject = ScartchResourceManager.instance.blockViewer;
            else if (obj is Reference)
                gameObject = ScartchResourceManager.instance.referenceViewer;
            else if (obj is Hat)
                gameObject = ScartchResourceManager.instance.hatViewer;
            ScriptingElementViewer viewer = GameObject.Instantiate(gameObject).GetComponent<ScriptingElementViewer>();
            Scripting.ScriptingElement.GenerateViewersFromText(ref text, viewer.gameObject, out refl, out optl);
            viewer.GetType().GetProperty("Text").SetValue(viewer, text, null);
            if (obj is DoubleMouthBlock)
                viewer.GetType().GetProperty("SecText").SetValue(viewer, secondText, null);
            ScriptingElement elem = (ScriptingElement)System.Activator.CreateInstance(obj.GetType(), this.actor, optl, refl, viewer, false);
            if (obj is VariableReference)
                (elem as VariableReference).Variable = (obj as VariableReference).Variable;
            viewer.Init(elem);
            scriptingElementViewers.Add(viewer.GetComponent<ScriptingElementViewer>(), elem);

            viewer.Deleted += OnScriptingElementDeleted;
        }

        private void ActorWindow_ScaleChanged(float obj)
        {
            SetActorScale(obj);
        }

        private void ActorWindow_RotationZChanged(float obj)
        {
            SetActorRotation(new Vector3(actor.Rotation.x, actor.Rotation.y, obj));
        }

        private void ActorWindow_PositionZChanged(float obj)
        {
            SetActorPosition(new Vector3(actor.Position.x, actor.Position.y, obj));
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
            {
                Selected.actorViewer.Highlighted = false;
                Selected.ShowScriptingElements(false);
            }
            Selected = this;
            Selected.actorViewer.Highlighted = true;
            actorWindow.Open();
            ShowScriptingElements(true);
        }

        public static ActorController Selected;
    }
}
