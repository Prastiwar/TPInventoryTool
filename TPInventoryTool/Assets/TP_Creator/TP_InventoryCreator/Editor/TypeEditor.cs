using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Type))]
public class TypeEditor : ScriptlessEditor
{
    public override void OnInspectorGUI()
    {
        //EditorGUILayout.LabelField("Statistic");
        base.OnInspectorGUI();
    }
}
