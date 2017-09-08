using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripting;
using View.Resources;
using System;

namespace View
{
    public class ChooseScriptingElementWindow : VRWindow
    {
        public List<VRButton> bookmarks;
        public List<VRButton> voices;
        public List<UnityEngine.UI.Text> voiceLabels;
        public List<UnityEngine.UI.Image> voiceSprites;
        private List<ScriptingElement> voiceElements;
        public VRButton nextBtn, prevBtn;
        public UnityEngine.UI.Text pageTxt;

        public event System.Action<ScriptingElement> ChosenScriptingElement;
        private ScriptingType filter;

        public ScriptingType Filter
        {
            get { return filter; }
            set
            {
                filter = value;
                transform.GetChild(0).gameObject.GetComponent<Renderer>().material = ScartchResourceManager.instance.pages[(int)filter];
                Page = 1;
            }
        }

        public override void Open()
        {
            base.Open();
            UpdateVoices();
        }

        private int page = 1;
        private int Page
        {
            get
            {
                return page;
            }
            set
            {
                var num = ScartchResourceManager.instance.scriptingElements[(int)Filter].Count;
                if (Filter == ScriptingType.variable)
                {
                    num += summoner.GetVariableNumber();
                }
                var limit = (num / 9 + (num % 9 != 0 ? 1 : 0));
                nextBtn.gameObject.SetActive(value != limit);
                prevBtn.gameObject.SetActive(value != 1);
                page = value;
                pageTxt.text = value + "/" + limit;
                UpdateVoices();
            }
        }

        private void UpdateVoices()
        {
            int count = 0;
            voiceElements = new List<ScriptingElement>();
            List<ScriptingElement> vars = null;
            if (Filter == ScriptingType.variable)
            {
                vars = new List<ScriptingElement>();
                ScartchResourceManager.instance.scriptingElements[(int)Filter].ForEach(x => vars.Add(x));
                Action<Model.Variable> fun = x => { var r = new VariableReference(null, null, null, null, true); r.Variable = x; vars.Add(r); };
                summoner.localVariables.ForEach(fun);
                Controller.EnvironmentController.Instance.globalVariables.ForEach(fun);
            }
            voiceLabels.ForEach(x =>
            {
                ScriptingElement vel = null;
                try
                {
                    if (Filter != ScriptingType.variable)
                        vel = ScartchResourceManager.instance.scriptingElements[(int)Filter][(Page - 1) * 9 + count];
                    else
                        vel = vars[(Page - 1) * 9 + count];
                }
                catch (ArgumentOutOfRangeException)
                {
                }
                voiceElements.Add(vel);
                voices[count].gameObject.SetActive(vel != null);
                if (vel != null)
                {
                    x.text = vel.Description;
                    voiceSprites[count].sprite = vel.Sprite;
                }
                count++;
            });
        }

        private Model.Actor summoner;

        public void Init(Model.Actor summoner)
        {
            this.summoner = summoner;
            bookmarks.ForEach(x => x.Pressed += OnBookmarkPressed);
            voices.ForEach(x => x.Pressed += OnVoicePressed);
            nextBtn.Pressed += OnNextPressed;
            prevBtn.Pressed += OnPrevPressed;
            Filter = ScriptingType.movement;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            bookmarks.ForEach(x => x.Pressed -= OnBookmarkPressed);
            voices.ForEach(x => x.Pressed -= OnVoicePressed);
            nextBtn.Pressed -= OnNextPressed;
            prevBtn.Pressed -= OnPrevPressed;
        }

        private void OnPrevPressed(object sender, System.EventArgs e)
        {
            Page--;
        }

        private void OnNextPressed(object sender, System.EventArgs e)
        {
            Page++;
        }

        private void OnVoicePressed(object sender, System.EventArgs e)
        {
            if (ChosenScriptingElement != null)
                ChosenScriptingElement(voiceElements[voices.IndexOf(sender as VRButton)]);
        }

        private void OnBookmarkPressed(object sender, System.EventArgs e)
        {
            Filter = (ScriptingType)(bookmarks.IndexOf(sender as VRButton));
        }
    }
}
