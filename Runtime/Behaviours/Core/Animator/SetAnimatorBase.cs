/* *************************************************
*  Created:  2018-1-28 19:46:32
*  File:     SetAnimatorBase.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using UnityEngine;

namespace DevBoost.ActionBehaviour
{
    /// <summary>
    /// Set animator base.
    /// </summary>
    public abstract class SetAnimatorBase : ActionNode
    {
        [SerializeField]
        protected AnimatorController animatorController;

        public AnimatorController controller => animatorController;

        protected override void OnReset()
        {
            base.OnReset();
            if (animatorController == null)
                animatorController = GetComponent<AnimatorController>();
        }

    }
}