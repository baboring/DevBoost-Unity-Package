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
        [SerializeField] private string label = "prefab";
        // list of items
        [SerializeField] private List<Chunk> lstPrefabs;           // list of prefabs

        [SerializeField] private List<PrefabGroup> fallbackGroups;

        public IEnumerable<Chunk> List { get => lstPrefabs; }
        public string Label => label;

        public List<PrefabGroup> GetAllGroups()
        {
            var lst = new List<PrefabGroup>() { this };
            FetchFallbackList(lst);
            return lst;
        }

        private void FetchFallbackList(List<PrefabGroup> builder)
        {
            foreach (var group in fallbackGroups)
            {
                if (!builder.Contains(group))
                {
                    builder.Add(group);
                    group.FetchFallbackList(builder);
                }
            }
        }

        /// <summary>
        /// find the prefabgroup asset recursively
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        private PrefabGroup FindGroupList(string groupName, List<PrefabGroup> builder)
        {
            if (builder.Contains(this))
                return null;

            builder.Add(this);

            if (label == groupName)
                return this;

            foreach (var group in fallbackGroups)
            {
                var found = group.FindGroupList(groupName, builder);
                if (found != null)
                    return found;
            }
            return null;
        }

        /// <summary>
        /// Find the prefab group by the label
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public bool TryGetGroup(string groupName, out PrefabGroup prefabGroup)
        {
            if (label == groupName)
            {
                prefabGroup = this;
                return this;
            }

            prefabGroup = FindGroupList(groupName, new List<PrefabGroup>());
            return prefabGroup != null;
        }

        /// <summary>
        /// Find the Prefab GameObject by the label
        /// </summary>
        /// <param name="label"></param>
        /// <param name="useFallback"></param>
        /// <returns></returns>
        public GameObject Find(string label, bool useFallback = true)
        {
            return FindChunk(label, useFallback)?.prefab;
        }

        /// <summary>
        /// Find the Prefab Chunk by the label
        /// </summary>
        /// <param name="label"></param>
        /// <param name="useFallback"></param>
        /// <returns></returns>
        public Chunk FindChunk(string label, bool useFallback = true)
        {
            var find = lstPrefabs.FirstOrDefault(va => va.label == label);
            if (find == default(Chunk))
            {
                if (useFallback)
                {
                    foreach (var group in GetAllGroups())
                    {
                        if (group == this)
                            continue;
                        var obj = group.FindChunk(label, false);
                        if (obj != null)
                            return obj;
                    }
                }
                return null;
            }
            return find;
        }

        /// <summary>
        /// Find a chunk of the prefab in the list
        /// </summary>
        /// <param name="label"></param>
        /// <param name="group"></param>
        /// <param name="found"></param>
        /// <param name="useFallback"></param>
        /// <returns></returns>
        public bool TryFindChunk(string label, out PrefabGroup group, out GameObject found, bool useFallback = true)
        {
            group = this;
            found = null;
            if (TryFindChunk(label, out group, out Chunk chunk, useFallback))
                found = chunk.prefab;
            return found != null;
        }

        public bool TryFindChunk(string label, out PrefabGroup group, out Chunk found, bool useFallback = true)
        {
            group = this;
            found = null;
            var find = lstPrefabs.FirstOrDefault(va => va.label == label);
            if (find == default(Chunk))
            {
                if (useFallback)
                {
                    foreach (var _group in GetAllGroups())
                    {
                        if (_group == this)
                            continue;
                        var obj = _group.FindChunk(label, false);
                        if (obj != null)
                        {
                            found = obj;
                            group = _group;
                            return true;
                        }
                    }
                }
                return false;
            }
            found = find;
            return true;
        }

        public List<GameObject> SpawnAll(Transform parant = null, SpawnOption options = SpawnOption.None, bool useFallback = true)
        {
            List<GameObject> spawned = new List<GameObject>();
            if (!useFallback)
            {
                foreach (var prefab in List)
                    spawned.Add(prefab.InstantiatePrefab(parant, options, label));
            }
            else
            {
                foreach (var group in GetAllGroups())
                    spawned.AddRange(group.SpawnAll(parant, options, false));
            }
            return spawned;
        }

        /// <summary>
        /// Spawn the GameObject by the name
        /// </summary>
        /// <param name="label"></param>
        /// <param name="parant"></param>
        /// <param name="options"></param>
        /// <param name="useFallback"></param>
        /// <returns></returns>
        public GameObject Spawn(string label, Transform parant = null, SpawnOption options = SpawnOption.None, bool useFallback = true)
        {
            return Spawn<GameObject>(label, parant, options, useFallback);
        }


        public T Spawn<T>(string label, Transform parant = null, SpawnOption options = SpawnOption.None, bool useFallback = true) where T : class
        {
            if (!TryFindChunk(label, out PrefabGroup group, out Chunk find, useFallback))
                return default(T);

            var spawned = find.InstantiatePrefab(parant, options, group.label);

            if (typeof(T) == typeof(GameObject))
                return spawned as T;
            return spawned.GetComponent<T>();
        }

#if UNITY_EDITOR
        //[EasyButtons.Button("AutoBuild")]
        public void AutoBuild()
        {
            string path = EditorUtility.OpenFolderPanel("Select a folder", "Assets", "");
            if (path.Contains(Application.dataPath))
            {
                var allPrefabs = GetAssets<GameObject>(new string[] { "Assets" + path.Substring(Application.dataPath.Length) }, "t:prefab");
                lstPrefabs = allPrefabs.Select(va => new Chunk() { label = va.name, prefab = va.gameObject }).ToList();

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
        //[EasyButtons.Button("Verify links")]
        public void VerifyLinks()
        {
            Debug.Log($"Verify : {name} -----------------------------------------------");
            foreach (var item in lstPrefabs)
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
            Debug.Log($"Total {lstPrefabs.Count} -----------------------------------------------");
        }
#endif

        [System.Serializable]
        public class Chunk
        {
            public string label;
            public GameObject prefab;

            public GameObject InstantiatePrefab(Transform parant = null, SpawnOption options = SpawnOption.None, string groupLabel = "")
                => InstantiatePrefab<GameObject>(parant, options, groupLabel);

            public T InstantiatePrefab<T>(Transform parant = null, SpawnOption options = SpawnOption.None, string groupLabel = "") where T : class
            {
                var spawned = Object.Instantiate(prefab, parant);
                spawned.SetActive(true);
                spawned.name = $"{groupLabel}:{label}";// :: {spawned.name.Replace("(Clone)","")}";
                if ((options & SpawnOption.LocalPosZero) == SpawnOption.LocalPosZero) spawned.transform.localPosition = Vector3.zero;
                if ((options & SpawnOption.LocalScaleOne) == SpawnOption.LocalScaleOne) spawned.transform.localScale = Vector3.one;

                if (typeof(T) == typeof(GameObject))
                    return spawned as T;
                return spawned.GetComponent<T>();
            }
        }
    }
}
