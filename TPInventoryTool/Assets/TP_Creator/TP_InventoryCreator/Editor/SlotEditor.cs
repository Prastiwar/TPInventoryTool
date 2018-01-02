using UnityEditor;
using TP_Inventory;

[CustomEditor(typeof(TPSlot))]
public class SlotEditor : ScriptlessEditor
{
    public override void OnInspectorGUI()
    {
        //EditorGUILayout.LabelField("Statistic");
        base.OnInspectorGUI();
    }
}
