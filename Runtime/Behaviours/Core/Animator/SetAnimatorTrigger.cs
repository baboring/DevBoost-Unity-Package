/* *************************************************
*  Created:  2018-1-28 19:46:32
*  File:     SetAnimatorTrigger.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using UnityEngine;

namespace DevBoost.ActionBehaviour
{
    /// <summary>
    /// Set animator trigger.
    /// </summary>
    public class SetAnimatorTrigger : SetAnimatorBase
    {
        [SerializeField] string eventName;

        protected override ActionState OnUpdate()
        {
            animatorController.SetTrigger(eventName);

            return ActionState.Success;

        }

    }
}