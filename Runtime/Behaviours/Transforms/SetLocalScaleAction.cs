/* ---------------------------------------------------------------------
 * Created on Mon Aug 19 2021 3:33:10 PM
 * Author : Benjamin Park
 * Description : Set Local Scale Action
--------------------------------------------------------------------- */

using UnityEngine;

namespace DevBoost.ActionBehaviour
{
    public class SetLocalScaleAction : ActionNode
    {
        [SerializeField]
        private GameObject Target;
        [SerializeField]
        private Vector3 value = Vector3.one;

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

            if (Target != null )
                Target.transform.localScale = value;

            return result;

        }



    }

}