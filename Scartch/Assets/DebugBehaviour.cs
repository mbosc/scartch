using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;
using Scripting;
using View.Resources;
using Model;
using System.Reflection;

public class DebugBehaviour : MonoBehaviour {

    private Actor actor;

    // Use this for initialization
    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            StartCoroutine(FactorialDemo());
        if (Input.GetKeyDown(KeyCode.Alpha2))
            StartCoroutine(MessageDemo());
    }

    private IEnumerator MessageDemo()
    {
        yield return new WaitForSeconds(1);
        

        #region Prepare_Variables_GLOBAL
        FindObjectOfType<EnvironmentViewer>().listVarButton.HitByBlueRay();
        yield return new WaitForSeconds(0.1f);
        var variableWindow = GameObject.Find("Global Variables Window").GetComponent<VariableWindow>();
        variableWindow.addVarButton.HitByBlueRay();
        yield return new WaitForSeconds(0.1f);
        var entry1 = variableWindow.transform.GetChild(4).GetComponent<VariableEntry>();
        entry1.transform.GetChild(0).GetComponent<VRTextbox>().Text = "HELLO";
        entry1.transform.GetChild(1).GetComponent<VRTextbox>().Text = "HELLO";
        variableWindow.addVarButton.HitByBlueRay();
        yield return new WaitForSeconds(0.1f);
        var entry2 = variableWindow.transform.GetChild(5).GetComponent<VariableEntry>();
        entry2.transform.GetChild(0).GetComponent<VRTextbox>().Text = "NOPE";
        entry2.transform.GetChild(1).GetComponent<VRTextbox>().Text = "NOPE";
        variableWindow.Close();
        #endregion

        // Spawn actors
        Controller.EnvironmentController.Instance.AddActor();
        Controller.EnvironmentController.Instance.AddActor();
        FindObjectsOfType<View.ActorWindow>().ToList().ForEach( s => s.addSEBtn.HitByRedRay());
        var actors = FindObjectsOfType<View.ActorViewer>();

        #region Prepare_Variables_ACTORS
        actors[0].HitByBlueRay();
        var actorWindow = FindObjectOfType<View.ActorWindow>();
        actorWindow.locVarBtn.HitByBlueRay();
        actorWindow.nameBox.Text = "ACT1";
        yield return new WaitForSeconds(0.1f);
        variableWindow = GameObject.Find("VarWindow(Clone)").GetComponent<VariableWindow>();
        variableWindow.addVarButton.HitByBlueRay();
        yield return new WaitForSeconds(0.1f);
        entry1 = variableWindow.transform.GetChild(4).GetComponent<VariableEntry>();
        entry1.transform.GetChild(0).GetComponent<VRTextbox>().Text = "220";
        entry1.transform.GetChild(2).GetComponent<VRCombobox>().Selected = 1;
        entry1.transform.GetChild(1).GetComponent<VRTextbox>().Text = "220";
        variableWindow.Close();
        actorWindow.locVarBtn.HitByRedRay();

        actors[1].HitByBlueRay();
        actorWindow = FindObjectOfType<View.ActorWindow>();
        actorWindow.modelBtn.HitByBlueRay();
        FindObjectOfType<ChooseModelWindow>().buttons[1].HitByBlueRay();
        actorWindow.nameBox.Text = "ACT2";
        actorWindow.posZBox.Text = "90";
        actorWindow.locVarBtn.HitByBlueRay();
        yield return new WaitForSeconds(0.1f);
        variableWindow = GameObject.Find("VarWindow(Clone)").GetComponent<VariableWindow>();
        variableWindow.addVarButton.HitByBlueRay();
        yield return new WaitForSeconds(0.1f);
        entry1 = variableWindow.transform.GetChild(4).GetComponent<VariableEntry>();
        entry1.transform.GetChild(0).GetComponent<VRTextbox>().Text = "VICINO";
        entry1.transform.GetChild(1).GetComponent<VRTextbox>().Text = "VICINO.AL.BORDO";
        variableWindow.Close();
        actorWindow.locVarBtn.HitByRedRay();
        #endregion

        #region Prepare_Pieces
        //Prepare pieces for actor 1
        actors[0].HitByBlueRay();
        actorWindow = FindObjectOfType<View.ActorWindow>();
        actorWindow.addSEBtn.HitByBlueRay();
        var seWindow = GameObject.Find("NewSelector(Clone)").GetComponent<View.ChooseScriptingElementWindow>();

        BlockViewer bro1, bro2, setpo;
        DoubleMouthBlockViewer ife;
        MouthBlockViewer forevah;
        HatViewer onplay;
        ReferenceViewer gt, contrpo, abs, posx, v220, mhello, mnope;
        
        Action<int> setPage = (num) => { seWindow.transform.GetChild(0).GetChild(0).GetChild(num).GetComponent<VRButton>().HitByBlueRay(); };
        Func<int, ScriptingElementViewer> selectVoice = (num) => { seWindow.transform.GetChild(0).GetChild(2).GetChild(num).GetComponent<VRButton>().HitByBlueRay(); return Controller.ActorController.lastSpawned; };
        Action nextPage = () => { seWindow.transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<VRButton>().HitByBlueRay(); };

        yield return new WaitForSeconds(0.1f);
        setPage(0);

        setpo = selectVoice(6) as BlockViewer;
        posx = selectVoice(8) as ReferenceViewer;

        yield return new WaitForSeconds(0.1f);
        setPage(3);

        ife = selectVoice(4) as DoubleMouthBlockViewer;
        onplay = selectVoice(7) as HatViewer;
        forevah = selectVoice(2) as MouthBlockViewer;

        yield return new WaitForSeconds(0.1f);
        nextPage();

        bro1 = selectVoice(1) as BlockViewer;
        bro2 = selectVoice(1) as BlockViewer;

        yield return new WaitForSeconds(0.1f);
        setPage(4);

        contrpo = selectVoice(2) as ReferenceViewer;

        yield return new WaitForSeconds(0.1f);
        setPage(5);

        gt = selectVoice(4) as ReferenceViewer;

        yield return new WaitForSeconds(0.1f);
        nextPage();

        abs = selectVoice(7) as ReferenceViewer;

        yield return new WaitForSeconds(0.1f);
        setPage(6);

        v220 = selectVoice(4) as ReferenceViewer;
        mhello = selectVoice(5) as ReferenceViewer;
        mnope = selectVoice(6) as ReferenceViewer;
        seWindow.Close();
        

        //mount them
        yield return new WaitForSeconds(0.1f);

        Func<ScriptingElementViewer, int, ReferenceSlotViewer> slot = (go, num) =>
        {
            ReferenceSlotViewer res = null;
            Enumerable.Range(0, go.transform.childCount).ToList().ForEach((n) =>
            {
                if (res != null)
                    return;
                if (go.transform.GetChild(n).name.Equals("ReferenceSlot(Clone)"))
                {
                    if (num == 0)
                        res = go.transform.GetChild(n).GetComponent<ReferenceSlotViewer>();
                    else
                        num--;
                }
            });
            return res;
        };

        yield return new WaitForSeconds(0.1f);
        onplay.SnapNext(forevah);
        yield return new WaitForSeconds(0.1f);
        forevah.SnapInnerNext(setpo);
        yield return new WaitForSeconds(0.1f);
        slot(setpo, 0).Filler = contrpo;
        yield return new WaitForSeconds(0.1f);
        setpo.SnapNext(ife);
        yield return new WaitForSeconds(0.1f);
        slot(ife, 0).Filler = gt;
        yield return new WaitForSeconds(0.1f);
        slot(gt, 0).Filler = abs;
        yield return new WaitForSeconds(0.1f);
        slot(abs, 0).Filler = posx;
        yield return new WaitForSeconds(0.1f);
        slot(gt, 1).Filler = v220;
        yield return new WaitForSeconds(0.1f);
        ife.SnapUpperInnerNext(bro1);
        yield return new WaitForSeconds(0.1f);
        slot(bro1, 0).Filler = mhello;
        yield return new WaitForSeconds(0.1f);
        ife.SnapLowerInnerNext(bro2);
        yield return new WaitForSeconds(0.1f);
        slot(bro2, 0).Filler = mnope;
        actorWindow.Close();

        //Prepare pieces for actor 2
        actors[1].HitByBlueRay();
        actorWindow = FindObjectOfType<View.ActorWindow>();
        actorWindow.addSEBtn.HitByBlueRay();
        seWindow = GameObject.Find("NewSelector(Clone)").GetComponent<View.ChooseScriptingElementWindow>();

        BlockViewer say, clear;
        HatViewer onmess1, onmess2;
        ReferenceViewer vicino;

        setPage = (num) => { seWindow.transform.GetChild(0).GetChild(0).GetChild(num).GetComponent<VRButton>().HitByBlueRay(); };
        selectVoice = (num) => { seWindow.transform.GetChild(0).GetChild(2).GetChild(num).GetComponent<VRButton>().HitByBlueRay(); return Controller.ActorController.lastSpawned; };
        nextPage = () => { seWindow.transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<VRButton>().HitByBlueRay(); };

        yield return new WaitForSeconds(0.1f);
        setPage(1);

        say = selectVoice(0) as BlockViewer;
        clear = selectVoice(2) as BlockViewer;

        yield return new WaitForSeconds(0.1f);
        setPage(3);
        nextPage();

        onmess1 = selectVoice(3) as HatViewer;
        onmess2 = selectVoice(3) as HatViewer;

        yield return new WaitForSeconds(0.1f);
        setPage(6);

        vicino = selectVoice(4) as ReferenceViewer;
        mhello = selectVoice(5) as ReferenceViewer;
        mnope = selectVoice(6) as ReferenceViewer;
        yield return new WaitForSeconds(0.1f);
        seWindow.Close();

        //mount them
        yield return new WaitForSeconds(0.1f);

        slot = (go, num) =>
        {
            ReferenceSlotViewer res = null;
            Enumerable.Range(0, go.transform.childCount).ToList().ForEach((n) =>
            {
                if (res != null)
                    return;
                if (go.transform.GetChild(n).name.Equals("ReferenceSlot(Clone)"))
                {
                    if (num == 0)
                        res = go.transform.GetChild(n).GetComponent<ReferenceSlotViewer>();
                    else
                        num--;
                }
            });
            return res;
        };

        slot(onmess1, 0).Filler = mhello;
        yield return new WaitForSeconds(0.1f);
        onmess1.SnapNext(say);
        yield return new WaitForSeconds(0.1f);
        slot(say, 0).Filler = vicino;
        yield return new WaitForSeconds(0.1f);

        slot(onmess2, 0).Filler = mnope;
        yield return new WaitForSeconds(0.1f);
        onmess2.SnapNext(clear);
        yield return new WaitForSeconds(0.1f);
        actorWindow.Close();

        #endregion
        Scripting.ExecutionController.Instance.delay = 0;
        //Controller.EnvironmentController.Instance.ChangeModeEv();
    }

    private IEnumerator FactorialDemo()
    {
        yield return new WaitForSeconds(1);
        Controller.EnvironmentController.Instance.AddActor();
        FindObjectOfType<View.ActorViewer>().HitByBlueRay();
        var actorWindow = FindObjectOfType<View.ActorWindow>();

        #region Prepare_Variables
        //Prepare Variables
        actorWindow.locVarBtn.HitByBlueRay();
        yield return new WaitForSeconds(0.1f);

        var variableWindow = GameObject.Find("VarWindow(Clone)").GetComponent<VariableWindow>();
        variableWindow.addVarButton.HitByBlueRay();
        yield return new WaitForSeconds(0.1f);

        var entry1 = variableWindow.transform.GetChild(4).GetComponent<VariableEntry>();
        entry1.transform.GetChild(0).GetComponent<VRTextbox>().Text = "ACC";
        entry1.transform.GetChild(2).GetComponent<VRCombobox>().Selected = 1;
        entry1.transform.GetChild(1).GetComponent<VRTextbox>().Text = "1";
        variableWindow.addVarButton.HitByBlueRay();
        yield return new WaitForSeconds(0.1f);

        var entry2 = variableWindow.transform.GetChild(5).GetComponent<VariableEntry>();
        entry2.transform.GetChild(0).GetComponent<VRTextbox>().Text = "COUNT";
        entry2.transform.GetChild(2).GetComponent<VRCombobox>().Selected = 1;
        entry2.transform.GetChild(1).GetComponent<VRTextbox>().Text = "6";
        variableWindow.addVarButton.HitByBlueRay();
        yield return new WaitForSeconds(0.1f);
        var entry3 = variableWindow.transform.GetChild(6).GetComponent<VariableEntry>();
        entry3.transform.GetChild(0).GetComponent<VRTextbox>().Text = "ZERO";
        entry3.transform.GetChild(2).GetComponent<VRCombobox>().Selected = 1;
        entry3.transform.GetChild(1).GetComponent<VRTextbox>().Text = "0";
        variableWindow.addVarButton.HitByBlueRay();
        yield return new WaitForSeconds(0.1f);
        var entry4 = variableWindow.transform.GetChild(7).GetComponent<VariableEntry>();
        entry4.transform.GetChild(0).GetComponent<VRTextbox>().Text = "FACT";
        entry4.transform.GetChild(1).GetComponent<VRTextbox>().Text = "FACT";
        variableWindow.addVarButton.HitByBlueRay();
        yield return new WaitForSeconds(0.1f);
        var entry5 = variableWindow.transform.GetChild(8).GetComponent<VariableEntry>();
        entry5.transform.GetChild(0).GetComponent<VRTextbox>().Text = "MINUSONE";
        entry5.transform.GetChild(2).GetComponent<VRCombobox>().Selected = 1;
        entry5.transform.GetChild(1).GetComponent<VRTextbox>().Text = "-1";
        yield return new WaitForSeconds(0.1f);
        variableWindow.Close();
        #endregion
        #region Prepare_Pieces
        //Prepare pieces
        actorWindow.addSEBtn.HitByBlueRay();
        var seWindow = GameObject.Find("NewSelector(Clone)").GetComponent<View.ChooseScriptingElementWindow>();

        BlockViewer say, set, incre;
        MouthBlockViewer whileb;
        HatViewer onplay;
        ReferenceViewer mult, minusone, gt, zero;
        List<ReferenceViewer> count, acc;
        count = new List<ReferenceViewer>();
        acc = new List<ReferenceViewer>();

        Action<int> setPage = (num) => { seWindow.transform.GetChild(0).GetChild(0).GetChild(num).GetComponent<VRButton>().HitByBlueRay(); };
        Func<int, ScriptingElementViewer> selectVoice = (num) => { seWindow.transform.GetChild(0).GetChild(2).GetChild(num).GetComponent<VRButton>().HitByBlueRay(); return Controller.ActorController.lastSpawned; };
        Action nextPage = () => { seWindow.transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<VRButton>().HitByBlueRay(); };

        yield return new WaitForSeconds(0.1f);
        setPage(1);

        say = selectVoice(0) as BlockViewer;

        yield return new WaitForSeconds(0.1f);
        setPage(3);

        whileb = selectVoice(1) as MouthBlockViewer;
        onplay = selectVoice(7) as HatViewer;

        yield return new WaitForSeconds(0.1f);
        setPage(5);

        mult = selectVoice(2) as ReferenceViewer;
        gt = selectVoice(4) as ReferenceViewer;

        yield return new WaitForSeconds(0.1f);
        setPage(6);

        set = selectVoice(1) as BlockViewer;
        incre = selectVoice(3) as BlockViewer;

        Enumerable.Range(1, 3).ToList().ForEach((s) =>
        {
            count.Add(selectVoice(5) as ReferenceViewer);
            acc.Add(selectVoice(4) as ReferenceViewer);
        });
        zero = selectVoice(6) as ReferenceViewer;
        minusone = selectVoice(8) as ReferenceViewer;
        yield return new WaitForSeconds(0.1f);
        seWindow.Close();
        #endregion
        #region Mount_Pieces
        //mount them
        yield return new WaitForSeconds(0.5f);

        Func<ScriptingElementViewer, int, ReferenceSlotViewer> slot = (go, num) =>
        {
            ReferenceSlotViewer res = null;
            Enumerable.Range(0, go.transform.childCount).ToList().ForEach((n) =>
            {
                if (res != null)
                    return;
                if (go.transform.GetChild(n).name.Equals("ReferenceSlot(Clone)"))
                {
                    if (num == 0)
                        res = go.transform.GetChild(n).GetComponent<ReferenceSlotViewer>();
                    else
                        num--;
                }
            });
            return res;
        };

        onplay.SnapNext(whileb);
        yield return new WaitForSeconds(0.1f);
        slot(whileb, 0).Filler = gt;
        yield return new WaitForSeconds(0.1f);
        slot(gt, 0).Filler = count[0];
        yield return new WaitForSeconds(0.1f);
        slot(gt, 1).Filler = zero;
        yield return new WaitForSeconds(0.1f);
        whileb.SnapInnerNext(set);
        yield return new WaitForSeconds(0.1f);
        slot(set, 0).Filler = acc[0];
        yield return new WaitForSeconds(0.1f);
        slot(set, 1).Filler = mult;
        yield return new WaitForSeconds(0.1f);
        slot(mult, 0).Filler = count[1];
        yield return new WaitForSeconds(0.1f);
        slot(mult, 1).Filler = acc[1];
        yield return new WaitForSeconds(0.1f);
        set.SnapNext(incre);
        yield return new WaitForSeconds(0.1f);
        slot(incre, 0).Filler = count[2];
        yield return new WaitForSeconds(0.1f);
        slot(incre, 1).Filler = minusone;
        yield return new WaitForSeconds(0.1f);
        incre.SnapNext(say);
        yield return new WaitForSeconds(0.1f);
        slot(say, 0).Filler = acc[2];

        #endregion
        Scripting.ExecutionController.Instance.delay = 0.1f;
        //Controller.EnvironmentController.Instance.ChangeModeEv();
    }

    //private void OnActorWindowScriptingElementAdded(Scripting.ScriptingElement obj)
    //{
    //    string text = obj.Description;
    //    string secondText = null;
    //    List<ReferenceSlotViewer> refl;
    //    List<Option> optl;
    //    GameObject gameObject = null;
    //    if (obj is DoubleMouthBlock)
    //    {
    //        text = (string)obj.GetType().GetField("description", BindingFlags.Public | BindingFlags.Static).GetValue(null);
    //        secondText = (string)obj.GetType().GetField("secondDescription", BindingFlags.Public | BindingFlags.Static).GetValue(null);
    //        gameObject = ScartchResourceManager.instance.doubleMouthBlockViewer;
    //    }
    //    else if (obj is MouthBlock)
    //        gameObject = ScartchResourceManager.instance.mouthBlockViewer;
    //    else if (obj is Block)
    //        gameObject = ScartchResourceManager.instance.blockViewer;
    //    else if (obj is Reference)
    //        gameObject = ScartchResourceManager.instance.referenceViewer;
    //    else if (obj is Hat)
    //        gameObject = ScartchResourceManager.instance.hatViewer;
    //    ScriptingElementViewer viewer = GameObject.Instantiate(gameObject).GetComponent<ScriptingElementViewer>();
    //    Scripting.ScriptingElement.GenerateViewersFromText(ref text, viewer.gameObject, out refl, out optl);
    //    viewer.GetType().GetProperty("Text").SetValue(viewer, text, null);
    //    if (obj is DoubleMouthBlock)
    //        viewer.GetType().GetProperty("SecText").SetValue(viewer, secondText, null);
    //    ScriptingElement elem = (ScriptingElement)System.Activator.CreateInstance(obj.GetType(), this.actor, optl, refl, viewer, false);
    //    if (obj is VariableReference)
    //        (elem as VariableReference).Variable = (obj as VariableReference).Variable;
    //    viewer.Init(elem);
        
    //}
}
