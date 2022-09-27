/* ---------------------------------------------------------------------
 * Created on Mon Jan 20 2021 3:33:10 PM
 * Author : Benjamin Park
 * Description : Managing Prefabs
--------------------------------------------------------------------- */

using UnityEngine;

namespace DevBoost.Utilities
{
    public enum SpawnOption
    {
        None = 0,
        LocalPosZero = 1 << 0,
        LocalScaleOne = 1 << 1
    }
    /// <summary>
    /// managing Prefab 
    /// </summary>
    public class PrefabManager : SingletonMono<PrefabManager>
    {
        // list of items
        [SerializeField] private PrefabGroup[] m_Groups;        // list of prefabs for mobile build


        public PrefabGroup FindGroup(string label)
        {
            foreach (var group in m_Groups)
            {
                if (group.Find(label) != null)
                    return group;
            }
            return null;
        }

        public GameObject FindGameObject(string label)
        {
            GameObject found = null;
            foreach (var group in m_Groups)
            {
                found = group.Find(label);
                if (found != null)
                    return found;
            }
            return null;
        }
        public GameObject Spawn(string label, Transform parant = null, SpawnOption options = SpawnOption.None)
        {
            return FindGroup(label)?.Spawn(label, parant, options);
        }

        public T Spawn<T>(string label, Transform parant = null, SpawnOption options = SpawnOption.None) where T : class
        {
            var found = FindGroup(label);
            if (found == null)
                return default(T);
            return found.Spawn<T>(label, parant, options);
        }

    }

}