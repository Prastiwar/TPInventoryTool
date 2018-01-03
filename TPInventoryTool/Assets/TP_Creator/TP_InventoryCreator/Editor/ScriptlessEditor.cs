using System.Collections.Generic;
using UnityEditor;

public class ScriptlessEditor : Editor
{
    public readonly string[] scriptField = new string[] { "m_Script" };

    public override void OnInspectorGUI()
    {
        DrawPropertiesExcluding(serializedObject, scriptField);
    }

    public List<T> FindAssetsByType<T>() where T : UnityEngine.Object
    {
        List<T> assets = new List<T>();
        string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (asset != null)
            {
                assets.Add(asset);
            }
        }
        return assets;
    }
}
