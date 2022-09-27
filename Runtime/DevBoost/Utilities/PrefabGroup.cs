/* ---------------------------------------------------------------------
 * Created on Mon Jan 20 2021 3:33:10 PM
 * Author : Benjamin Park
 * Description : Managing Prefabs
--------------------------------------------------------------------- */

using System.Linq;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DevBoost.Utilities
{

    [CreateAssetMenu(fileName = "PrefabGroup", menuName = "Resources/PrefabGroup", order = 1)]
    public class PrefabGroup : ScriptableObject
    {
        // list of items
        [SerializeField] private List<Chunk> m_Prefabs;           // list of prefabs

        public IEnumerable<Chunk> List { get => m_Prefabs; }

        public GameObject Find(string label)
        {
            var find = m_Prefabs.FirstOrDefault(va => va.label == label);
            if (find == default(Chunk))
                return null;
            return find.prefab;
        }


        public GameObject Spawn(string label, Transform parant, SpawnOption options = SpawnOption.None)
        {
            return Spawn<GameObject>(label, parant, options);
        }


        public T Spawn<T>(string label, Transform parant, SpawnOption options = SpawnOption.None) where T : class
        {
            var find = Find(label);
            if (find == null)
                return default(T);

            var spawned = Instantiate(find, parant);
            spawned.SetActive(true);
            spawned.name = $"{name}:{label}";// :: {spawned.name.Replace("(Clone)","")}";
            if ((options & SpawnOption.LocalPosZero) == SpawnOption.LocalPosZero) spawned.transform.localPosition = Vector3.zero;
            if ((options & SpawnOption.LocalScaleOne) == SpawnOption.LocalScaleOne) spawned.transform.localScale = Vector3.one;

            if (typeof(T) == typeof(GameObject))
                return spawned as T;
            return spawned.GetComponent<T>();
        }

    #if UNITY_EDITOR
        public void AutoBuild()
        {
            string path = EditorUtility.OpenFolderPanel("Select a folder", "Assets", "");
            if (path.Contains(Application.dataPath))
            {
                var allPrefabs = GetAssets<GameObject>(new string[] { "Assets" + path.Substring(Application.dataPath.Length) }, "t:prefab");
                m_Prefabs = allPrefabs.Select(va => new Chunk() { label = va.name, prefab = va.gameObject }).ToList();

                EditorUtility.SetDirty(this);
            }
        }

        private static List<T> GetAssets<T>(string[] _foldersToSearch, string _filter) where T : UnityEngine.Object
        {
            string[] guids = AssetDatabase.FindAssets(_filter, _foldersToSearch);
            List<T> a = new List<T>();
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                a.Add(AssetDatabase.LoadAssetAtPath<T>(path));
            }
            return a;
        }

        /// <summary>
        /// Verify Sprites
        /// </summary>
        public void VerifySprites()
        {
            Debug.Log($"Verify : {name} -----------------------------------------------");
            foreach (var item in m_Prefabs)
            {
                if (item.prefab == null)
                {
                    Debug.LogError("no prefab : " + item.label);
                }
                else
                {
                    Debug.Log($"{item.label} - Ok");
                }
            }
            Debug.Log($"Total {m_Prefabs.Count} -----------------------------------------------");
        }
    #endif

        [System.Serializable]
        public class Chunk
        {
            public string label;
            public GameObject prefab;
        }

    }
}
