using System.Collections;
using System.Collections.Generic;
using System;
using Model;
using View;
using UnityEngine;
using System.Linq;

namespace Scripting
{
    public abstract class ScriptingElement
    {
        protected Actor owner;
        protected Dictionary<int, Reference> referenceList;
        protected List<Option> optionList;
        protected ScriptingType type;
        public event Action Destroyed;
        protected List<ReferenceSlotViewer> referenceSlotViewers;

        public abstract string Description { get; }

        public abstract UnityEngine.Sprite Sprite
        {
            get;
        }

        public ScriptingElement(Actor owner, List<Option> optionList, ScriptingType type, List<ReferenceSlotViewer> referenceSlotViewers)
        {
            this.owner = owner;
            this.optionList = optionList ?? new List<Option>();
            this.type = type;
            this.referenceSlotViewers = referenceSlotViewers;

            // initialise references dictionary
            referenceList = new Dictionary<int, Reference>();
            int refcount = 0;
            referenceSlotViewers.ForEach(x => referenceList.Add(refcount++, null));

            // subscribe to referenceSlotViewers' events
            referenceSlotViewers.ForEach(x =>
            {
                x.SlotFilled += OnReferenceSlotViewerFilled;
                x.SlotEmptied += OnReferenceSlotViewerEmptied;
            });

        }

        private void OnReferenceSlotViewerEmptied(int num)
        {
            referenceList[num] = null;
        }

        private void OnReferenceSlotViewerFilled(int num, ReferenceViewer fillee)
        {
            referenceList[num] = fillee.Reference;
        }

        public bool TryGetReference(int num, out Reference reference)
        {
            reference = referenceList[num];
            return reference != null;
        }

        public Option GetOption(int num)
        {
            return optionList[num];
        }

        public virtual void Destroy()
        {
            referenceSlotViewers.ForEach(x =>
            {
                x.SlotFilled -= OnReferenceSlotViewerFilled;
                x.SlotEmptied -= OnReferenceSlotViewerEmptied;
            });
            if (Destroyed != null)
                Destroyed();
        }

        public static List<ReferenceSlotViewer> GenerateViewersFromText(ref string text, GameObject parent)
        {
            var res = new List<ReferenceSlotViewer>();
            bool closing = false;
            ReferenceSlotViewer refPF = null;
            char charToClose = 'a';
            string outString = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (!closing)
                {
                    if (refOpeningChars.Contains(text[i]))
                    {
                        closing = true;
                        charToClose = refClosingChars[refOpeningChars.IndexOf(text[i])];
                        refPF = GameObject.Instantiate(ScartchResourceManager.instance.referencePrefab).GetComponent<ReferenceSlotViewer>();
                        refPF.transform.SetParent(parent.transform, false);
                        var offset = new Vector3(1 + i / 2.0f, 0, -1);
                        if (parent.GetComponent<BlockoidViewer>() == null)
                            offset = new Vector3(i, 0, -1);
                        refPF.transform.localPosition = offset;
                        refPF.transform.localScale = Vector3.one;
                        refPF.Type = (RefType)refOpeningChars.IndexOf(text[i]);
                    }
                    else
                        outString += text[i];
                }
                else if (closing && text[i] == charToClose)
                {
                    closing = false;
                    res.Add(refPF);
                    outString += "    ";
                }
            }
            if (closing)
                throw new Exception("Illegal status!");
            text = outString;
            return res;
        }
        public static List<Option> GenerateOptionsFromText(ref string text, GameObject parent)
        {
            var res = new List<Option>();
            bool closing = false;
            int startIndex = 0;
            char charToClose = 'a';
            string outString = "";
            string currentString = "";
            List<string> currentStringList = null;
            for (int i = 0; i < text.Length; i++)
            {
                if (!closing)
                {
                    if (optOpeningChars.Contains(text[i]))
                    {
                        closing = true;
                        charToClose = optClosingChars[optOpeningChars.IndexOf(text[i])];
                        startIndex = i;
                        currentStringList = new List<string>();
                    }
                    else
                        outString += text[i];
                }
                else
                {
                    if (text[i] == '|')
                    {
                        currentStringList.Add(currentString.Trim());
                        currentString = "";
                    }
                    else if (text[i] == charToClose)
                    {
                        currentStringList.Add(currentString.Trim());
                        currentString = "";
                        closing = false;
                        int max = currentStringList.Max(x => x.Length);
                        outString += new string(' ', max + 3);
                        var combo = GameObject.Instantiate(ScartchResourceManager.instance.combobox).GetComponent<View.Resources.VRCombobox>();
                        combo.transform.SetParent(parent.transform, false);
                        var offset = new Vector3(1 + startIndex / 2.0f, 0, -1);
                        if (parent.GetComponent<BlockoidViewer>() == null)
                            offset = new Vector3(startIndex, 0, -1);
                        combo.transform.localPosition = offset;
                        combo.transform.localScale = Vector3.one/2;
                        var viewer = new OptionViewer(combo, currentStringList);
                        var opt = new Option(currentStringList, viewer);
                        res.Add(opt);
                    }
                    else
                    {
                        currentString += text[i];
                    }
                }
            }
            if (closing)
                throw new Exception("Illegal status!");
            text = outString;
            return res;
        }
        public static string refOpeningChars = "<([", refClosingChars = ">)]";
        public static string optOpeningChars = "{", optClosingChars = "}";
    }
}