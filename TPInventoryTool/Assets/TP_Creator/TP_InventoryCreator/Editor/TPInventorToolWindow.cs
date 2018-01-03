using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TP_Inventory;
using UnityEngine.Events;
using System;

public class TPInventorToolWindow : EditorWindow
{
    public enum ToolEnum
    {
        Items,
        Types,
        Stats,
        Slots
    }
    static ToolEnum tool;
    static TPInventoryCreator inventoryCreator;
    static TPInventorToolWindow window;

    Vector2 scrollPos = Vector2.zero;

    static object _object;
    static string loaded;
    static string noLoaded;
    static string horizontalVar;
    static Array array;
    static UnityAction action;
    static Type type;

    public static void OpenToolWindow(ToolEnum _tool)
    {
        window = (TPInventorToolWindow)GetWindow(typeof(TPInventorToolWindow));
        window.minSize = new Vector2(300, 400);
        window.Show();
        tool = _tool;
        inventoryCreator = FindObjectOfType<TPInventoryCreator>();
        SetToolWindow();
    }

    void OnGUI()
    {
        scrollPos = GUILayout.BeginScrollView(scrollPos, false, false, GUILayout.Width(window.position.width), GUILayout.Height(window.position.height));
        DrawTool();
        GUILayout.EndScrollView();
    }

    static void SetToolWindow()
    {
        switch (tool)
        {
            case ToolEnum.Items:
                loaded = "Items Loaded";
                noLoaded = "No Items Loaded!";
                horizontalVar = "coś";
                array = inventoryCreator.InventoryPersistance.inventoryData.Items.ToArray();
                action = DrawItems;
                type = typeof(TPItem);
                break;
            case ToolEnum.Types:
                loaded = "Types Loaded";
                noLoaded = "No Types Loaded!";
                horizontalVar = "";
                array = inventoryCreator.InventoryPersistance.inventoryData.Types.ToArray();
                action = DrawTypes;
                type = typeof(TPType);
                break;
            case ToolEnum.Stats:
                loaded = "Stats Loaded";
                noLoaded = "No Stats Loaded!";
                horizontalVar = "Value";
                array = inventoryCreator.InventoryPersistance.inventoryData.Stats.ToArray();
                action = DrawStats;
                type = typeof(TPStat);
                break;
            case ToolEnum.Slots:
                loaded = "Slots Loaded";
                noLoaded = "No Slots Loaded!";
                horizontalVar = "Is Equip Slot";
                array = inventoryCreator.Slots.ToArray();
                action = DrawSlots;
                type = typeof(TPSlot);
                break;
            default:
                break;
        }
    }

    void DrawTool()
    {
        if (GUILayout.Button("Create"))
        {
            CreateScriptable();
        }

        if (array.Length == 0)
        {
            EditorGUILayout.HelpBox(noLoaded + " Try to Refresh/Update Manager or create one!", MessageType.Error);
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
        EditorGUILayout.Toggle((_object as TPSlot).isEquipSlot);
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
        if (GUILayout.Button("Edit"))
        {
            //(_object as TPItem).GetInstanceID();
        }
    }
    
    void CreateScriptable()
    {
        string assetPath = TPInventoryDesigner.DataPath;

        UnityEngine.Object newObj = null;

        switch (tool)
        {
            case ToolEnum.Items:
                newObj = ScriptableObject.CreateInstance<TPItem>();
                assetPath += "Items/New Item" + inventoryCreator.InventoryPersistance.inventoryData.Items.Count + ".asset";
                break;
            case ToolEnum.Types:
                newObj = ScriptableObject.CreateInstance<TPType>();
                assetPath += "Types/New Type" + inventoryCreator.InventoryPersistance.inventoryData.Types.Count + ".asset";
                break;
            case ToolEnum.Stats:
                newObj = ScriptableObject.CreateInstance<TPStat>();
                assetPath += "Stats/New Stat" + inventoryCreator.InventoryPersistance.inventoryData.Stats.Count + ".asset";
                break;
            case ToolEnum.Slots:
                break;
            default:
                break;
        }

        AssetDatabase.CreateAsset(newObj, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(newObj + " created in " + assetPath);
        TPInventoryDesigner.UpdateManager();
        SetToolWindow();
    }
}