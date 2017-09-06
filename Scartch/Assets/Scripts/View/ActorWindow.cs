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
        private ChooseModelWindow modelChooser;
        private ChooseScriptingElementWindow seChooser;
        private VariableWindow varWindow;

        public event Action<string> NameChanged;
        public event Action<float> PositionXChanged, PositionYChanged, PositionZChanged,
            RotationXChanged, RotationYChanged, RotationZChanged;
        public event Action<float> ScaleChanged, VolumeChanged;
        public event Action<bool, string> MessageChanged;
        public event Action<Model.ActorModel> ModelChanged;
        public event Action<bool> ChangedScriptingElementVisibility;
        public event Action<Scripting.ScriptingElement> ScriptingElementAdded;
        public event Action<VariableEntry> VariableAdded;
        public event Action<int> VariableRemoved;
        public event Action Closed;

        public UnityEngine.UI.Text title, messageShowHide;
        public SpriteRenderer modelSprite;
        public Resources.VRTextbox nameBox, posXBox, posYBox, posZBox, rotXBox, rotYBox, rotZBox, scaleBox, volBox, msgBox;
        public Resources.VRButton msgShowHideBtn, modelBtn, hideSEBtn, locVarBtn, addSEBtn;
        private bool msgVisibility;

        //ricordati quando azzeri tutto di settare bene i tipi di dato per i textbox

        public void Init(Model.Actor actor)
        {
            this.actor = actor;

            modelChooser = GameObject.Instantiate(ScartchResourceManager.instance.chooseModelWindow);
            modelChooser.level = 1;
            seChooser = GameObject.Instantiate(ScartchResourceManager.instance.chooseBlockWindow);
            seChooser.level = 1;
            varWindow = GameObject.Instantiate(ScartchResourceManager.instance.variableWindow);
            varWindow.level = 1;

            // Hide subwindows
            modelChooser.Close();
            seChooser.Init(actor);
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
            posXBox.TextChanged += OnPositionXChanged;
            posYBox.TextChanged += OnPositionYChanged;
            posZBox.TextChanged += OnPositionZChanged;
            rotXBox.TextChanged += OnRotationXChanged;
            rotYBox.TextChanged += OnRotationYChanged;
            rotZBox.TextChanged += OnRotationZChanged;
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

            actor.NameChanged += Actor_NameChanged;
            actor.Destroyed += Actor_Destroyed;
            actor.MessageChanged += Actor_MessageChanged;
            actor.Moved += Actor_Moved;
            actor.ModelChanged += Actor_ModelChanged;
            actor.ScaleChanged += Actor_ScaleChanged;
            actor.VolumeChanged += Actor_VolumeChanged;
        }

        private void Actor_NameChanged(string obj)
        {
            UpdateFields();
        }

        private void Actor_VolumeChanged(float obj)
        {
            UpdateFields();
        }

        private void Actor_ScaleChanged(float obj)
        {
            UpdateFields();
        }

        private void Actor_ModelChanged(Model.ActorModel obj)
        {
            UpdateFields();
        }

        private void Actor_Moved(Vector3 arg1, Vector3 arg2)
        {
            UpdateFields();
        }

        private void Actor_MessageChanged(bool arg1, string arg2)
        {
            UpdateFields();
        }

        private void VarWindow_VariableRemoved(int obj)
        {
            if (VariableRemoved != null)
                VariableRemoved(obj);
        }

        private void VarWindow_VariableAdded(VariableEntry ent)
        {
            if (VariableAdded != null)
                VariableAdded(ent);
        }

        private void SeChooser_ChosenScriptingElement(Scripting.ScriptingElement obj)
        {
            if (ScriptingElementAdded != null)
                ScriptingElementAdded(obj);
        }

        private void OnMessageChanged(object sender, EventArgs e)
        {
            if (sender.Equals(msgShowHideBtn))
            {
                msgVisibility = !msgVisibility;
            }
            if (MessageChanged != null)
            {
                MessageChanged(msgVisibility, msgBox.Text);
            }
        }

        public override void Close()
        {
            base.Close();
            varWindow.Close();
            seChooser.Close();
            modelChooser.Close();
            if (Closed != null)
                Closed();
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
                ScaleChanged(scaleBox.Text.ToStdNum());
            }
        }

        private void OnRotationXChanged(object sender, EventArgs e)
        {
            if (RotationXChanged != null)
            {
                RotationXChanged(rotXBox.Text.ToStdNum());
            }
        }
        private void OnRotationYChanged(object sender, EventArgs e)
        {
            if (RotationYChanged != null)
            {
                RotationYChanged(rotYBox.Text.ToStdNum());
            }
        }
        private void OnRotationZChanged(object sender, EventArgs e)
        {
            if (RotationZChanged != null)
            {
                RotationZChanged(rotZBox.Text.ToStdNum());
            }
        }

        private void OnPositionXChanged(object sender, EventArgs e)
        {
            if (PositionXChanged != null)
            {
                PositionXChanged(posXBox.Text.ToStdNum());
            }
        }
        private void OnPositionYChanged(object sender, EventArgs e)
        {
            if (PositionYChanged != null)
            {
                PositionYChanged(posYBox.Text.ToStdNum());
            }
        }
        private void OnPositionZChanged(object sender, EventArgs e)
        {
            if (PositionZChanged != null)
            {
                PositionZChanged(posZBox.Text.ToStdNum());
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
            posXBox.TextChanged -= OnPositionXChanged;
            posYBox.TextChanged -= OnPositionYChanged;
            posZBox.TextChanged -= OnPositionZChanged;
            rotXBox.TextChanged -= OnRotationXChanged;
            rotYBox.TextChanged -= OnRotationYChanged;
            rotZBox.TextChanged -= OnRotationZChanged;
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
            actor.NameChanged -= Actor_NameChanged;
            actor.Destroyed -= Actor_Destroyed;
            actor.MessageChanged -= Actor_MessageChanged;
            actor.Moved -= Actor_Moved;
            actor.ModelChanged -= Actor_ModelChanged;
            actor.ScaleChanged -= Actor_ScaleChanged;
            actor.VolumeChanged -= Actor_VolumeChanged;

            if (seChooser != null)
                GameObject.Destroy(seChooser.gameObject);
            if (varWindow != null)
                GameObject.Destroy(varWindow.gameObject);
            if (modelChooser != null)
                GameObject.Destroy(modelChooser.gameObject);
        }

        private void Actor_Destroyed()
        {
            Destroy(this.gameObject);
        }

        private void AddSEBtn_Pressed(object sender, EventArgs e)
        {
            seChooser.Open();
            seChooser.transform.SetParent(null);
        }

        private void LocVarBtn_Pressed(object sender, EventArgs e)
        {
            varWindow.Open();
            varWindow.transform.SetParent(null);
        }

        private void HideSEBtn_Pressed(object sender, EventArgs e)
        {
            if (ChangedScriptingElementVisibility != null)
                ChangedScriptingElementVisibility(false);
        }

        private void ModelBtn_Pressed(object sender, EventArgs e)
        {
            modelChooser.Open();
            modelChooser.transform.SetParent(null);
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
    }
}
