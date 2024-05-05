using UnityEngine;

namespace DevBoost.Extensions
{

    public static class GameObjectExtention
    {
        public static bool IsNull<T>(this T myObject, string message = "") where T : UnityEngine.Object
        {
            if (!myObject)
            {
                if (!string.IsNullOrEmpty(message))
                    Debug.LogError("The object is null! " + message);
                return true;
            }

            return false;
        }

        public static bool IsNull(this GameObject obj)
        {
            return ReferenceEquals(null, obj);
        }


        /// <summary>
        /// Checks if a GameObject has been destroyed.
        /// </summary>
        /// <param name="gameObject">GameObject reference to check for destructedness</param>
        /// <returns>If the game object has been marked as destroyed by UnityEngine</returns>
        public static bool IsDestroyed(this GameObject gameObject)
        {
            // UnityEngine overloads the == opeator for the GameObject type
            // and returns null when the object has been destroyed, but 
            // actually the object is still there but has not been cleaned up yet
            // if we test both we can determine if the object has been destroyed.
            return gameObject == null && !ReferenceEquals(gameObject, null);
        }


        /// <summary>
        /// Checks if a DontDestroyOnLoad is activated 
        /// </summary>
        /// <param name="gameObject">GameObject reference to check for destructedness</param>
        public static bool IsDontDestroyOnLoad(this GameObject gameObject)
        {
            return gameObject != null && gameObject.scene.buildIndex == -1;
        }

        public static void RemoveComponent<Component>(this GameObject go)
        {
            Component component = go.GetComponent<Component>();

            if (component != null)
            {
                UnityEngine.Object.DestroyImmediate(component as UnityEngine.Object, true);

            }
        }
    }

}
