using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Stat))]
public class StatEditor : ScriptlessEditor
{
    public override void OnInspectorGUI()
    {
        //EditorGUILayout.LabelField("Statistic");
        base.OnInspectorGUI();
    }
}