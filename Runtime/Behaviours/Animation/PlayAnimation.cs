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
    public class PlayAnimation : ActionNode
    {
        /// <summary>
        /// Animator instance
        /// </summary>   
        [SerializeField] private Animation ani = null;
        [SerializeField] string animationName;
        [Range(0.0f,1.0f)]
        [SerializeField] float startTime;

        /// <summary>
        /// play Animation
        /// </summary>
        /// <param name="aniName"></param>
        public void Play(string aniName)
        {
            ani.Play();
        }


        /// <summary>
        /// play Animation
        /// </summary>
        /// <param name="aniName"></param>
        /// <param name="time"></param>
        public void Play(string aniName, float time)
        {
            ani[aniName].normalizedTime = time;
            ani.Play();
        }

        /// <summary>
        /// Play Animation
        /// </summary>
        /// <param name="aniName"></param>
        /// <param name="time"></param>
        /// <param name="speed"></param>
        public void Play(string aniName, float time, float speed)
        {
            ani[aniName].normalizedTime = time;
            ani[aniName].speed = speed;
            ani.Play();
        }
        protected override ActionState OnUpdate()
        {
            this.Play(animationName, startTime);

            return ActionState.Success;

        }
    }
}
