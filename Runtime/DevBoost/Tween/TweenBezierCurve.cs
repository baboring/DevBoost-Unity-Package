using System.Collections.Generic;
using UnityEngine;

namespace DevBoost.Effects
{

    public class TweenBezierCurve : TweenBase
    {
        // local values
        private List<Vector2> controlPoints;

        public Vector2 Output;

        public TweenBezierCurve(List<Vector2> points)
        {
            if (null != points)
            {
                controlPoints = new List<Vector2>(points);
                Output = BezierCurve.Point2(0, controlPoints);
            }
        }

        protected override bool OnUpdateFrame(float t)
        {
            if (controlPoints == null)
                return false;

            if (controlPoints.Count > 1)
            {
                if (tweenType != EasingType.None)
                    t = Tween.Ease(t, 1, tweenType);
                Output = BezierCurve.Point2(t, controlPoints);
            }

            return true;
        }

    }

}