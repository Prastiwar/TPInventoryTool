using UnityEditor;

namespace TP.InventoryEditor
{
    [CustomEditor(typeof(TPInventoryData))]
    internal class TPInventoryDataEditor : ScriptlessInventoryEditor
    {
        void OnEnable()
        {
            if (serializedObject.targetObject.hideFlags != UnityEngine.HideFlags.NotEditable)
                serializedObject.targetObject.hideFlags = UnityEngine.HideFlags.NotEditable;
        }
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Inventory Data");
            DrawPropertiesExcluding(serializedObject, scriptField);
        }
    }
}