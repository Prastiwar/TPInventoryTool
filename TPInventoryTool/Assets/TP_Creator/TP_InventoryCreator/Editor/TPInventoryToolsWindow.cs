using UnityEngine;
using UnityEditor;
using TP.Inventory;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

namespace TP.InventoryEditor
{
    [InitializeOnLoad]
    internal class TPInventoryToolsWindow : EditorWindow
    {
        public static TPInventoryToolsWindow window;
        static string currentScene;
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

        static bool isSet = false;

        List<SerializedObject> slotObjs = new List<SerializedObject>();
        List<SerializedProperty> slotProps = new List<SerializedProperty>();
        List<SerializedProperty> slotSelectable = new List<SerializedProperty>();
        int iterator = 0;
        int[] index;
        string[] enumNamesList;

        public static void OpenToolWindow(ToolEnum _tool)
        {
            window = (TPInventoryToolsWindow)GetWindow(typeof(TPInventoryToolsWindow));
            currentScene = EditorSceneManager.GetActiveScene().name;
            EditorApplication.hierarchyWindowChanged += hierarchyWindowChanged;
            window.minSize = new Vector2(425, 400);
            window.maxSize = new Vector2(425, 400);
            window.Show();
            tool = _tool;
            isSet = false;
        }

        static void hierarchyWindowChanged()
        {
            if (currentScene != EditorSceneManager.GetActiveScene().name)
            {
                if (TPInventoryDesigner.window)
                    TPInventoryDesigner.window.Close();
                if (window)
                    window.Close();
            }
        }

        void Update()
        {
            if (EditorApplication.isCompiling)
                this.Close();
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
            if(!isSet)
                SetToolWindow();
            DrawTool();
            GUILayout.EndScrollView();
        }

