using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TP_Inventory;

public class TPInventoryDesigner : EditorWindow
{
    GUISkin skin;

    Texture2D headerTexture;
    Texture2D managerTexture;
    Texture2D toolTexture;

    Rect headerSection;
    Rect managerSection;
    Rect toolSection;

    TPInventoryCreator inventoryCreator;

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
        GUILayout.EndArea();
    }

    void DrawManager()
    {
        GUILayout.BeginArea(managerSection);
        GUILayout.Label("Inventory Manager - Core");

        if (inventoryCreator == null && !(inventoryCreator = FindObjectOfType<TPInventoryCreator>()))
        {
            if (GUILayout.Button("Initialize Inventory Manager"))
            {
                GameObject go = (new GameObject("TP_InventoryManager", typeof(TPInventoryCreator), typeof(TPInventorySaveLoad)));
                inventoryCreator = go.GetComponent<TPInventoryCreator>();
                Debug.Log("Inventory Manager created and updated");
            }
        }
        else
        {
            EditorGUILayout.LabelField("Put there parent of all slot's parent");
            inventoryCreator.slotTransform = (EditorGUILayout.ObjectField(inventoryCreator.slotTransform.gameObject, typeof(GameObject), true) as GameObject).transform;
            if (GUILayout.Button("Check Loaded Slots"))
            {
                EditorGUILayout.TextArea("Slots Loaded: ");
                foreach (var item in inventoryCreator.Slots)
                {
                    EditorGUILayout.ObjectField(item, typeof(TPSlot), true);
                }
            }

            if (GUILayout.Button("Update Manager", skin.GetStyle("Header")))
            {
                Debug.Log("Inventory Manager updated");
            }
        }

        GUILayout.EndArea();
    }

    void DrawTools()
    {

    }

}