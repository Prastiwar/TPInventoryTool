using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TP_Inventory_Slot;

[CustomEditor(typeof(Slot))]
public class SlotEditor : ScriptlessEditor
{
    public override void OnInspectorGUI()
    {
        //EditorGUILayout.LabelField("Statistic");
        base.OnInspectorGUI();
    }
}
