/* ---------------------------------------------------------------------
 * Created on Mon Aug 19 2021 3:33:10 PM
 * Author : Benjamin Park
 * Description : Making Dissolve animation
--------------------------------------------------------------------- */
using DevBoost.Mecani;
using UnityEngine;

namespace DevBoost.ActionBehaviour
{

    public class PlayUITweener : ActionNode
    {
        [SerializeField]
        private UITweener tweener;
        [SerializeField]
        private bool playFoward = true;
        [SerializeField]
        private bool isReset = true;

        protected override ActionState OnUpdate()
        {
            // parent update
            ActionState result = base.OnUpdate();
            if (result != ActionState.Success && result != ActionState.Running)
                return result;

            if (isReset)
                tweener?.ResetToBeginning();
            tweener?.Play(playFoward);

            return result;

        }


    }

}