/* *************************************************
*  Created:  2021-11-01 19:46:32
*  File:     StateMachineHandler.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using System;
using UnityEngine;

namespace DevBoost.ActionBehaviour
{
    public class StateMachineInfo
    {
        public Animator animator;
        public AnimatorStateInfo stateInfo;
        public int layer;
    }

    public sealed class StateMachineHandler : StateMachineBehaviour
    {
        [SerializeField]
        private bool triggerEnter;
        [SerializeField]
        private bool triggerUpdate;
        [SerializeField]
        private bool triggerExit;

        public enum Trigger
        {
            Enter,
            Exit,
            Update
        }



        public event Action<StateMachineInfo> OnEnterState = null;
        public event Action<StateMachineInfo> OnExitState = null;
        public event Action<StateMachineInfo> OnUpdateState = null;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Using the shortNameHash to avoid dealing with layers. Can use nameHash instead.
            if (OnEnterState != null && triggerEnter)
                OnEnterState.Invoke(new StateMachineInfo() 
                {
                    animator = animator,
                    stateInfo = stateInfo,
                    layer = layerIndex
                });
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (OnExitState != null && triggerExit)
                OnExitState.Invoke(new StateMachineInfo() 
                {
                    animator = animator,
                    stateInfo = stateInfo,
                    layer = layerIndex
                });
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);
            if (OnUpdateState != null && triggerUpdate)
                OnUpdateState.Invoke(new StateMachineInfo()
                {
                    animator = animator,
                    stateInfo = stateInfo,
                    layer = layerIndex
                });
        }
    }

}