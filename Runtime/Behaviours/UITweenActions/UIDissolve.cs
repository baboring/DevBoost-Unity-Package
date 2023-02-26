/* ---------------------------------------------------------------------
 * Created on Mon Aug 19 2021 3:33:10 PM
 * Author : Benjamin Park
 * Description : Making Dissolve animation
--------------------------------------------------------------------- */

using UnityEngine;
using NaughtyAttributes;
using DevBoost.Effects;

namespace DevBoost.ActionBehaviour
{

    public class UIDissolve : UIBaseAction
    {

        [SerializeField]
        [Dropdown("GetMode"), Label("Mode")]
        private bool reverse;

        [SerializeField]
        private CanvasGroup canvasGroup;

        private float begin;

        private DropdownList<bool> GetMode() => new DropdownList<bool>() {
                { "Fade out",   true },
                { "Fade in",   false },
            };
        

        private CanvasGroup canvasGroup_ {
            get {
                if (canvasGroup == null)
                    canvasGroup = target?.GetComponent<CanvasGroup>();
                return canvasGroup;
            }
        }

        protected override void OnReset()
        {
            base.OnReset();

            begin = canvasGroup_.alpha;
            if (tweenType == EasingType.None)
                tweenType = EasingType.Linear;
        }

        protected override bool DoUpdateFrame(float t)
        {
            if (canvasGroup_ != null)
            {
                t = Tween.Ease(t, 1, tweenType);
                canvasGroup_.alpha = Mathf.Lerp(begin, reverse ? 0 : 1, t);
            }

            return true;
        }

    }

}