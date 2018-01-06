using UnityEditor;

namespace TP_InventoryEditor
{
    public class ScriptlessEditor : Editor
    {
        public readonly string[] scriptField = new string[] { "m_Script" };

        public override void OnInspectorGUI()
        {
            DrawPropertiesExcluding(serializedObject, scriptField);
        }
    }
}