using UnityEditor;
using TP_Inventory;

namespace TP_InventoryEditor
{
    [CustomEditor(typeof(TPSlot))]
    public class TPSlotEditor : ScriptlessEditor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("Inventory Slot");

            DrawPropertiesExcluding(serializedObject, scriptField);
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}