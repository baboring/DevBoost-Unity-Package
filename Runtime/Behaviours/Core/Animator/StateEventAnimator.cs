/* *************************************************
*  Created:  2021-11-01 19:46:32
*  File:     StateEventAnimator.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DevBoost.ActionBehaviour
{

    public sealed class StateEventAnimator : MonoBehaviour
    {
        public enum HashType
        {
            State,
            Tag
        }

        [SerializeField]
        private Animator animator = null;

        [SerializeField]
        private HashType hashType;

        /// <summary>
        /// List of enter animation events registered
        /// </summary>
        [SerializeField, ReorderableList]
        public List<AnimationEvent> EnterAnimationEvents = null;

        /// <summary>
        /// List of exit animation events registered
        /// </summary>
        [SerializeField, ReorderableList]
        public List<AnimationEvent> ExitAnimationEvents = null;

        /// <summary>
        /// List of enter animation events registered
        /// </summary>
        [SerializeField, ReorderableList]
        public List<AnimationEvent> UpdateAnimationEvents = null;

        private sealed class StateCallbacks
        {
            public int hash;
            public Action callbackEnter = null;
            public Action callbackExit = null;
            public Action callbackUpdate = null;
        }

        private StateCallbacks currentState = null;
        private Dictionary<int, StateCallbacks> registeredStates = new Dictionary<int, StateCallbacks>();

        private void Awake()
        {
            if (animator == null)
                animator = GetComponent<Animator>();
            Initial();
        }

        private void Initial()
        {
            if (animator == null)
                return;
            // Assuming the animator is assigned in the inspector
            foreach (var stateBehaviour in animator.GetBehaviours<StateMachineHandler>())
            {
                if (stateBehaviour != null)
                {
                    stateBehaviour.OnEnterState += OnEnterState;
                    stateBehaviour.OnExitState += OnExitState;
                    stateBehaviour.OnUpdateState += OnUpdateState;
                }
            }

            // hash
            foreach( var aniEvent in EnterAnimationEvents)
                aniEvent.hash = Animator.StringToHash(aniEvent.animationEventId);
            foreach (var aniEvent in ExitAnimationEvents)
                aniEvent.hash = Animator.StringToHash(aniEvent.animationEventId);
            foreach (var aniEvent in UpdateAnimationEvents)
                aniEvent.hash = Animator.StringToHash(aniEvent.animationEventId);
        }

        public void RegisterStateCallback(string eventName, Action onEnter, Action onExit = null, Action onUpdate = null)
        {
            // Using the state names to register, but using the hash internally
            int shortNameHash = Animator.StringToHash(eventName);
            if (!string.IsNullOrEmpty(eventName) && registeredStates.ContainsKey(shortNameHash) == false)
            {
                registeredStates.Add(shortNameHash, new StateCallbacks()
                {
                    hash = shortNameHash,
                    callbackEnter = onEnter,
                    callbackExit = onExit,
                    callbackUpdate = onUpdate,
                });
            }
        }

        public void Trigger(string trigger)
        {
            animator.SetTrigger(trigger);
        }

        private void OnEnterState(StateMachineInfo stateInfo)
        {
            int hash = hashType == HashType.State ? stateInfo.stateInfo.shortNameHash : stateInfo.stateInfo.tagHash;
            if (registeredStates.ContainsKey(hash))
            {
                if (currentState != null)
                    currentState.callbackExit?.Invoke();

                EnterState(hash);
            }

            if (EnterAnimationEvents.Count > 0)
            {
                foreach(var info in EnterAnimationEvents)
                    if (info.hash == hash)
                        info.RunEvent();
            }
        }

        private void OnUpdateState(StateMachineInfo stateInfo)
        {
            int hash = hashType == HashType.State ? stateInfo.stateInfo.shortNameHash : stateInfo.stateInfo.tagHash;
            if (registeredStates.TryGetValue(hash, out StateCallbacks state))
                state?.callbackUpdate?.Invoke();

            if (UpdateAnimationEvents.Count > 0)
            {
                foreach (var info in UpdateAnimationEvents)
                    if (info.hash == hash)
                        info.RunEvent();
            }
        }


        private void OnExitState(StateMachineInfo stateInfo)
        {
            int hash = hashType == HashType.State ? stateInfo.stateInfo.shortNameHash : stateInfo.stateInfo.tagHash;
            if (currentState != null && currentState.hash == hash)
            {
                // Copying the callback just in case the callbackExit triggers a OnEnterState
                // This would lead to nulling the new current state instead of the one we want
                var callbackExit = currentState.callbackExit;
                currentState = null;
                callbackExit?.Invoke();
            }

            if (ExitAnimationEvents.Count > 0)
            {
                foreach (var info in ExitAnimationEvents)
                    if (info.hash == hash)
                        info.RunEvent();
            }

        }

        private void EnterState(int stateNameHash)
        {
            if (registeredStates.ContainsKey(stateNameHash))
            {
                currentState = registeredStates[stateNameHash];
                currentState.callbackEnter?.Invoke();
            }
        }

    }

}