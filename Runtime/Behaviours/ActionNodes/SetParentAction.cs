/* ---------------------------------------------------------------------
 * Created on Mon Aug 19 2021 3:33:10 PM
 * Author : Benjamin Park
 * Description : Set Parent Action
--------------------------------------------------------------------- */

using UnityEngine;

namespace DevBoost.ActionBehaviour
{
    public class SetParentAction : ActionNode
    {
        [SerializeField]
        private GameObject Target;
        [SerializeField]
        private Transform Context;

        [SerializeField]
        private bool isResetLocal = false;
        [SerializeField]
        private bool isWorldPositionStay = false;
        protected override void OnReset()
        {
            base.OnReset();

            if (Target == null)
                Target = this.gameObject;
        }

        protected override ActionState OnUpdate()
        {
            // parent update
            ActionState result = base.OnUpdate();
            if (result != ActionState.Success && result != ActionState.Running)
                return result;

            if (Target != null || Context != null)
            {
                Target.transform.SetParent(Context, isWorldPositionStay);

                if (isResetLocal)
                {
                    Target.transform.localPosition = Vector3.zero;
                    Target.transform.localScale = Vector3.one;
                }
            }
            return result;

        }



    }

}