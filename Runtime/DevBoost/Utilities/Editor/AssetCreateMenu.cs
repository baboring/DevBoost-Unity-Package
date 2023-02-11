#if UNITY_EDITOR

using UnityEditor;

namespace DevBoost
{
    using DevBoost.ActionBehaviour;
    public class AssetCreateMenu
    {
        [MenuItem("Assets/Create/Data/StringSet")]
        public static void CreateAsset()
        {
            ScriptableObjectUtility.CreateAsset<StringSet>();
        }
    }
}

#endif