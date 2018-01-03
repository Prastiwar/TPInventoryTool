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
                array = inventoryCreator.InventorySaveLoad.Items;
                action = DrawItems;
                type = typeof(TPItem);
                break;
            case ToolEnum.Types:
                loaded = "Types Loaded";
                noLoaded = "No Types Loaded!";
                horizontalVar = "";
                array = inventoryCreator.InventorySaveLoad.Types;
                action = DrawTypes;
                type = typeof(TPType);
                break;
            case ToolEnum.Stats:
                loaded = "Stats Loaded";
                noLoaded = "No Stats Loaded!";
                horizontalVar = "Value";
                array = inventoryCreator.InventorySaveLoad.Stats;
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
        string dataPath = "Assets/TP_Creator/TP_InventoryCreator/InventoryData/";

        UnityEngine.Object newObj = null;

        switch (tool)
        {
            case ToolEnum.Items:
                newObj = ScriptableObject.CreateInstance<TPItem>();
                dataPath += "Items/New Item.asset";
                break;
            case ToolEnum.Types:
                newObj = ScriptableObject.CreateInstance<TPType>();
                dataPath += "Types/New Type.asset";
                break;
            case ToolEnum.Stats:
                newObj = ScriptableObject.CreateInstance<TPStat>();
                dataPath += "Stats/New Stat.asset";
                break;
            case ToolEnum.Slots:
                break;
            default:
                break;
        }

        AssetDatabase.CreateAsset(newObj, dataPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}