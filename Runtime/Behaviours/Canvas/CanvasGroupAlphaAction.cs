/* ---------------------------------------------------------------------
 * Created on Mon Aug 19 2021 3:33:10 PM
 * Author : Benjamin Park
 * Description : Canvas Group Alpha
--------------------------------------------------------------------- */
using UnityEngine;

namespace DevBoost.ActionBehaviour
{

    public class CanvasGroupAlphaAction : ActionNode
    {
        [SerializeField]
        private CanvasGroup canvasGroup;

        [SerializeField]
        private float alpha;

        protected override ActionState OnUpdate()
        {
            // parent update
            ActionState result = base.OnUpdate();
            if (result != ActionState.Success && result != ActionState.Running)
                return result;
            if (canvasGroup != null)
                canvasGroup.alpha = alpha;
            else
                Debug.LogError("canvasGroup is null : " + name);

            return result;

        }

    }

}