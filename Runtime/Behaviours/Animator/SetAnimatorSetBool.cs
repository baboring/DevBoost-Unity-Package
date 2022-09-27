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
    public class SetAnimatorSetBool : SetAnimatorBase
    {
        [SerializeField] string eventName;
        [SerializeField] bool value;

        protected override ActionState OnUpdate()
        {
            animatorController?.SetBool(eventName, value);

            return ActionState.Success;

        }

    }
}