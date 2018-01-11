using UnityEngine;
using UnityEditor;
using TP_Inventory;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

namespace TP_InventoryEditor
{
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

        Rect mainRect;
        Vector2 scrollPos = Vector2.zero;
        Texture2D mainTexture;

        public static void OpenToolWindow(ToolEnum _tool)
        {
            window = (TPInventoryToolsWindow)GetWindow(typeof(TPInventoryToolsWindow));
            window.minSize = new Vector2(400, 400);
            window.maxSize = new Vector2(400, 400);
            window.Show();
            tool = _tool;
            SetToolWindow();
        }

        void OnEnable()
        {
            Color color = new Color(0.19f, 0.19f, 0.19f);
            mainTexture = new Texture2D(1, 1);
            mainTexture.SetPixel(0, 0, color);
            mainTexture.Apply();
        }

        void OnGUI()
        {
            mainRect = new Rect(0, 0, Screen.width, Screen.height);
            GUI.DrawTexture(mainRect, mainTexture);
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
                    array = TPInventoryDesigner.InventoryCreator.Data.Items.ToArray();
                    action = DrawItems;
                    type = typeof(TPItem);
                    break;
                case ToolEnum.Types:
                    loaded = "Types Loaded";
                    noLoaded = "No Types Loaded! Try to Refresh / Update Manager or create one!";
                    horizontalVar = "";
                    array = TPInventoryDesigner.InventoryCreator.Data.Types.ToArray();
                    action = DrawTypes;
                    type = typeof(TPType);
                    break;
                case ToolEnum.Stats:
                    loaded = "Stats Loaded";
                    noLoaded = "No Stats Loaded! Try to Refresh / Update Manager or create one!";
                    horizontalVar = "Value";
                    array = TPInventoryDesigner.InventoryCreator.Data.Stats.ToArray();
                    action = DrawStats;
                    type = typeof(TPStat);
                    break;
                case ToolEnum.Slots:
                    loaded = "Slots Loaded";
                    noLoaded = "No Slots Loaded! Try to Refresh / Update Manager or change slot parent!";
                    horizontalVar = "Is Equip Slot";
                    array = TPInventoryDesigner.InventoryCreator.Slots.ToArray();
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
                if (GUILayout.Button("Create new", TPInventoryDesigner.EditorData.GUISkin.button))
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
            EditorGUILayout.LabelField(loaded, GUILayout.Width(180));
            EditorGUILayout.LabelField(horizontalVar, GUILayout.Width(150));
            GUILayout.EndHorizontal();

            foreach (UnityEngine.Object element in array)
            {
                _object = element;
                GUILayout.BeginHorizontal();
                EditorGUILayout.ObjectField(element, type, false, GUILayout.Width(200));
                action();
                GUILayout.EndHorizontal();
                EditorUtility.SetDirty((_object as UnityEngine.Object) == null ? this : (_object as UnityEngine.Object));
            }
        }

        static List<SerializedObject> slotObjs = new List<SerializedObject>();
        static List<SerializedProperty> slotProps = new List<SerializedProperty>();
        static int iterator = 0;

        static void DrawSlots()
        {
            if (slotObjs.Count != TPInventoryDesigner.InventoryCreator.Slots.Count)
            {
                SerializedObject slotObj = new SerializedObject(_object as UnityEngine.Object);
                SerializedProperty slotProp = slotObj.FindProperty("IsEquipSlot");
                slotObjs.Add(slotObj);
                slotProps.Add(slotProp);

                EditorGUILayout.PropertyField(slotProp, GUIContent.none, GUILayout.Width(30));
                slotObj.ApplyModifiedProperties();
            }
            else
            {
                EditorGUILayout.PropertyField(slotProps[iterator], GUIContent.none, GUILayout.Width(30));
                slotObjs[iterator].ApplyModifiedProperties();
                iterator++;
                if (iterator >= slotObjs.Count)
                    iterator = 0;
            }
            EditAsset(_object as UnityEngine.Object);
        }

        static void DrawStats()
        {
            (_object as TPStat).Value = EditorGUILayout.FloatField((_object as TPStat).Value);
            DeleteAsset(_object as UnityEngine.Object);
            EditAsset(_object as UnityEngine.Object);
        }

        static void DrawTypes()
        {
            DeleteAsset(_object as UnityEngine.Object);
            EditAsset(_object as UnityEngine.Object);
        }

        static void DrawItems()
        {
            DeleteAsset(_object as UnityEngine.Object);
            EditAsset(_object as UnityEngine.Object);
        }

        static void DeleteAsset(UnityEngine.Object obj)
        {
            if (GUILayout.Button("Del", GUILayout.Width(30)))
            {
                string assetPath = AssetDatabase.GetAssetPath(obj);
                AssetDatabase.MoveAssetToTrash(assetPath);

                SetToolWindow();
                TPInventoryDesigner.UpdateManager();
            }
        }

        static void EditAsset(UnityEngine.Object obj)
        {
            if (GUILayout.Button("Edit", GUILayout.Width(35)))
            {
                AssetDatabase.OpenAsset(obj);
            }
        }

        void CreateScriptable()
        {
            string assetPath = "Assets/" + TPInventoryDesigner.EditorData.InventoryAssetsPath;
            string folderName = "";
            UnityEngine.Object newObj = null;

            switch (tool)
            {
                case ToolEnum.Items:
                    newObj = ScriptableObject.CreateInstance<TPItem>();
                    folderName = "Items";
                    assetPath += folderName + "/New Item.asset";
                    break;
                case ToolEnum.Types:
                    newObj = ScriptableObject.CreateInstance<TPType>();
                    folderName = "Types";
                    assetPath += folderName + "/New Type.asset";
                    break;
                case ToolEnum.Stats:
                    newObj = ScriptableObject.CreateInstance<TPStat>();
                    folderName = "Stats";
                    assetPath += folderName + "/New Stat.asset";
                    break;
                case ToolEnum.Slots:
                    return;
                default:
                    return;
            }

            if (!AssetDatabase.IsValidFolder("Assets/" + TPInventoryDesigner.EditorData.InventoryAssetsPath + folderName))
            {
                Debug.Log(assetPath);
                Debug.Log("This assets path doesn't exist, create one or check EditorGUIData to change path.");
                return;
            }

            AssetDatabase.CreateAsset(newObj, AssetDatabase.GenerateUniqueAssetPath(assetPath));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log(newObj.name + " created in Assets/" + TPInventoryDesigner.EditorData.InventoryAssetsPath + folderName);
            TPInventoryDesigner.UpdateManager();
            SetToolWindow();
        }
    }
}