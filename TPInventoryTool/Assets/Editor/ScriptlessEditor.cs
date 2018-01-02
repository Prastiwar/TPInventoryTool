using UnityEditor;

public class ScriptlessEditor : Editor
{
    public readonly string[] scriptField = new string[] { "m_Script" };

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        DrawPropertiesExcluding(serializedObject, scriptField);

        serializedObject.ApplyModifiedProperties();
    }
}
