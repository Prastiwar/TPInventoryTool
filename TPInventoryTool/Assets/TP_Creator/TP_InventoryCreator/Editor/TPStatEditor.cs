using TP_Inventory;
using UnityEditor;

namespace TP_InventoryEditor
{
    [CustomEditor(typeof(TPStat))]
    internal class TPStatEditor : ScriptlessInventoryEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Statistic: " + serializedObject.targetObject.name);

            if (TPInventoryCreator.DebugMode)
                DrawPropertiesExcluding(serializedObject, scriptField);

            OpenCreator();
        }
    }
}