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
        StartCoroutine(WaitAndLaunch());
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
