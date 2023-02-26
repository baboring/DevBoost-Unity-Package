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
    public class SetAnimatorSetInt : SetAnimatorBase
    {
        [SerializeField] string eventName;
        [SerializeField] int value;

        protected override ActionState OnUpdate()
        {
            animatorController?.SetInt(eventName, value);

            return ActionState.Success;

        }

    }
}