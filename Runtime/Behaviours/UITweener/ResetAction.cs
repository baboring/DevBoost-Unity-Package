/* ---------------------------------------------------------------------
 * Created on Mon Aug 19 2021 3:33:10 PM
 * Author : Benjamin Park
 * Description : Reset action
--------------------------------------------------------------------- */

using UnityEngine;

namespace DevBoost.ActionBehaviour
{
    public class ResetAction : ActionNode
    {
        [SerializeField]
        private ActionNode target;


        protected override ActionState OnUpdate()
        {
            // parent update
            ActionState result = base.OnUpdate();
            if (result != ActionState.Success && result != ActionState.Running)
                return result;

            target.Reset();

            return result;

        }



    }

}