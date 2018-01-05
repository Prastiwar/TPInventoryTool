using UnityEngine;
using UnityEditor;
using TP_Inventory;
using UnityEngine.Events;
using System;

public class TPInventoryToolsWindow : EditorWindow
{
    public static TPInventoryToolsWindow window;
    public enum ToolEnum
    {
        Items,
        Types,
        Stats,
        Slots
    }
    static ToolEnum tool;
    static object _object;
    static string loaded;
    static string noLoaded;
    static string horizontalVar;
    static Array array;
    static UnityAction action;
    static Type type;

    Vector2 scrollPos = Vector2.zero;

    public static void OpenToolWindow(ToolEnum _tool)
    {
        window = (TPInventoryToolsWindow)GetWindow(typeof(TPInventoryToolsWindow));
        window.minSize = new Vector2(300, 400);
        window.Show();
        tool = _tool;
        SetToolWindow();
    }

    void OnGUI()
    {
        scrollPos = GUILayout.BeginScrollView(scrollPos, false, false, GUILayout.Width(window.position.width), GUILayout.Height(window.position.height));
        DrawTool();
        GUILayout.EndScrollView();
    }

    public static void SetToolWindow()
    {
        switch (tool)
        {
            case ToolEnum.Items:
                loaded = "Items Loaded";
                noLoaded = "No Items Loaded! Try to Refresh / Update Manager or create one!";
                horizontalVar = "";
                array = TPInventoryDesigner.inventoryCreator.InventoryPersistance.inventoryData.Items.ToArray();
                action = DrawItems;
                type = typeof(TPItem);
                break;
            case ToolEnum.Types:
                loaded = "Types Loaded";
                noLoaded = "No Types Loaded! Try to Refresh / Update Manager or create one!";
                horizontalVar = "";
                array = TPInventoryDesigner.inventoryCreator.InventoryPersistance.inventoryData.Types.ToArray();
                action = DrawTypes;
                type = typeof(TPType);
                break;
            case ToolEnum.Stats:
                loaded = "Stats Loaded";
                noLoaded = "No Stats Loaded! Try to Refresh / Update Manager or create one!";
                horizontalVar = "Value";
                array = TPInventoryDesigner.inventoryCreator.InventoryPersistance.inventoryData.Stats.ToArray();
                action = DrawStats;
                type = typeof(TPStat);
                break;
            case ToolEnum.Slots:
                loaded = "Slots Loaded";
                noLoaded = "No Slots Loaded! Try to Refresh / Update Manager or change slot parent!";
                horizontalVar = "Is Equip Slot";
                array = TPInventoryDesigner.inventoryCreator.Slots.ToArray();
                action = DrawSlots;
                type = typeof(TPSlot);
                break;
            default:
                break;
        }
    }

    void DrawTool()
    {
        if (tool != ToolEnum.Slots)
        {
            if (GUILayout.Button("Create new", TPInventoryDesigner.editorData.GUISkin.button))
            {
                CreateScriptable();
            }
        }
        if (array.Length == 0)
        {
            EditorGUILayout.HelpBox(noLoaded, MessageType.Error);
            return;
        }
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(loaded, GUILayout.MinWidth(150));
        EditorGUILayout.LabelField(horizontalVar);
        GUILayout.EndHorizontal();

        foreach (UnityEngine.Object element in array)
        {
            _object = element;
            GUILayout.BeginHorizontal();
            EditorGUILayout.ObjectField(element, type, false);
            action();
            GUILayout.EndHorizontal();
            EditorUtility.SetDirty(_object as UnityEngine.Object);
        }
    }

    static void DrawSlots()
    {
        (_object as TPSlot).isEquipSlot = EditorGUILayout.Toggle((_object as TPSlot).isEquipSlot);
    }

    static void DrawStats()
    {
        (_object as TPStat).Value = EditorGUILayout.FloatField((_object as TPStat).Value);
    }

    static void DrawTypes()
    {
    }

    static void DrawItems()
    {
    }
    
    void CreateScriptable()
    {
        string assetPath = "Assets/" + TPInventoryDesigner.editorData.InventoryAssetsPath;

        UnityEngine.Object newObj = null;

        switch (tool)
        {
            case ToolEnum.Items:
                newObj = ScriptableObject.CreateInstance<TPItem>();
                assetPath += "Items/New Item" + TPInventoryDesigner.inventoryCreator.InventoryPersistance.inventoryData.Items.Count + ".asset";
                break;
            case ToolEnum.Types:
                newObj = ScriptableObject.CreateInstance<TPType>();
                assetPath += "Types/New Type" + TPInventoryDesigner.inventoryCreator.InventoryPersistance.inventoryData.Types.Count + ".asset";
                break;
            case ToolEnum.Stats:
                newObj = ScriptableObject.CreateInstance<TPStat>();
                assetPath += "Stats/New Stat" + TPInventoryDesigner.inventoryCreator.InventoryPersistance.inventoryData.Stats.Count + ".asset";
                break;
            case ToolEnum.Slots:
                break;
            default:
                break;
        }

        if (AssetDatabase.IsValidFolder("Assets/" + TPInventoryDesigner.editorData.InventoryAssetsPath))
            Debug.Log("This path doesn't exist, create one!");

        AssetDatabase.CreateAsset(newObj, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(newObj.name + " created in " + assetPath);
        TPInventoryDesigner.UpdateManager();
        SetToolWindow();
    }
}