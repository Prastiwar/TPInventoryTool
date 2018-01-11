using UnityEngine;
using UnityEditor;
using TP_Inventory;

namespace TP_InventoryEditor
{
    public class TPInventoryDesigner : EditorWindow
    {
        public static TPInventoryGUIData EditorData;
        public static TPInventoryCreator InventoryCreator;
        GUISkin skin;

        Texture2D headerTexture;
        Texture2D managerTexture;
        Texture2D toolTexture;

        Rect headerSection;
        Rect managerSection;
        Rect toolSection;

        bool toggleParent;
        bool existManager;

        SerializedObject creator;
        SerializedProperty slotsParents;

        [MenuItem("TP_Creator/TP_InventoryCreator")]
        public static void OpenWindow()
        {
            if (EditorApplication.isPlaying)
            {
                Debug.Log("You can't change Inventory Designer runtime!");
                return;
            }
            TPInventoryDesigner window = (TPInventoryDesigner)GetWindow(typeof(TPInventoryDesigner));
            window.minSize = new Vector2(615, 330);
            window.maxSize = new Vector2(615, 330);
            window.Show();
        }

        void OnEnable()
        {
            InitEditorData();

            InitTextures();

            InitCreator();

            if (InventoryCreator != null)
            {
                creator = new SerializedObject(InventoryCreator);
                slotsParents = creator.FindProperty("SlotParentsTransform");
            }
        }

        void InitEditorData()
        {
            EditorData = AssetDatabase.LoadAssetAtPath(
                   "Assets/TP_Creator/TP_InventoryCreator/EditorResources/InventoryEditorGUIData.asset",
                   typeof(TPInventoryGUIData)) as TPInventoryGUIData;
            
            if (EditorData == null)
                CreateEditorData();
            else
                CheckGUIData();
            
            skin = EditorData.GUISkin;
        }

        void CheckGUIData()
        {
            if (EditorData.GUISkin == null)
                EditorData.GUISkin = AssetDatabase.LoadAssetAtPath(
                      "Assets/TP_Creator/TP_InventoryCreator/EditorResources/TPInventoryGUISkin.guiskin",
                      typeof(GUISkin)) as GUISkin;

            if (EditorData.InventoryDataPath == null || EditorData.InventoryAssetsPath.Length < 5)
                EditorData.InventoryDataPath = "TP_Creator/TP_InventoryCreator/InventoryData/";

            if (EditorData.InventoryAssetsPath == null || EditorData.InventoryAssetsPath.Length < 5)
                EditorData.InventoryAssetsPath = "TP_Creator/TP_InventoryCreator/InventoryData/";

            if (EditorData.InventoryPrefab == null)
                EditorData.InventoryPrefab = AssetDatabase.LoadAssetAtPath(
                    "Assets/TP_Creator/TP_InventoryCreator/EditorResources/TPInventoryCanvas.prefab",
                    typeof(GameObject)) as GameObject;

            EditorUtility.SetDirty(EditorData);
        }

        void CreateEditorData()
        {
            TPInventoryGUIData newEditorData = ScriptableObject.CreateInstance<TPInventoryGUIData>();
            AssetDatabase.CreateAsset(newEditorData, "Assets/TP_Creator/TP_InventoryCreator/EditorResources/InventoryEditorGUIData.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorData = newEditorData;
            CheckGUIData();
        }

        void InitTextures()
        {
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
        }

        static void InitCreator()
        {
            if (InventoryCreator == null)
            {
                InventoryCreator = FindObjectOfType<TPInventoryCreator>();

                if (InventoryCreator != null)
                    UpdateManager();
            }

            var data = AssetDatabase.LoadAssetAtPath("Assets/" + EditorData.InventoryDataPath + "InventoryData" + ".asset",
                typeof(TPInventoryData));

            if (data == null)
                CreateInventoryData();
            else
                (data as TPInventoryData).Refresh();
        }

        static void CreateInventoryData()
        {
            string path = "Assets/" + EditorData.InventoryDataPath + "InventoryData" + ".asset";
            if (AssetDatabase.IsValidFolder(path))
            {
                Debug.Log("This path doesn't exist, create one!");
                return;
            }

            TPInventoryData newData = ScriptableObject.CreateInstance<TPInventoryData>();
            AssetDatabase.CreateAsset(newData, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            if (InventoryCreator)
                InventoryCreator.Data = newData;

            Debug.Log("Created InventoryData file");
        }

        void OnGUI()
        {
            if(creator != null)
                creator.Update();

            if (EditorApplication.isPlaying)
            {
                if (TPInventoryToolsWindow.window)
                    TPInventoryToolsWindow.window.Close();
                this.Close();
            }
            DrawLayouts();
            DrawHeader();
            DrawManager();
            DrawTools();

            if (creator != null)
                creator.ApplyModifiedProperties();
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

            if (InventoryCreator == null)
            {
                InitializeManager();
            }
            else
            {
                ChangeParent();
                SpawnEmpty();
                ResetManager();

                if (GUILayout.Button("Refresh and update", skin.button, GUILayout.Height(60)))
                {
                    UpdateManager();
                }
            }

            GUILayout.EndArea();
        }

        void InitializeManager()
        {
            if (GUILayout.Button("Initialize New Manager", skin.button, GUILayout.Height(50)))
            {
                GameObject go = (new GameObject("TP_InventoryManager", typeof(TPInventoryCreator)));
                InventoryCreator = go.GetComponent<TPInventoryCreator>();
                UpdateManager();
                Debug.Log("Inventory Manager created!");
                OnEnable();
            }

            if (GUILayout.Button("Initialize Exist Manager", skin.button, GUILayout.Height(50)))
                existManager = !existManager;

            if (existManager)
                InventoryCreator = EditorGUILayout.ObjectField(InventoryCreator, typeof(TPInventoryCreator), true,
                    GUILayout.Height(30)) as TPInventoryCreator;
        }

        void SpawnEmpty()
        {
            if (GUILayout.Button("Spawn empty inventory hierarchy", skin.button, GUILayout.Height(40)))
            {
                if (EditorData.InventoryPrefab == null)
                {
                    Debug.LogError("There is no inventory prefab in InventoryEditorGUIData file!");
                    return;
                }
                Instantiate(EditorData.InventoryPrefab);
                Debug.Log("Inventory Hierarchy Created");
            }
        }

        void ChangeParent()
        {
            if (GUILayout.Button("Change Parent", skin.button, GUILayout.Height(50)))
                toggleParent = !toggleParent;

            if (toggleParent)
            {
                EditorGUILayout.LabelField("Put there parent of all slot's parent", skin.GetStyle("TipLabel"));
                EditorGUILayout.PropertyField(slotsParents, GUIContent.none, GUILayout.Height(15));

                if (GUI.changed)
                {
                    InventoryCreator.RefreshSlots();
                    UpdateManager();
                }
                EditorGUILayout.Space();
            }
        }

        void ResetManager()
        {
            if (GUILayout.Button("Reset Manager", skin.button, GUILayout.Height(35)))
                InventoryCreator = null;
        }

        public static void UpdateManager()
        {
            InitCreator();
            InventoryCreator.RefreshSlots();
            EditorUtility.SetDirty(InventoryCreator);

            if (TPInventoryToolsWindow.window)
                TPInventoryToolsWindow.SetToolWindow();
        }

        void DrawTools()
        {
            GUILayout.BeginArea(toolSection);
            GUILayout.Label("Inventory Manager - Tools", skin.box);

            if (InventoryCreator != null)
            {
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
            }
            GUILayout.EndArea();
        }
    }
}