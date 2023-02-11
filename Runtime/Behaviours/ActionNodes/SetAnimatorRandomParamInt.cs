/* ---------------------------------------------------------------------
 * Created on Mon Aug 19 2021 3:33:10 PM
 * Author : Benjamin Park
 * Description : Show Message Box
--------------------------------------------------------------------- */

using NaughtyAttributes;
using UnityEngine;

namespace DevBoost.ActionBehaviour
{
    public class SetAnimatorRandomParamInt : ActionNode
    {
        public Animator animator;

        [AnimatorParam("animator", AnimatorControllerParameterType.Int)]
        public string paramName;

        [SerializeField]
        private Vector2Int range;   // x ann y means in range of min max

        protected override ActionState OnUpdate()
        {
            // parent update
            ActionState result = base.OnUpdate();
            if (result != ActionState.Success)
                return result;

            int val = Random.Range(range.x, range.y);
            animator.SetInteger(paramName, val);

            return ActionState.Success;

        }


    }

}