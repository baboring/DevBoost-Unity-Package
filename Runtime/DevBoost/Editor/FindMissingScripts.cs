using UnityEditor;
using UnityEngine;

namespace DevBoost
{
    public class FindMissingScripts : EditorWindow
    {
        static int _goCount = 0, _componentsCount = 0, _missingCount = 0, _matchCount = 0;
        static string input;
        static private string status = "Ready..";

        [MenuItem("Window/Find Missing Scripts")]
        public static void ShowWindow()
        {
            GetWindow(typeof(FindMissingScripts));
        }

        public void OnGUI()
        {
            input = GUILayout.TextField(input);

            if (GUILayout.Button("Find Missing Scripts in selected GameObjects"))
            {
                FindInSelected(input);
            }

            if (GUILayout.Button("Find Missing Scripts"))
            {
                FindAll();
            }
            if (GUILayout.Button($"Find '{input}' Scripts"))
            {
                FindAll(input);
            }
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Status:");
                EditorGUILayout.LabelField(status);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Component Scanned:");
                EditorGUILayout.LabelField("" + (_componentsCount == -1 ? "---" : _componentsCount.ToString()));
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Object Scanned:");
                EditorGUILayout.LabelField("" + (_goCount == -1 ? "---" : _goCount.ToString()));
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Possible Missing Scripts:");
                EditorGUILayout.LabelField("" + (_missingCount == -1 ? "---" : _missingCount.ToString()));
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Possible Matching Scripts:");
                EditorGUILayout.LabelField("" + (_matchCount == -1 ? "---" : _matchCount.ToString()));
            }
            EditorGUILayout.EndHorizontal();
        }

        static private void SetStatue(string _sta)
        {
            status = _sta;
        }
        private static void FindAll(string scriptName = null)
        {
            SetStatue("Working..");
            _componentsCount = 0;
            _goCount = 0;
            _missingCount = 0;
            _matchCount = 0;


            string[] assetsPaths = AssetDatabase.GetAllAssetPaths();

            foreach (string assetPath in assetsPaths)
            {
                Object[] data = LoadAllAssetsAtPath(assetPath);
                foreach (Object o in data)
                {
                    if (o != null)
                    {
                        if (o is GameObject)
                        {
                            FindInGO((GameObject) o, scriptName);
                        }
                    }
                }
            }
            SetStatue("Done..");
            LogResult();
        }

        private static void LogResult()
        {
            Debug.Log($"Searched {_goCount} GameObjects, {_componentsCount} components, found {_missingCount} missing, found {_matchCount} matching");
        }
        public static Object[] LoadAllAssetsAtPath(string assetPath)
        {
            return typeof(SceneAsset).Equals(AssetDatabase.GetMainAssetTypeAtPath(assetPath))
                ?
                // prevent error "Do not use readobjectthreaded on scene objects!"
                new[] {AssetDatabase.LoadMainAssetAtPath(assetPath)}
                : AssetDatabase.LoadAllAssetsAtPath(assetPath);
        }

        private static void FindInSelected(string scriptName = null)
        {
            SetStatue("Working..");
            GameObject[] go = Selection.gameObjects;
            _goCount = 0;
            _componentsCount = 0;
            _missingCount = 0;
            _matchCount = 0;
            foreach (GameObject g in go)
            {

                FindInGO(g, scriptName);
            }
            SetStatue("Done..");
            LogResult();
        }

        private static string GetFullName(GameObject g)
        {
            string s = g.name;
            Transform t = g.transform;
            while (t.parent != null)
            {
                var parent = t.parent;
                s = parent.name + "/" + s;
                t = parent;
            }

            return s;

        }
        private static void FindInGO(GameObject g, string scriptName)
        {
            _goCount++;
            Component[] components = g.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                _componentsCount++;
                if (components[i] == null)
                {
                    _missingCount++;
                    Debug.Log(GetFullName(g) + " has an empty script attached in position: " + i, g);
                }
                else if (CompareScriptName(components[i], scriptName))
                {
                    _matchCount++;
                    Debug.Log(GetFullName(g) + $" has found '{scriptName}' script attached in position: " + i, g);
                }
            }

            // Now recurse through each child GO (if there are any):
            foreach (Transform childT in g.transform)
            {
                //Debug.Log("Searching " + childT.name  + " " );
                FindInGO(childT.gameObject, scriptName);
            }
        }

        private static bool CompareScriptName(Component comp, string scriptName)
        {
            var compString = comp.GetType().ToString();
            if (scriptName != null && compString.EndsWith(scriptName, System.StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }
    }
}