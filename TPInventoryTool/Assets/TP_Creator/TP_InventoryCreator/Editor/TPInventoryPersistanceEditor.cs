using UnityEditor;
using TP_Inventory;

namespace TP_InventoryEditor
{
    [CustomEditor(typeof(TPInventoryPersistance))]
    public class TPInventoryPersistanceEditor : ScriptlessEditor
    {
        TPInventoryPersistance InventoryPersistance;

        void OnEnable()
        {
            InventoryPersistance = target as TPInventoryPersistance;

            if (InventoryPersistance.inventoryData == null)
            {
                InventoryPersistance.inventoryData =
                    (TPInventoryData)AssetDatabase.LoadAssetAtPath("Assets/TP_Creator/TP_InventoryCreator/InventoryData/InventoryData.asset", typeof(TPInventoryData));
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("Script for Inventory Data");

            serializedObject.ApplyModifiedProperties();
        }
    }
}