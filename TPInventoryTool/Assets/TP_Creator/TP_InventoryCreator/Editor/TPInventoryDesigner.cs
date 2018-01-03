using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using TP_Inventory;
using System;

public class TPInventoryDesigner : EditorWindow
{
    static TPInventoryCreator inventoryCreator;
    public static string DataPath = "Assets/TP_Creator/TP_InventoryCreator/InventoryData/";
    GUISkin skin;

    Texture2D headerTexture;
    Texture2D managerTexture;
    Texture2D toolTexture;

    Rect headerSection;
    Rect managerSection;
    Rect toolSection;

    bool toggleCustom;

    [MenuItem("TP_Creator/TP_InventoryCreator")]
    public static void OpenWindow()
    {
        TPInventoryDesigner window = (TPInventoryDesigner)GetWindow(typeof(TPInventoryDesigner));
        window.minSize = new Vector2(600, 300);
        window.Show();
    }

    void OnEnable()
    {
        skin = Resources.Load<GUISkin>("TP_GUISkin");

        headerTexture = new Texture2D(1, 1);
        headerTexture.SetPixel(0, 0, Color.gray);
        headerTexture.Apply();

        managerTexture = new Texture2D(1, 1);
        managerTexture.SetPixel(0, 0, Color.black);
        managerTexture.Apply();

        toolTexture = new Texture2D(1, 1);
        toolTexture.SetPixel(0, 0, Color.white);
        toolTexture.Apply();

        InitCreator();
    }

    static void InitCreator()
    {
        if (inventoryCreator == null)
        {
            inventoryCreator = FindObjectOfType<TPInventoryCreator>();

            if (inventoryCreator != null)
                UpdateManager();
        }

        var data = AssetDatabase.LoadAssetAtPath(DataPath, typeof(TPInventoryData));
        if (data == null)
        {
            TPInventoryData newData = ScriptableObject.CreateInstance<TPInventoryData>();
            AssetDatabase.CreateAsset(newData, DataPath + "InventoryData.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            inventoryCreator.InventoryPersistance.inventoryData = newData;
        }
    }

    void OnGUI()
    {
        DrawLayouts();
        DrawHeader();
        DrawManager();
        DrawTools();
    }

    void DrawLayouts()
    {
        headerSection = new Rect(0, 0, Screen.width, 50);
        managerSection = new Rect(0, 50, Screen.width / 2, Screen.height);
        toolSection = new Rect(Screen.width / 2, 50, Screen.width / 2, Screen.height);

        GUI.DrawTexture(headerSection, headerTexture);
        GUI.DrawTexture(managerSection, managerTexture);
        GUI.DrawTexture(toolSection, toolTexture);
    }

    void DrawHeader()
    {
        GUILayout.BeginArea(headerSection);
        GUILayout.Label("TP Inventory Creator - Manage your Inventory!");
        if (GUILayout.Button("Generate empty inventory hierarchy"))
        {
            // load prefab from resources
            Debug.Log("Inventory Created");
        }
        GUILayout.EndArea();
    }

    void DrawManager()
    {
        GUILayout.BeginArea(managerSection);
        GUILayout.Label("Inventory Manager - Core");

        if (inventoryCreator == null)
        {
            if (GUILayout.Button("Initialize Inventory Manager"))
            {
                GameObject go = (new GameObject("TP_InventoryManager", typeof(TPInventoryCreator), typeof(TPInventoryPersistance)));
                inventoryCreator = go.GetComponent<TPInventoryCreator>();
                UpdateManager();
                Debug.Log("Inventory Manager created and updated");
            }
        }
        else
        {
            if (GUILayout.Button("Custom Inventory"))
                toggleCustom = !toggleCustom;

            if (toggleCustom)
            {
                EditorGUILayout.LabelField("Put there parent of all slot's parent");
                inventoryCreator.slotTransform =
                    (EditorGUILayout.ObjectField(inventoryCreator.slotTransform.gameObject, typeof(GameObject), true) as GameObject).transform;
            }

            if (GUILayout.Button("Update Manager", skin.GetStyle("Header")))
            {
                UpdateManager();
            }
        }

        GUILayout.EndArea();
    }

    public static void UpdateManager()
    {
        InitCreator();
        EditorUtility.SetDirty(inventoryCreator);
        EditorUtility.SetDirty(inventoryCreator.InventoryPersistance);
        EditorUtility.SetDirty(inventoryCreator.slotTransform);
        EditorUtility.SetDirty(inventoryCreator.InventoryPersistance.inventoryData);
        Debug.Log("Inventory Manager updated");
    }

    void DrawTools()
    {
        GUILayout.BeginArea(toolSection);
        if (GUILayout.Button("Items"))
        {
            TPInventorToolWindow.OpenToolWindow(TPInventorToolWindow.ToolEnum.Items);
        }
        if (GUILayout.Button("Types"))
        {
            TPInventorToolWindow.OpenToolWindow(TPInventorToolWindow.ToolEnum.Types);
        }
        if (GUILayout.Button("Stats"))
        {
            TPInventorToolWindow.OpenToolWindow(TPInventorToolWindow.ToolEnum.Stats);
        }
        if (GUILayout.Button("Slots"))
        {
            TPInventorToolWindow.OpenToolWindow(TPInventorToolWindow.ToolEnum.Slots);
        }
        GUILayout.EndArea();
    }

}