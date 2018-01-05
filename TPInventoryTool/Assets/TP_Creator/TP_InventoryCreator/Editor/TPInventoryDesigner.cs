﻿using UnityEngine;
using UnityEditor;
using TP_Inventory;

public class TPInventoryDesigner : EditorWindow
{
    public static TPInventoryGUIData editorData;
    public static TPInventoryCreator inventoryCreator;
    GUISkin skin;

    Texture2D headerTexture;
    Texture2D managerTexture;
    Texture2D toolTexture;

    Rect headerSection;
    Rect managerSection;
    Rect toolSection;

    bool toggleParent;
    bool togglePersistance;

    [MenuItem("TP_Creator/TP_InventoryCreator")]
    public static void OpenWindow()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.Log("You can't change Inventory Designer runtime!");
            return;
        }
        TPInventoryDesigner window = (TPInventoryDesigner)GetWindow(typeof(TPInventoryDesigner));
        window.minSize = new Vector2(600, 300);
        window.Show();
    }

    void OnEnable()
    {
        editorData = AssetDatabase.LoadAssetAtPath(
            "Assets/TP_Creator/TP_InventoryCreator/Editor Resources/EditorGUIData.asset",
            typeof(TPInventoryGUIData)) as TPInventoryGUIData;
        if (editorData == null)
        {
            Debug.Log("Editor data didn't found! Check path: 'Assets / TP_Creator / TP_InventoryCreator / Editor Resources / EditorGUIData.asset'");
            return;
        }
        skin = editorData.GUISkin;

        Color colorHeader = new Color(0.19f, 0.19f, 0.19f);
        Color color = new Color(0.15f, 0.15f, 0.15f);

        headerTexture = new Texture2D(1, 1);
        headerTexture.SetPixel(0, 0, colorHeader);
        headerTexture.Apply();

        managerTexture = new Texture2D(1, 1);
        managerTexture.SetPixel(0, 0, color);
        managerTexture.Apply();

        toolTexture = new Texture2D(1, 1);
        toolTexture.SetPixel(0, 0, color);
        toolTexture.Apply();

        togglePersistance = EditorPrefs.GetBool("Persistance", false);

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

        var data = AssetDatabase.LoadAssetAtPath("Assets/" + editorData.InventoryDataPath, typeof(TPInventoryData));
        if (data == null)
        {
            TPInventoryData newData = ScriptableObject.CreateInstance<TPInventoryData>();

            if (AssetDatabase.IsValidFolder("Assets/" + editorData.InventoryDataPath + "InventoryData.asset"))
                Debug.Log("This path doesn't exist, create one!");

            newData.isSaving = EditorPrefs.GetBool("Persistance", false);
            AssetDatabase.CreateAsset(newData, "Assets/" + editorData.InventoryDataPath + "InventoryData.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            if(inventoryCreator)
                inventoryCreator.InventoryPersistance.inventoryData = newData;
        }
    }

    void OnGUI()
    {
        if (EditorApplication.isPlaying)
        {
            if(TPInventoryToolsWindow.window)
                TPInventoryToolsWindow.window.Close();
            this.Close();
        }
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
        GUILayout.Label("TP Inventory Creator - Manage your Inventory!", skin.GetStyle("HeaderLabel"));
        GUILayout.EndArea();
    }

    void DrawManager()
    {
        GUILayout.BeginArea(managerSection);
        GUILayout.Label("Inventory Manager - Core", skin.box);

        if (inventoryCreator == null)
        {
            InitializeManager();
        }
        else
        {
            SpawnEmpty();
            ChangeParent();
            TogglePersistance();

            if (GUILayout.Button("Refresh and update", skin.button, GUILayout.Height(60)))
            {
                UpdateManager();
            }
        }

        GUILayout.EndArea();
    }
    void InitializeManager()
    {
        if (GUILayout.Button("Initialize Manager", skin.button, GUILayout.Height(50)))
        {
            GameObject go = (new GameObject("TP_InventoryManager", typeof(TPInventoryCreator)));
            inventoryCreator = go.GetComponent<TPInventoryCreator>();
            UpdateManager();
            Debug.Log("Inventory Manager created!");
        }
    }
    void SpawnEmpty()
    {
        if (GUILayout.Button("Spawn empty inventory hierarchy", skin.button, GUILayout.Height(40)))
        {
            Instantiate(editorData.InventoryPrefab);
            Debug.Log("Inventory Created");
        }
    }

    void ChangeParent()
    {
        if (GUILayout.Button("Change Parent", skin.button, GUILayout.Height(50)))
            toggleParent = !toggleParent;

        if (toggleParent)
        {
            EditorGUILayout.LabelField("Put there parent of all slot's parent", skin.GetStyle("TipLabel"));
            inventoryCreator.slotParentsTransform =
                EditorGUILayout.ObjectField(inventoryCreator.slotParentsTransform, typeof(Transform), true) as Transform;
            inventoryCreator.RefreshSlots();
            if (GUI.changed)
                UpdateManager();
        }
    }

    void TogglePersistance()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Toggle Save/Load System", skin.button))
        {
            togglePersistance = !togglePersistance;
            inventoryCreator.InventoryPersistance.inventoryData.isSaving = togglePersistance;
            EditorPrefs.SetBool("Persistance", togglePersistance);
            UpdateManager();
            Debug.Log(togglePersistance ? "Save/Load enabled" : "Save/Load disabled");
        }
        EditorGUILayout.EndHorizontal();
    }

    public static void UpdateManager()
    {
        InitCreator();
        inventoryCreator.RefreshSlots();
        EditorUtility.SetDirty(inventoryCreator);
        //EditorUtility.SetDirty(inventoryCreator.InventoryPersistance);
        //EditorUtility.SetDirty(inventoryCreator.InventoryPersistance.inventoryData);

        if (TPInventoryToolsWindow.window)
            TPInventoryToolsWindow.SetToolWindow();
    }

    void DrawTools()
    {
        GUILayout.BeginArea(toolSection);
        GUILayout.Label("Inventory Manager - Tools", skin.box);
        if (GUILayout.Button("Items", skin.button, GUILayout.Height(50)))
        {
            TPInventoryToolsWindow.OpenToolWindow(TPInventoryToolsWindow.ToolEnum.Items);
        }
        if (GUILayout.Button("Types", skin.button, GUILayout.Height(50)))
        {
            TPInventoryToolsWindow.OpenToolWindow(TPInventoryToolsWindow.ToolEnum.Types);
        }
        if (GUILayout.Button("Stats", skin.button, GUILayout.Height(50)))
        {
            TPInventoryToolsWindow.OpenToolWindow(TPInventoryToolsWindow.ToolEnum.Stats);
        }
        if (GUILayout.Button("Slots", skin.button, GUILayout.Height(50)))
        {
            TPInventoryToolsWindow.OpenToolWindow(TPInventoryToolsWindow.ToolEnum.Slots);
        }
        GUILayout.EndArea();
    }

}