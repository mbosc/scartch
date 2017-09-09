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

        yield return new WaitForSeconds(0.5f);
        setPage(1);

        say = selectVoice(0) as BlockViewer;

        yield return new WaitForSeconds(0.5f);
        setPage(3);

        whileb = selectVoice(1) as MouthBlockViewer;
        onplay = selectVoice(7) as HatViewer;

        yield return new WaitForSeconds(0.5f);
        setPage(5);

        mult = selectVoice(2) as ReferenceViewer;
        gt = selectVoice(4) as ReferenceViewer;

        yield return new WaitForSeconds(0.5f);
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

        onplay.SnapNext(whileb);
        slot(whileb, 0).Filler = gt;
        slot(gt, 0).Filler = count[0];
        slot(gt, 1).Filler = zero;
        whileb.SnapInnerNext(set);
        slot(set, 0).Filler = acc[0];
        slot(set, 1).Filler = mult;
        slot(mult, 0).Filler = count[1];
        slot(mult, 1).Filler = acc[1];
        set.SnapNext(incre);
        slot(incre, 0).Filler = count[2];
        slot(incre, 1).Filler = minusone;
        whileb.SnapNext(say);
        slot(say, 0).Filler = acc[2];
        
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
