using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TPType))]
public class TPTypeEditor : ScriptlessEditor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Type");
        base.OnInspectorGUI();

        serializedObject.ApplyModifiedProperties();
    }
}