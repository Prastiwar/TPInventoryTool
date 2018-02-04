using UnityEditor;
using TP.Inventory;

namespace TP.InventoryEditor
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