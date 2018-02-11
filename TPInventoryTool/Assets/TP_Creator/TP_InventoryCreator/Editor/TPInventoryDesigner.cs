using UnityEngine;
using UnityEditor;
using TP.Inventory;
using TP.Utilities;
using UnityEditor.SceneManagement;

namespace TP.InventoryEditor
{
    [InitializeOnLoad]
    internal class TPInventoryDesigner : EditorWindow
    {
        public static TPInventoryDesigner window;
        static string currentScene;
        public static TPEditorGUIData EditorData;
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
            window = (TPInventoryDesigner)GetWindow(typeof(TPInventoryDesigner));
            currentScene = EditorSceneManager.GetActiveScene().name;
            EditorApplication.hierarchyWindowChanged += hierarchyWindowChanged;
            window.minSize = new Vector2(615, 330);
            window.maxSize = new Vector2(615, 330);
            window.Show();
        }

        static void hierarchyWindowChanged()
        {
            if (currentScene != EditorSceneManager.GetActiveScene().name)
            {
                if (TPInventoryToolsWindow.window)
                    TPInventoryToolsWindow.window.Close();
                if (window)
                    window.Close();
            }
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
            string path = "Assets/TP_Creator/_CreatorResources/";
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);

            EditorData = AssetDatabase.LoadAssetAtPath(path + "InventoryEditorGUIData.asset",
                   typeof(TPEditorGUIData)) as TPEditorGUIData;
            
            if (EditorData == null)
                CreateEditorData();
            else
                CheckGUIData();
            
            skin = EditorData.GUISkin;
        }

        void CheckGUIData()
        {
            string pathData = "Assets/TP_Creator/TP_InventoryCreator/InventoryData/";
            if (!System.IO.Directory.Exists(pathData))
                System.IO.Directory.CreateDirectory(pathData);

            if (EditorData.Paths == null || EditorData.Paths.Length < 1)
            {
                EditorData.Paths = new string[1];
            }

            if (EditorData.GUISkin == null)
                EditorData.GUISkin = AssetDatabase.LoadAssetAtPath(
                      "Assets/TP_Creator/_CreatorResources/TPEditorGUISkin.guiskin",
                      typeof(GUISkin)) as GUISkin;

            if (EditorData.Paths[0] == null || EditorData.Paths[0].Length < 5)
                EditorData.Paths[0] = pathData;

            if (EditorData.Prefab == null)
                EditorData.Prefab = AssetDatabase.LoadAssetAtPath(
                    "Assets/TP_Creator/_CreatorResources/TPInventoryCanvas.prefab",
                    typeof(GameObject)) as GameObject;

            if (EditorData.GUISkin == null)
            {
                window.Close();
                Debug.LogError("There is no guiskin for TPEditor!");
            }

            EditorUtility.SetDirty(EditorData);
        }

        void CreateEditorData()
        {
            TPEditorGUIData newEditorData = ScriptableObject.CreateInstance<TPEditorGUIData>();
            AssetDatabase.CreateAsset(newEditorData, "Assets/TP_Creator/_CreatorResources/InventoryEditorGUIData.asset");
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

            var data = AssetDatabase.LoadAssetAtPath(EditorData.Paths[0] + "InventoryData" + ".asset",
                typeof(TPInventoryData));

            if (data == null)
                CreateInventoryData();
            else
                (data as TPInventoryData).Refresh();
        }

        static void CreateInventoryData()
        {
            string path = EditorData.Paths[0] + "InventoryData" + ".asset";

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
                ToggleDebugMode();
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
                if (EditorData.Prefab == null)
                {
                    Debug.LogError("There is no inventory prefab named 'TPInventoryCanvas' in Creator Resource folder!");
                    return;
                }
                Instantiate(EditorData.Prefab);
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

        void ToggleDebugMode()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Toggle Debug Mode", skin.button, GUILayout.Height(20)))
            {
                TPInventoryCreator.DebugMode = !TPInventoryCreator.DebugMode;
                if (TPInventoryToolsWindow.window)
                {
                    UpdateManager();
                    TPInventoryToolsWindow.window.Close();
                }
            }
            GUILayout.Toggle(TPInventoryCreator.DebugMode, GUIContent.none, GUILayout.Width(15));
            GUILayout.EndHorizontal();
        }

        void ResetManager()
        {
            if (GUILayout.Button("Reset Manager", skin.button, GUILayout.Height(35)))
                InventoryCreator = null;
        }

        public static void UpdateManager()
        {
            InitCreator();
            if (InventoryCreator)
            {
                InventoryCreator.RefreshSlots();
                EditorUtility.SetDirty(InventoryCreator);
                EditorUtility.SetDirty(InventoryCreator.Data);
            }
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