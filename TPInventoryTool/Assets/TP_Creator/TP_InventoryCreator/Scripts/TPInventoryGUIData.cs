using UnityEngine;

[CreateAssetMenu(menuName = "TP_InventoryTool/gui", fileName = "New gui")]
public class TPInventoryGUIData : ScriptableObject
{
    [HideInInspector] public GUISkin GUISkin;
    [HideInInspector] public string InventoryDataPath;
    [HideInInspector] public string InventoryAssetsPath;
    [HideInInspector] public GameObject InventoryPrefab;
}