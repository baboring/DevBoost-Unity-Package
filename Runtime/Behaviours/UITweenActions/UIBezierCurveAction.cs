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

    public class UIBezierCurveAction : UIBaseAction
    {
        [Header("Bezier Curve Points")]
        [SerializeField]
        private bool isUpdatePoints;    // update all position while moving

        [SerializeField, NaughtyAttributes.ReorderableList]
        private RectTransform[] Nodes;


        private List<Vector2> controlPoints = new List<Vector2>();

        protected override void OnReset()
        {
            base.OnReset();

            UpdatePoints();
        }

        private void UpdatePoints()
        {
            controlPoints.Clear();
            foreach (var item in Nodes)
                controlPoints.Add(item.transform.position);
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
                var pos = BezierCurve.Point2(t, controlPoints);
                var myPos = target.transform.position;
                myPos.x = pos.x;
                myPos.y = pos.y;
                target.transform.position = myPos;
            }

            return true;
        }

    }

}