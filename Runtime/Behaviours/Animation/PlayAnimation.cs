/* *************************************************
*  Created:  2021-2-16 10:51:59
*  File:     PlayAnimation.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevBoost.ActionBehaviour
{
    /// <summary>
    /// Play Animation Clip
    /// </summary> 
    [RequireComponent(typeof(AnimationController))]
    public class PlayAnimation : ActionNode
    {
        [SerializeField] string animationName;
        [Range(0.0f,1.0f)]
        [SerializeField] float startTime;

        protected override ActionState OnUpdate()
        {
            var ani = GetComponent<AnimationController>();

            ani.Play(animationName, startTime);

            return ActionState.Success;

        }
    }
}
