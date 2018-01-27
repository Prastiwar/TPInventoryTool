using UnityEditor;
using TP_Inventory;

namespace TP_InventoryEditor
{
    [CustomEditor(typeof(TPType))]
    internal class TPTypeEditor : ScriptlessInventoryEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Inventory Type: " + serializedObject.targetObject.name);

            OpenCreator();
        }
    }
}