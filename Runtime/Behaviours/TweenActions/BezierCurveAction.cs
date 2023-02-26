/* ---------------------------------------------------------------------
 * Created on Mon Aug 19 2021 3:33:10 PM
 * Author : Benjamin Park
 * Description : Making bezier curve movement
--------------------------------------------------------------------- */
using DevBoost.Effects;
using System.Collections.Generic;
using UnityEngine;

namespace DevBoost.ActionBehaviour
{

    public class BezierCurveAction : UIBaseAction
    {
        [Header("Bezier Curve Points")]
        [SerializeField]
        private bool isUpdatePoints;    // update all position while moving
        [SerializeField]
        private bool isLocalPosition;    // use local position

        [SerializeField, NaughtyAttributes.ReorderableList]
        private Transform[] Nodes;


        private List<Vector3> controlPoints = new List<Vector3>();

        protected override void OnReset()
        {
            base.OnReset();

            UpdatePoints();
        }

        private void UpdatePoints()
        {
            controlPoints.Clear();
            foreach (var item in Nodes)
            {
                if (isLocalPosition)
                    controlPoints.Add(item.transform.localPosition);
                else
                    controlPoints.Add(item.transform.position);
            }
        }

        protected override bool DoUpdateFrame(float t)
        {
            // update points
            if (isUpdatePoints)
                UpdatePoints();

            if (controlPoints.Count > 1)
            {
                if (tweenType != EasingType.None)
                    t = Tween.Ease(t, 1, tweenType);
                var pos = BezierCurve.Point3(t, controlPoints);
                if (isLocalPosition)
                    target.transform.localPosition = pos;
                else
                    target.transform.position = pos;
            }

            return true;
        }

    }

}