/* ---------------------------------------------------------------------
 * Created on Mon Aug 19 2021 3:33:10 PM
 * Author : Benjamin Park
 * Description : Making scale animation
--------------------------------------------------------------------- */
using DevBoost.Effects;
using System.Collections.Generic;
using UnityEngine;

namespace DevBoost.ActionBehaviour
{

    public class UIScaleAction : UIBaseAction
    {

        [SerializeField,NaughtyAttributes.ReorderableList]
        private Vector2[] Nodes;

        private List<Vector2> controlPoints = new List<Vector2>();


        protected override void OnReset()
        {
            base.OnReset();

            controlPoints.Clear();

            foreach (var item in Nodes)
            {
                controlPoints.Add(item);
            }
        }

        protected override bool DoUpdateFrame(float t)
        {
            if (controlPoints.Count > 1)
            {
                if (tweenType != EasingType.None)
                    t = Tween.Ease(t, 1, tweenType);
                target.transform.localScale = BezierCurve.Point2(t, controlPoints);
            }

            return true;
        }

    }

}