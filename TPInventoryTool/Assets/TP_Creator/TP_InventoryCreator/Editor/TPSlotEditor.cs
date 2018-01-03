using UnityEditor;
using TP_Inventory;

[CustomEditor(typeof(TPSlot))]
public class TPSlotEditor : ScriptlessEditor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //EditorGUILayout.LabelField("Statistic");
        base.OnInspectorGUI();

        serializedObject.ApplyModifiedProperties();
    }
}
