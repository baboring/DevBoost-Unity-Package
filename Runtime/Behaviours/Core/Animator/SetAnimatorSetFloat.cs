/* *************************************************
*  Created:  2018-1-28 19:46:32
*  File:     SetAnimatorSetInt.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using UnityEngine;

namespace DevBoost.ActionBehaviour
{
    /// <summary>
    /// Set animator integer.
    /// </summary>
    public class SetAnimatorSetFloat : SetAnimatorBase
    {
        [SerializeField] string eventName;
        [SerializeField] float value;

        protected override ActionState OnUpdate()
        {
            animatorController?.SetFloat(eventName, value);

            return ActionState.Success;

        }

    }
}