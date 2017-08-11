using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace View
{
    public class ActorViewer : Resources.VRWindow
    {
        private Model.Actor actor;
        public ChooseModelWindow modelChooser;
        public ChooseScriptingElementWindow seChooser;
        public event Action<Vector3> PositionChanged, RotationChanged;
        public event Action<float> ScaleChanged, VolumeChanged;
        public event Action<bool, string> MessageChanged;
        public event Action<Model.ActorModel> ModelChanged;
        public event Action<bool> ChangedScriptingElementVisibility;
        public event Action<Scripting.ScriptingElement> ScriptingElementAdded;

        public UnityEngine.UI.Text title, messageShowHide;
        public Sprite modelSprite;
        public Resources.VRTextbox nameBox, posXBox, posYBox, posZBox, rotXBox, rotYBox, rotZBox, scaleBox, volBox, msgBox;
        public Resources.VRButton msgShowHideBtn, modelBtn, hideSEBtn, locVarBtn, addSEBtn;

        //ricordati quando azzeri tutto di settare bene i tipi di dato per i textbox
    }
}
