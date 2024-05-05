/* ---------------------------------------------------------------------
 * Created on Mon Jan 11 2021 3:55:29 PM
 * Author : Benjamin Park
 * Description : Capsule extensions
--------------------------------------------------------------------- */

using UnityEngine;

namespace DevBoost.Extensions
{

    public static class ColliderExtensions
    {
        public static void GetCapsuleDataLocal(this CapsuleCollider col, out Vector3 localPoint0, out Vector3 localPoint1, out float radius)
        {
            var direction = new Vector3 { [col.direction] = 1 };
            radius = col.height / 2 - col.radius;
            localPoint0 = col.center - direction * radius;
            localPoint1 = col.center + direction * radius;
        }

        public static void GetCapsuleDataWorld(this CapsuleCollider col, out Vector3 point0, out Vector3 point1, out float radius)
        {
            var direction = new Vector3 { [col.direction] = 1 };
            radius = col.height / 2 - col.radius;
            var localPoint0 = col.center - direction * radius;
            var localPoint1 = col.center + direction * radius;

            point0 = col.transform.TransformPoint(localPoint0);
            point1 = col.transform.TransformPoint(localPoint1);
        }

        public static Collider[] OverlapCapsule(CapsuleCollider box, LayerMask layer)
        {
            box.GetCapsuleDataWorld(out Vector3 point0, out Vector3 point1, out float radius);
            return Physics.OverlapCapsule(point0, point1, radius, layer);
        }

        public static Collider[] OverlapBox(BoxCollider box, Quaternion orientation, LayerMask layer)
        {
            return Physics.OverlapBox(box.bounds.center, box.bounds.extents, orientation, layer);
        }


    }

}