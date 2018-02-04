using TP.Inventory;
using UnityEditor;

namespace TP.InventoryEditor
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