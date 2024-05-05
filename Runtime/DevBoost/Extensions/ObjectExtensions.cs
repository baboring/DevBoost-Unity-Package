/* ---------------------------------------------------------------------
 * Created on Mon Jan 11 2021 3:55:29 PM
 * Author : Benjamin Park
 * Description : Object extensions
--------------------------------------------------------------------- */

namespace DevBoost.Extensions
{

    public static class ObjectExtensions
    {
        public static bool IsNullOrDestroyed(this System.Object obj)
        {

            if (object.ReferenceEquals(obj, null)) return true;

            if (obj is UnityEngine.Object) return (obj as UnityEngine.Object) == null;

            return false;
        }
    }

}