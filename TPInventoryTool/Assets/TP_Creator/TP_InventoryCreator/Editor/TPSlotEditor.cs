using UnityEditor;
using TP_Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace TP_InventoryEditor
{
    [CustomEditor(typeof(TPSlot))]
    internal class TPSlotEditor : ScriptlessInventoryEditor
    {
        TPSlot slot;
        void OnEnable()
        {
            slot = target as TPSlot;
        }

        public override void OnInspectorGUI()
        {
            if (TPInventoryCreator.DebugMode)
            {
                if (slot.GetComponent<CanvasRenderer>().hideFlags != HideFlags.NotEditable)
                    slot.GetComponent<CanvasRenderer>().hideFlags = HideFlags.NotEditable;
                if (slot.GetComponent<CanvasGroup>().hideFlags != HideFlags.NotEditable)
                    slot.GetComponent<CanvasGroup>().hideFlags = HideFlags.NotEditable;
                if (slot.GetComponent<Image>().hideFlags != HideFlags.NotEditable)
                    slot.GetComponent<Image>().hideFlags = HideFlags.NotEditable;
            }
            else
            {
                if (slot.GetComponent<CanvasRenderer>().hideFlags != HideFlags.HideInInspector)
                    slot.GetComponent<CanvasRenderer>().hideFlags = HideFlags.HideInInspector;
                if (slot.GetComponent<CanvasGroup>().hideFlags != HideFlags.HideInInspector)
                    slot.GetComponent<CanvasGroup>().hideFlags = HideFlags.HideInInspector;
                if (slot.GetComponent<Image>().hideFlags != HideFlags.HideInInspector)
                    slot.GetComponent<Image>().hideFlags = HideFlags.HideInInspector;
            }
            EditorGUILayout.LabelField("Inventory Slot");

            if (TPInventoryCreator.DebugMode)
                DrawPropertiesExcluding(serializedObject, scriptField);

            OpenCreator();
        }
    }
}