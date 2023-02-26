/* ---------------------------------------------------------------------
 * Created on Mon Aug 19 2021 3:33:10 PM
 * Author : Benjamin Park
 * Description : Delete childern action
--------------------------------------------------------------------- */

using UnityEngine;

namespace DevBoost.ActionBehaviour
{
    public class DeleteTransformChildernAction : ActionNode
    {
        [SerializeField]
        private Transform target;

        protected override ActionState OnUpdate()
        {
            // parent update
            ActionState result = base.OnUpdate();
            if (result != ActionState.Success && result != ActionState.Running)
                return result;

            if (target == null)
                return ActionState.Fail;

            target.Clear();

            return result;

        }



    }

}