using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono;
using System;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Net;

using System.CodeDom.Compiler;

using UnityEditor;

[CustomEditor(typeof(welo))]
[CanEditMultipleObjects]
public class editorfr : Editor
{
    SerializedProperty lookAtPoint;

    void OnEnable()
    {
       
    }

    private bool vis = false;
    private string code1 = @"
namespace model {
    public class ";
    private string code2 = @" {
public string hey(){";
    private string code3 = "} } }";
    private string nombre, code;
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.LabelField("Class Name");
        nombre = EditorGUILayout.TextArea(nombre);
        EditorGUILayout.LabelField("Execute()");
        code = EditorGUILayout.TextArea(code);
        if (GUILayout.Button("Compute"))
            vis = !vis;
        if (vis)
            EditorGUILayout.TextArea(System.Environment.Version.ToString());
        serializedObject.ApplyModifiedProperties();
        

    }
}

