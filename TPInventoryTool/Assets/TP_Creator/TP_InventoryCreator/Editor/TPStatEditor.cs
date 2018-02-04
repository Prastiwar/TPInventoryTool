using TP.Inventory;
using UnityEditor;

namespace TP.InventoryEditor
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