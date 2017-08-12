using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace View
{
    public static class StringToFloatExtender
    {
        public static float ToStdNum(this string str)
        {
            return float.Parse(str);
        }
        public static string ToStdStr(this float flo)
        {
            return flo.ToString("N2");
        }
    }

    public class ActorWindow : Resources.VRWindow
    {
        private Model.Actor actor;
        public ChooseModelWindow modelChooser;
        public ChooseScriptingElementWindow seChooser;
        public VariableWindow varWindow;

        public event Action<string> NameChanged;
        public event Action<Vector3> PositionChanged, RotationChanged;
        public event Action<float> ScaleChanged, VolumeChanged;
        public event Action<bool, string> MessageChanged;
        public event Action<Model.ActorModel> ModelChanged;
        public event Action<bool> ChangedScriptingElementVisibility;
        public event Action<Scripting.ScriptingElement> ScriptingElementAdded;
        public event Action VariableAdded;
        public event Action<int> VariableRemoved;

        public UnityEngine.UI.Text title, messageShowHide;
        public SpriteRenderer modelSprite;
        public Resources.VRTextbox nameBox, posXBox, posYBox, posZBox, rotXBox, rotYBox, rotZBox, scaleBox, volBox, msgBox;
        public Resources.VRButton msgShowHideBtn, modelBtn, hideSEBtn, locVarBtn, addSEBtn;
        private bool msgVisibility;

        //ricordati quando azzeri tutto di settare bene i tipi di dato per i textbox

        public void Init(Model.Actor actor)
        {
            this.actor = actor;

            // Hide subwindows
            modelChooser.Close();
            seChooser.Init();
            seChooser.Close();
            varWindow.Init(this.actor);
            varWindow.Close();

            // Fields are initialised
            nameBox.Type = msgBox.Type = Model.RefType.stringType;
            posXBox.Type = posYBox.Type = posZBox.Type = rotXBox.Type = rotYBox.Type =
                rotZBox.Type = scaleBox.Type = volBox.Type = Model.RefType.numberType;
            UpdateFields();

            // Events are linked
            modelChooser.ModelChosen += ModelChooser_ModelChosen;
            seChooser.ChosenScriptingElement += SeChooser_ChosenScriptingElement;
            nameBox.TextChanged += OnNameChanged;
            posXBox.TextChanged += OnPositionChanged;
            posYBox.TextChanged += OnPositionChanged;
            posZBox.TextChanged += OnPositionChanged;
            rotXBox.TextChanged += OnRotationChanged;
            rotYBox.TextChanged += OnRotationChanged;
            rotZBox.TextChanged += OnRotationChanged;
            scaleBox.TextChanged += OnScaleChanged;
            volBox.TextChanged += OnVolumeChanged;
            msgBox.TextChanged += OnMessageChanged;
            msgShowHideBtn.Pressed += OnMessageChanged;
            modelBtn.Pressed += ModelBtn_Pressed;
            hideSEBtn.Pressed += HideSEBtn_Pressed;
            locVarBtn.Pressed += LocVarBtn_Pressed;
            addSEBtn.Pressed += AddSEBtn_Pressed;
            varWindow.VariableAdded += VarWindow_VariableAdded;
            varWindow.VariableRemoved += VarWindow_VariableRemoved;
        }

        private void VarWindow_VariableRemoved(int obj)
        {
            if (VariableRemoved != null)
                VariableRemoved(obj);
        }

        private void VarWindow_VariableAdded()
        {
            if (VariableAdded != null)
                VariableAdded();
        }

        private void SeChooser_ChosenScriptingElement(Scripting.ScriptingElement obj)
        {
            if (ScriptingElementAdded != null)
                ScriptingElementAdded(obj);
        }

        private void OnMessageChanged(object sender, EventArgs e)
        {
            if (MessageChanged != null)
            {
                MessageChanged(messageShowHide, msgBox.Text);
            }
        }

        private void OnVolumeChanged(object sender, EventArgs e)
        {
            if (VolumeChanged != null)
            {
                VolumeChanged(volBox.Text.ToStdNum());
            }
        }

        private void OnScaleChanged(object sender, EventArgs e)
        {
            if (ScaleChanged != null)
            {
                ScaleChanged(volBox.Text.ToStdNum());
            }
        }

        private void OnRotationChanged(object sender, EventArgs e)
        {
            if (RotationChanged != null)
            {
                RotationChanged(new Vector3(rotXBox.Text.ToStdNum(),
                    rotYBox.Text.ToStdNum(), rotZBox.Text.ToStdNum()));
            }
        }

        private void OnPositionChanged(object sender, EventArgs e)
        {
            if (PositionChanged != null)
            {
                PositionChanged(new Vector3(posXBox.Text.ToStdNum(),
                    posYBox.Text.ToStdNum(), posZBox.Text.ToStdNum()));
            }
        }

        private void UpdateFields()
        {
            title.text = "Actor " + actor.Name;
            nameBox.Text = actor.Name;
            msgBox.Text = actor.Message;
            msgVisibility = actor.IsMessageVisible;
            messageShowHide.text = msgVisibility ? "VISIBLE" : "HIDDEN";
            posXBox.Text = actor.Position.x.ToStdStr();
            posYBox.Text = actor.Position.y.ToStdStr();
            posZBox.Text = actor.Position.z.ToStdStr();
            rotXBox.Text = actor.Rotation.x.ToStdStr();
            rotYBox.Text = actor.Rotation.y.ToStdStr();
            rotZBox.Text = actor.Rotation.z.ToStdStr();
            scaleBox.Text = actor.Scale.ToStdStr();
            volBox.Text = actor.Volume.ToStdStr();
            modelSprite.sprite = ScartchResourceManager.instance.modelSprites[Model.ActorModel.IndexOfModel(actor.Model)];
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            // Events are unlinked
            modelChooser.ModelChosen -= ModelChooser_ModelChosen;
            seChooser.ChosenScriptingElement -= SeChooser_ChosenScriptingElement;
            nameBox.TextChanged -= OnNameChanged;
            posXBox.TextChanged -= OnPositionChanged;
            posYBox.TextChanged -= OnPositionChanged;
            posZBox.TextChanged -= OnPositionChanged;
            rotXBox.TextChanged -= OnRotationChanged;
            rotYBox.TextChanged -= OnRotationChanged;
            rotZBox.TextChanged -= OnRotationChanged;
            scaleBox.TextChanged -= OnScaleChanged;
            volBox.TextChanged -= OnVolumeChanged;
            msgBox.TextChanged -= OnMessageChanged;
            msgShowHideBtn.Pressed -= OnMessageChanged;
            modelBtn.Pressed -= ModelBtn_Pressed;
            hideSEBtn.Pressed -= HideSEBtn_Pressed;
            locVarBtn.Pressed -= LocVarBtn_Pressed;
            addSEBtn.Pressed -= AddSEBtn_Pressed;
            varWindow.VariableAdded -= VarWindow_VariableAdded;
            varWindow.VariableRemoved -= VarWindow_VariableRemoved;
        }

        private void AddSEBtn_Pressed(object sender, EventArgs e)
        {
            seChooser.Open();
        }

        private void LocVarBtn_Pressed(object sender, EventArgs e)
        {
            varWindow.Open();
        }

        private void HideSEBtn_Pressed(object sender, EventArgs e)
        {
            if (ChangedScriptingElementVisibility != null)
                ChangedScriptingElementVisibility(false);
        }

        private void ModelBtn_Pressed(object sender, EventArgs e)
        {
            modelChooser.Open();
        }

        private void OnNameChanged(object sender, EventArgs e)
        {
            if (NameChanged != null)
                NameChanged(nameBox.Text);
        }

        private void ModelChooser_ModelChosen(Model.ActorModel obj)
        {
            if (ModelChanged != null)
                ModelChanged(obj);
        }

        protected override void Update()
        {
            base.Update();
            if (Input.GetKeyDown(KeyCode.C))
                Init(new Model.Actor
                {
                    Name = "Actorino",
                    Position = new Vector3(4, 12, 0),
                    Rotation = Vector3.zero,
                    Scale = 1,
                    Volume = 1,
                    Message = "",
                    IsMessageVisible = false,
                    Model = Model.ActorModel.GetActorModel(1)
                });
        }
    }
}
