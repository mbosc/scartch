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
                gameObject.GetComponent<Renderer>().material = ScartchResourceManager.instance.pages[(int)filter];
                Page = 0;
            }
        }

        private bool lastPage;

        private int page;
        private int Page
        {
            get
            {
                return page;
            }
            set
            {
                var num = ScartchResourceManager.instance.scriptingElements[(int)Filter].Count;
                var limit = (num / 9 + num % 9 != 0 ? 1 : 0);
                nextBtn.gameObject.SetActive(value != limit);
                prevBtn.gameObject.SetActive(value != 0);
                page = value;
                pageTxt.text = value + "/" + limit;
                UpdateVoices();
            }
        }

        private void UpdateVoices()
        {
            int count = 0;
            voiceElements = new List<ScriptingElement>();
            voiceLabels.ForEach(x =>
            {
                ScriptingElement vel = null;
                try
                {
                    vel = ScartchResourceManager.instance.scriptingElements[(int)Filter][Page * 9 + count];
                }
                catch (ArgumentOutOfRangeException) { }
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

        public void Init()
        {
            bookmarks.ForEach(x => x.Pressed += OnBookmarkPressed);
            voices.ForEach(x => x.Pressed += OnVoicePressed);
            nextBtn.Pressed += OnNextPressed;
            prevBtn.Pressed += OnPrevPressed;
            Filter = ScriptingType.control;
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
