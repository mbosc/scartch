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
        private class ImmediateReference : ExpressionReference
        {
            public ImmediateReference(RefType rType) : base(null, null, ScriptingType.variable, null, null, rType, true)
            {
                value = RefTypeHelper.Default(rType);
            }

            private string value = "";

            public override string Description
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public override string Evaluate()
            {
                return value;
            }
        }

        protected Actor owner;
        protected Dictionary<int, Reference> referenceList;
        public List<Reference> ReferenceList
        {
            get
            {
                var res = new List<Reference>();
                RSV.ForEach(x => res.Add(x.Filler == null ? new ImmediateReference(x.Type) : x.Filler.Reference));
                return res;
            }
        }
        protected List<Option> optionList;
        protected ScriptingType type;
        public event Action Destroyed;
        protected List<ReferenceSlotViewer> referenceSlotViewers;
        public List<ReferenceSlotViewer> RSV
        {
            get { return referenceSlotViewers; }
        }
        public List<Option> OPT
        {
            get { return optionList; }
        }

        public abstract string Description { get; }

        public abstract UnityEngine.Sprite Sprite
        {
            get;
        }

        public ScriptingElement(Actor owner, List<Option> optionList, ScriptingType type, List<ReferenceSlotViewer> referenceSlotViewers, bool sample)
        {
            if (!sample)
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
            optionList.ForEach(x => x.Destroy());
            if (Destroyed != null)
                Destroyed();
        }

        public static void GenerateViewersFromText(ref string text, GameObject parent, out List<ReferenceSlotViewer> refViewers, out List<Option> options)
        {
            var rvw = new List<ReferenceSlotViewer>();
            var opts = new List<Option>();
            bool closing = false;
            ReferenceSlotViewer refPF = null;
            char charToClose = 'a';
            string outString = "";
            bool workingOnOpt = false;
            int newpos = 0;
            int startIndex = 0;
            string currentString = "";
            List<string> currentStringList = null;
            for (int i = 0; i < text.Length; i++)
            {
                if (!closing)
                {
                    if (refOpeningChars.Contains(text[i]))
                    {
                        closing = true;
                        charToClose = refClosingChars[refOpeningChars.IndexOf(text[i])];
                        GameObject gameo = GameObject.Instantiate(ScartchResourceManager.instance.referenceSlotViewer);
                        refPF = gameo.GetComponent<ReferenceSlotViewer>();
                        refPF.transform.SetParent(parent.transform, false);
                        var offset = new Vector3(1 + newpos / 2.0f, 0, -1);
                        if (parent.GetComponent<BlockoidViewer>() == null)
                            offset = new Vector3(newpos, 0, -1) / 2;
                        refPF.transform.localPosition = offset;
                        refPF.transform.localScale = Vector3.one;
                        refPF.Type = (RefType)refOpeningChars.IndexOf(text[i]);

                    }
                    else if (optOpeningChars.Contains(text[i]))
                    {
                        workingOnOpt = true;
                        closing = true;
                        charToClose = optClosingChars[optOpeningChars.IndexOf(text[i])];
                        startIndex = newpos;
                        currentStringList = new List<string>();
                    }
                    outString += text[i];
                    newpos++;
                }
                else
                {
                    if (workingOnOpt)
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
                            var addition = new string(' ', max + 2) + "}";
                            outString += addition;
                            newpos += addition.Length;

                            var combo = GameObject.Instantiate(ScartchResourceManager.instance.combobox).GetComponent<View.Resources.VRCombobox>();
                            combo.transform.SetParent(parent.transform, false);
                            var offset = new Vector3(1 + startIndex / 2.0f, 0, -1);
                            if (parent.GetComponent<BlockoidViewer>() == null)
                                offset = new Vector3(startIndex, 0, -1)/2;
                            combo.transform.localPosition = offset;
                            combo.transform.localScale = Vector3.one / 2;
                            var viewer = new OptionViewer(combo, currentStringList);
                            var opt = new Option(currentStringList, viewer);
                            opts.Add(opt);
                            workingOnOpt = false;
                        }
                        else
                        {
                            currentString += text[i];
                        }
                    }
                    else if (closing && text[i] == charToClose)
                    {
                        closing = false;
                        rvw.Add(refPF);
                        refPF.Number = rvw.IndexOf(refPF);
                        outString += "  ";
                        outString += text[i];
                        newpos += 3;
                    }
                }
            }
            if (closing)
                throw new Exception("Illegal status!");
            text = outString;
            refViewers = rvw;
            options = opts;
        }
        public static string refOpeningChars = "<([", refClosingChars = ">)]";
        public static string optOpeningChars = "{", optClosingChars = "}";
        public static string openingChars = "<([{", closingChars = ">)]}";
    }
}