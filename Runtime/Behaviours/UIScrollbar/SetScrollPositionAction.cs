/* ---------------------------------------------------------------------
 * Created on Mon Aug 19 2021 3:33:10 PM
 * Author : Benjamin Park
 * Description : Set scroll Position Action
--------------------------------------------------------------------- */

using UnityEngine;
using UnityEngine.UI;

namespace DevBoost.ActionBehaviour
{
    public class SetScrollPositionAction : ActionNode
    {
        [SerializeField]
        private Scrollbar Target;
        [SerializeField]
        private float value;

        protected override void OnReset()
        {
            base.OnReset();
        }

        protected override ActionState OnUpdate()
        {
            // parent update
            ActionState result = base.OnUpdate();
            if (result != ActionState.Success && result != ActionState.Running)
                return result;

            if (Target != null)
                Target.value = value;

            return result;

        }



    }

}