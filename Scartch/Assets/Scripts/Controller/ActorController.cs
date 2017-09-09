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

        //DEBUG ONLY
        public static ScriptingElementViewer lastSpawned;

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

            actorViewer.Selected += OnActorViewerSelected;
            actorViewer.Destroyed += OnActorViewerDestroyed;
            actorWindow.ChangedScriptingElementVisibility += OnActorWindowChangedScriptingElementVisibility;
            actorWindow.MessageChanged += OnActorWindowMessageChanged;
            actorWindow.ModelChanged += OnActorWindowModelChanged;
            actorWindow.NameChanged += OnActorWindowNameChanged;
            actorWindow.PositionXChanged += OnActorWindowPositionXChanged;
            actorWindow.PositionYChanged += OnActorWindowPositionYChanged;
            actorWindow.PositionZChanged += OnActorWindowPositionZChanged;
            actorWindow.RotationXChanged += OnActorWindowRotationXChanged;
            actorWindow.RotationYChanged += OnActorWindowRotationYChanged;
            actorWindow.RotationZChanged += OnActorWindowRotationZChanged;
            actorWindow.ScaleChanged += OnActorWindowScaleChanged;
            actorWindow.ScriptingElementAdded += OnActorWindowScriptingElementAdded;
            actorWindow.VariableAdded += OnActorWindowVariableAdded;
            actorWindow.VariableRemoved += OnActorWindowVariableRemoved;
            actorWindow.VolumeChanged += OnActorWindowVolumeChanged;
            actorWindow.Closed += OnActorWindowClosed;

            scriptingElementViewers.Keys.ToList().ForEach(x => x.Deleted += OnScriptingElementDeleted);

            actorWindow.Close();
        }

        private void OnActorWindowRotationYChanged(float obj)
        {
            SetActorRotation(new Vector3(actor.Rotation.x, obj, actor.Rotation.z));
        }

        private void OnActorWindowRotationXChanged(float obj)
        {
            SetActorRotation(new Vector3(obj, actor.Rotation.y, actor.Rotation.z));
        }

        private void OnActorWindowPositionYChanged(float obj)
        {
            SetActorPosition(new Vector3(actor.Position.x, obj, actor.Position.z));
        }

        private void OnActorWindowPositionXChanged(float obj)
        {
            SetActorPosition(new Vector3(obj, actor.Position.y, actor.Position.z));
        }

        private void OnActorWindowClosed()
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
            actorViewer.Selected -= OnActorViewerSelected;
            actorViewer.Destroyed -= OnActorViewerDestroyed;
            actorWindow.ChangedScriptingElementVisibility -= OnActorWindowChangedScriptingElementVisibility;
            actorWindow.MessageChanged -= OnActorWindowMessageChanged;
            actorWindow.ModelChanged -= OnActorWindowModelChanged;
            actorWindow.NameChanged -= OnActorWindowNameChanged;
            actorWindow.PositionXChanged -= OnActorWindowPositionXChanged;
            actorWindow.PositionYChanged -= OnActorWindowPositionYChanged;
            actorWindow.PositionZChanged -= OnActorWindowPositionZChanged;
            actorWindow.RotationXChanged -= OnActorWindowRotationXChanged;
            actorWindow.RotationYChanged -= OnActorWindowRotationYChanged;
            actorWindow.RotationZChanged -= OnActorWindowRotationZChanged;
            actorWindow.ScaleChanged -= OnActorWindowScaleChanged;
            actorWindow.ScriptingElementAdded -= OnActorWindowScriptingElementAdded;
            actorWindow.VariableAdded -= OnActorWindowVariableAdded;
            actorWindow.VariableRemoved -= OnActorWindowVariableRemoved;
            actorWindow.VolumeChanged -= OnActorWindowVolumeChanged;
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

        private void OnActorWindowVolumeChanged(float obj)
        {
            actor.Volume = obj;
        }

        private void OnActorWindowVariableRemoved(int obj)
        {

            localVariables[obj].Remove();
            this.actor.RemoveVariable(obj);

        }

        private void OnActorWindowVariableAdded(VariableEntry entr)
        {
            AddLocalVariable(entr);
        }

        private void OnActorWindowScriptingElementAdded(Scripting.ScriptingElement obj)
        {
            string text = obj.Description;
            string secondText = null;
            List<ReferenceSlotViewer> refl;
            List<Option> optl;
            GameObject gameObject = null;
            if (obj is DoubleMouthBlock)
            {
                text = (string)obj.GetType().GetField("description", BindingFlags.Public | BindingFlags.Static).GetValue(null);
                secondText = (string)obj.GetType().GetField("secondDescription", BindingFlags.Public | BindingFlags.Static).GetValue(null);
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
            string ignorelist = "";
            if (obj is LTExpression || obj is GTExpression)
            {
                ignorelist = "<>";
                (viewer as ReferenceViewer).Ignorelist = ignorelist;
            }
            Scripting.ScriptingElement.GenerateViewersFromText(ref text, viewer.gameObject, out refl, out optl, ignorelist);
            viewer.GetType().GetProperty("Text").SetValue(viewer, text, null);
            if (obj is DoubleMouthBlock)
                viewer.GetType().GetProperty("SecText").SetValue(viewer, secondText, null);
            ScriptingElement elem = (ScriptingElement)System.Activator.CreateInstance(obj.GetType(), this.actor, optl, refl, viewer, false);
            if (obj is VariableReference)
                (elem as VariableReference).Variable = (obj as VariableReference).Variable;
            viewer.Init(elem);
            scriptingElementViewers.Add(viewer, elem);
            viewer.Deleted += OnScriptingElementDeleted;

            //DEBUG ONLY
            lastSpawned = viewer;
        }

        private void OnActorWindowScaleChanged(float obj)
        {
            SetActorScale(obj);
        }

        private void OnActorWindowRotationZChanged(float obj)
        {
            SetActorRotation(new Vector3(actor.Rotation.x, actor.Rotation.y, obj));
        }

        private void OnActorWindowPositionZChanged(float obj)
        {
            SetActorPosition(new Vector3(actor.Position.x, actor.Position.y, obj));
        }

        private void OnActorWindowNameChanged(string obj)
        {
            actor.Name = obj;
        }

        private void OnActorWindowModelChanged(ActorModel obj)
        {
            actor.Model = obj;
        }

        private void OnActorWindowMessageChanged(bool arg1, string arg2)
        {
            actor.Message = arg2;
            actor.IsMessageVisible = arg1;
        }

        private void OnActorWindowChangedScriptingElementVisibility(bool obj)
        {
            ShowScriptingElements(obj);
        }

        private void OnActorViewerDestroyed()
        {
			localVariables.ForEach (x => x.Remove ());
            DeleteActor();
        }

        private void OnActorViewerSelected()
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