        void SetToolWindow()
        {
            isSet = true;
            enumNamesList = new string[TPInventoryDesigner.InventoryCreator.Data.Types.Count + 1];
            index = new int[TPInventoryDesigner.InventoryCreator.Slots.Count];

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
                    horizontalVar = "Equip Slot";
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
            if (tool == ToolEnum.Slots)
            {
                EditorGUILayout.LabelField(loaded, GUILayout.Width(155));
                EditorGUILayout.LabelField(horizontalVar, GUILayout.Width(62));
                EditorGUILayout.LabelField("Selectable", GUILayout.Width(65));
                EditorGUILayout.LabelField("Type", GUILayout.Width(65));
            }
            else
            {
                EditorGUILayout.LabelField(loaded, GUILayout.Width(180));
                EditorGUILayout.LabelField(horizontalVar, GUILayout.Width(150));
            }
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

        void DrawSlots()
        {
            if (slotObjs.Count != TPInventoryDesigner.InventoryCreator.Slots.Count)
            {
                SerializedObject slotObj = new SerializedObject(_object as UnityEngine.Object);
                SerializedProperty slotProp = slotObj.FindProperty("IsEquipSlot");
                SerializedProperty slotSelect = slotObj.FindProperty("IsSelectable");
                slotObjs.Add(slotObj);
                slotProps.Add(slotProp);
                slotSelectable.Add(slotSelect);

                EditorGUILayout.PropertyField(slotProp, GUIContent.none, GUILayout.Width(30));
                EditorGUILayout.PropertyField(slotSelect, GUIContent.none, GUILayout.Width(30));
                TypesPopup(_object as UnityEngine.Object);

                if(GUI.changed)
                    slotObj.ApplyModifiedProperties();
            }
            else
            {
                EditorGUILayout.PropertyField(slotProps[iterator], GUIContent.none, GUILayout.Width(30));
                EditorGUILayout.PropertyField(slotSelectable[iterator], GUIContent.none, GUILayout.Width(30));
                TypesPopup(_object as UnityEngine.Object);

                if (GUI.changed)
                    slotObjs[iterator].ApplyModifiedProperties();

                iterator++;
                if (iterator >= slotObjs.Count)
                    iterator = 0;
            }
            EditAsset(_object as UnityEngine.Object);
        }

        void TypesPopup(UnityEngine.Object _TPslot)
        {
            int length = TPInventoryDesigner.InventoryCreator.Data.Types.Count;
            for (int i = 0; i < length; i++)
                enumNamesList[i] = TPInventoryDesigner.InventoryCreator.Data.Types[i].name;
            enumNamesList[enumNamesList.Length - 1] = "Not Specified";

            int selectionFromInspector = enumNamesList.Length - 1;
            for (int i = 0; i < length; i++)
            {
                if ((_TPslot as TPSlot).Type)
                {
                    if ((_TPslot as TPSlot).Type.name == enumNamesList[i])
                        selectionFromInspector = i;
                }
            }

            index[iterator] = EditorGUILayout.Popup(selectionFromInspector, enumNamesList, GUILayout.Width(100));

            if(index[iterator] == enumNamesList.Length - 1)
            (_TPslot as TPSlot).Type = null;
            else
            (_TPslot as TPSlot).Type = TPInventoryDesigner.InventoryCreator.Data.Types[index[iterator]];

            if (GUI.changed)
            {
                EditorUtility.SetDirty(_TPslot);
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
            }
        }

        void DrawStats()
        {
            (_object as TPStat).Value = EditorGUILayout.FloatField((_object as TPStat).Value);
            DeleteAsset(_object as UnityEngine.Object);
            EditAsset(_object as UnityEngine.Object);
        }

        void DrawTypes()
        {
            DeleteAsset(_object as UnityEngine.Object);
            EditAsset(_object as UnityEngine.Object);
        }

        void DrawItems()
        {
            DeleteAsset(_object as UnityEngine.Object);
            EditAsset(_object as UnityEngine.Object);
        }

        void DeleteAsset(UnityEngine.Object obj)
        {
            if (GUILayout.Button("Del", GUILayout.Width(30)))
            {
                string assetPath = AssetDatabase.GetAssetPath(obj);
                AssetDatabase.MoveAssetToTrash(assetPath);

                SetToolWindow();
                TPInventoryDesigner.UpdateManager();
                SetToolWindow();
            }
        }

        void EditAsset(UnityEngine.Object obj)
        {
            if (GUILayout.Button("Edit", GUILayout.Width(40)))
            {
                AssetDatabase.OpenAsset(obj);
            }
        }

        void CreateScriptable()
        {
            string assetPath = TPInventoryDesigner.EditorData.Paths[0];
            string folderName = "";
            UnityEngine.Object newObj = null;

            switch (tool)
            {
                case ToolEnum.Items:
                    newObj = ScriptableObject.CreateInstance<TPItem>();
                    folderName = "Items/";
                    assetPath += folderName + "New Item.asset";
                    break;
                case ToolEnum.Types:
                    newObj = ScriptableObject.CreateInstance<TPType>();
                    folderName = "Types/";
                    assetPath += folderName + "New Type.asset";
                    break;
                case ToolEnum.Stats:
                    newObj = ScriptableObject.CreateInstance<TPStat>();
                    folderName = "Stats/";
                    assetPath += folderName + "New Stat.asset";
                    break;
            }

            if (!AssetDatabase.IsValidFolder(TPInventoryDesigner.EditorData.Paths[0] + folderName))
                System.IO.Directory.CreateDirectory(TPInventoryDesigner.EditorData.Paths[0] + folderName);
            
            AssetDatabase.CreateAsset(newObj, AssetDatabase.GenerateUniqueAssetPath(assetPath));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            AssetDatabase.OpenAsset(newObj);

            Debug.Log(newObj.name + " created in " + TPInventoryDesigner.EditorData.Paths[0] + folderName);
            TPInventoryDesigner.UpdateManager();
            SetToolWindow();
        }
    }
}