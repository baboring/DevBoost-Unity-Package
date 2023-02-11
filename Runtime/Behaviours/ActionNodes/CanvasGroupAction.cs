/* ---------------------------------------------------------------------
 * Created on Mon Aug 19 2021 3:33:10 PM
 * Author : Benjamin Park
 * Description : Canvas Group Alpha
--------------------------------------------------------------------- */
using UnityEngine;

namespace DevBoost.ActionBehaviour
{

    public class CanvasGroupAction : ActionNode
    {
        [SerializeField]
        private CanvasGroup canvasGroup;

        [SerializeField]
        private bool interactable;
        [SerializeField]
        private bool blockRaycasts;
        [SerializeField]
        private bool ignoreParentGroup;

        protected override ActionState OnUpdate()
        {
            // parent update
            ActionState result = base.OnUpdate();
            if (result != ActionState.Success && result != ActionState.Running)
                return result;
            if (canvasGroup != null)
            {
                canvasGroup.interactable = interactable;
                canvasGroup.blocksRaycasts = blockRaycasts;
                canvasGroup.ignoreParentGroups = ignoreParentGroup;
            }
            else
                Debug.LogError("canvasGroup is null : " + name);

            return result;

        }

    }

}