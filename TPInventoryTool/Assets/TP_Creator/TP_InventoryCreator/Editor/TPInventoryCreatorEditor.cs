using TP_Inventory;
using UnityEditor;

namespace TP_InventoryEditor
{
    [CustomEditor(typeof(TPInventoryCreator))]
    internal class TPInventoryCreatorEditor : ScriptlessInventoryEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("This script allows you to manage your Inventory");

            if(TPInventoryCreator.DebugMode)
                DrawPropertiesExcluding(serializedObject, scriptField);

            OpenCreator();
        }
    }
}