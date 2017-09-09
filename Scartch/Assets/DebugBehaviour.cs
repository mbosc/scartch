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
    void Start() {
        StartCoroutine(FactorialDemo());
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
        yield return new WaitForSeconds(0.5f);
        var variableWindow = GameObject.Find("VarWindow(Clone)").GetComponent<VariableWindow>();
        variableWindow.addVarButton.HitByBlueRay();
        yield return new WaitForSeconds(0.5f);
        var entry1 = variableWindow.transform.GetChild(4).GetComponent<VariableEntry>();
        entry1.transform.GetChild(0).GetComponent<VRTextbox>().Text = "ACC";
        entry1.transform.GetChild(2).GetComponent<VRCombobox>().Selected = 1;
        entry1.transform.GetChild(1).GetComponent<VRTextbox>().Text = "1";
        variableWindow.addVarButton.HitByBlueRay();
        yield return new WaitForSeconds(0.5f);
        var entry2 = variableWindow.transform.GetChild(5).GetComponent<VariableEntry>();
        entry2.transform.GetChild(0).GetComponent<VRTextbox>().Text = "COUNT";
        entry2.transform.GetChild(2).GetComponent<VRCombobox>().Selected = 1;
        entry2.transform.GetChild(1).GetComponent<VRTextbox>().Text = "6";
        variableWindow.addVarButton.HitByBlueRay();
        yield return new WaitForSeconds(0.5f);
        var entry3 = variableWindow.transform.GetChild(6).GetComponent<VariableEntry>();
        entry3.transform.GetChild(0).GetComponent<VRTextbox>().Text = "ZERO";
        entry3.transform.GetChild(2).GetComponent<VRCombobox>().Selected = 1;
        entry3.transform.GetChild(1).GetComponent<VRTextbox>().Text = "0";
        variableWindow.addVarButton.HitByBlueRay();
        yield return new WaitForSeconds(0.5f);
        var entry4 = variableWindow.transform.GetChild(7).GetComponent<VariableEntry>();
        entry4.transform.GetChild(0).GetComponent<VRTextbox>().Text = "FACT";
        entry4.transform.GetChild(1).GetComponent<VRTextbox>().Text = "FACT";
        variableWindow.addVarButton.HitByBlueRay();
        yield return new WaitForSeconds(0.5f);
        var entry5 = variableWindow.transform.GetChild(8).GetComponent<VariableEntry>();
        entry5.transform.GetChild(0).GetComponent<VRTextbox>().Text = "MINUSONE";
        entry5.transform.GetChild(2).GetComponent<VRCombobox>().Selected = 1;
        entry5.transform.GetChild(1).GetComponent<VRTextbox>().Text = "-1";
        yield return new WaitForSeconds(0.5f);
        variableWindow.Close();
        #endregion
        #region Prepare_Pieces
        //Prepare pieces
        actorWindow.addSEBtn.HitByBlueRay();
        var seWindow = GameObject.Find("NewSelector(Clone)").GetComponent<View.ChooseScriptingElementWindow>();

        BlockViewer say1, say2, set, incre, brow1, brow2;
        DoubleMouthBlockViewer ife1, ife2;
        HatViewer onmess, onplay;
        ReferenceViewer mult, minusone, lt, eq;
        List<ReferenceViewer> count, acc, fact, zero;
        count = new List<ReferenceViewer>();
        acc = new List<ReferenceViewer>();
        fact = new List<ReferenceViewer>();
        zero = new List<ReferenceViewer>();

        Action<int> setPage = (num) => { seWindow.transform.GetChild(0).GetChild(0).GetChild(num).GetComponent<VRButton>().HitByBlueRay(); };
        Func<int, ScriptingElementViewer> selectVoice = (num) => { seWindow.transform.GetChild(0).GetChild(2).GetChild(num).GetComponent<VRButton>().HitByBlueRay(); return Controller.ActorController.lastSpawned; };
        Action nextPage = () => { seWindow.transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<VRButton>().HitByBlueRay(); };

        yield return new WaitForSeconds(0.5f);
        setPage(1);

        say1 = selectVoice(0) as BlockViewer;
        say2 = selectVoice(0) as BlockViewer;

        yield return new WaitForSeconds(0.5f);
        setPage(3);

        ife1 = selectVoice(4) as DoubleMouthBlockViewer;
        ife2 = selectVoice(4) as DoubleMouthBlockViewer;
        onplay = selectVoice(7) as HatViewer;

        yield return new WaitForSeconds(0.5f);
        nextPage();

        brow1 = selectVoice(2) as BlockViewer;
        brow2 = selectVoice(2) as BlockViewer;
        onmess = selectVoice(3) as HatViewer;

        yield return new WaitForSeconds(0.5f);
        setPage(5);

        mult = selectVoice(2) as ReferenceViewer;
        lt = selectVoice(5) as ReferenceViewer;
        eq = selectVoice(8) as ReferenceViewer;

        yield return new WaitForSeconds(0.5f);
        setPage(6);

        set = selectVoice(1) as BlockViewer;
        incre = selectVoice(3) as BlockViewer;

        Enumerable.Range(1, 3).ToList().ForEach((s) =>
        {
            count.Add(selectVoice(5) as ReferenceViewer);
            acc.Add(selectVoice(4) as ReferenceViewer);
            fact.Add(selectVoice(7) as ReferenceViewer);
            zero.Add(selectVoice(6) as ReferenceViewer);
        });
        count.Add(selectVoice(5) as ReferenceViewer);
        minusone = selectVoice(8) as ReferenceViewer;
        yield return new WaitForSeconds(0.5f);
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

        onplay.SnapNext(brow1);
        slot(brow1, 0).Filler = fact[0];
        slot(onmess, 0).Filler = fact[1];

        onmess.SnapNext(ife1);
        slot(ife1, 0).Filler = lt;
        slot(lt, 0).Filler = count[0];
        slot(lt, 1).Filler = zero[0];
        ife1.SnapUpperInnerNext(say1);
        slot(say1, 0).Filler = zero[1];
        ife1.SnapLowerInnerNext(ife2);
        slot(ife2, 0).Filler = eq;
        slot(eq, 0).Filler = count[1];
        slot(eq, 1).Filler = zero[2];
        ife2.SnapUpperInnerNext(say2);
        slot(say2, 0).Filler = acc[0];
        ife2.SnapLowerInnerNext(incre);
        incre.SnapNext(set);
        slot(incre, 0).Filler = count[2];
        slot(incre, 1).Filler = minusone;
        slot(set, 0).Filler = acc[2];
        slot(set, 1).Filler = mult;
        slot(mult, 0).Filler = count[3];
        slot(mult, 1).Filler = acc[1];
        set.SnapNext(brow2);
        slot(brow2, 0).Filler = fact[2];
        #endregion
        Controller.EnvironmentController.Instance.ChangeModeEv();
    }

    private IEnumerator WaitAndLaunch()
    {
        actor = new Actor();
        //Controller.EnvironmentController.Instance.AddActor();
        yield return new WaitForSeconds(1);
        //FindObjectOfType<View.ActorViewer>().HitByBlueRay();
        //yield return new WaitForSeconds(1);
        //FindObjectOfType<View.ActorWindow>().addSEBtn.HitByBlueRay();
        //FindObjectOfType<View.ChooseScriptingElementWindow>().bookmarks.ForEach(x => Debug.Log(x.name));
        //FindObjectOfType<View.ChooseScriptingElementWindow>().bookmarks[0].HitByBlueRay();

        //FindObjectOfType<View.ChooseScriptingElementWindow>().voices[6].HitByBlueRay();

        //OnActorWindowScriptingElementAdded(ScartchResourceManager.instance.movementElements[6]);
        //FindObjectOfType<VRComboboxVoice>().HitByBlueRay();

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
        Scripting.ScriptingElement.GenerateViewersFromText(ref text, viewer.gameObject, out refl, out optl);
        viewer.GetType().GetProperty("Text").SetValue(viewer, text, null);
        if (obj is DoubleMouthBlock)
            viewer.GetType().GetProperty("SecText").SetValue(viewer, secondText, null);
        ScriptingElement elem = (ScriptingElement)System.Activator.CreateInstance(obj.GetType(), this.actor, optl, refl, viewer, false);
        if (obj is VariableReference)
            (elem as VariableReference).Variable = (obj as VariableReference).Variable;
        viewer.Init(elem);
        
    }
}
