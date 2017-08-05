using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;


public class VariableViewer : MonoBehaviour {

    public UnityEngine.UI.Text varname, scope, value;
    private Variable myVar;

	void Start () {
        GetComponentInChildren<Closable>().Closed += Dispose;	
	}

    private void Dispose()
    {
        myVar.VariableChanged -= UpdateValue;
        Destroy(this.gameObject);
    }

    public void Init(Variable var)
    {
        var.VariableChanged += UpdateValue;
        varname.text = var.Name;
        // TODO meglio implementare una relazione bidirezionale?
        string owner = null;
        if (model.Environment.Instance.Variables.Contains(var))
            owner = "Globale";
        else
            model.Environment.Instance.Actors.ForEach(s => { if (s.variables.Contains(var)) owner = "Locale di " + s.Name; });
        scope.text = owner;
        myVar = var;
        UpdateValue(var, EventArgs.Empty);
    }

    private void UpdateValue(object sender, EventArgs e)
    {
        value.text = (sender as Variable).EvaluateAsString();
    }
}
