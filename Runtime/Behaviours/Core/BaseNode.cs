/* *************************************************
*  Created:  2018-1-28 19:46:40
*  File:     BaseNode.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/
using System;
using System.Collections;
using UnityEngine;

namespace DevBoost.ActionBehaviour
{

    public enum ActionState
    {
        None = 0,
        Success,
        Fail,
        Running,
        Error = -1,

    }

    public interface IBaseNode
    {
        Type agentType { get; }
        IBaseNode agent { get; }
    }

    // Base class
    public abstract class BaseNode : MonoBehaviour, IBaseNode
    {

        protected System.Action onSuccessCallback;
        protected ActionState _curr_state;
        protected ActionState _prev_state;

        public bool isDisabled { get; protected set; }
        public virtual Type agentType { get { return GetType(); } }
        public IBaseNode agent { get { return this; } }
        // state
        public ActionState state
        {
            get { return _curr_state; }

            protected set {
                if (_curr_state != value)
                {
                    _prev_state = _curr_state;
                    _curr_state = value;
                    Debug.Assert(value != ActionState.Fail && value != ActionState.Error,"Action:" + value);
                    // notify state changed
                    onChangedState?.Invoke(_curr_state, _prev_state);
                }
            }
        }

        // delegates
        public System.Action<ActionState, ActionState> onChangedState;

        protected void OnSuccess()
        {
            if (onSuccessCallback != null)
                onSuccessCallback?.Invoke();
        }


        // Hierarchy functions
        protected virtual void OnReset() { }

        public void ExecuteThen(System.Action callback, bool isReset = false)
        {
            if (state == ActionState.Success && !isReset)
                callback?.Invoke();
            else
            {
                RemoveListener(callback);
                AddListener(callback);
                ExecuteInvoke();
            }
        }

        public void SetListener(System.Action callback)
        {
            onSuccessCallback = callback;
        }

        public void AddListener(System.Action callback)
        {
            onSuccessCallback += callback;
        }
        public void RemoveListener(System.Action callback)
        {
            onSuccessCallback -= callback;
        }

        public void RemoveAllListener()
        {
            onSuccessCallback = null;
        }

        // Interface methods
        public void Reset()
        {
            _curr_state = ActionState.None;
            _prev_state = ActionState.None;

            OnReset();
        }

        // OnPostUpdate functions
        protected virtual void OnPostUpdate() { }

        // ready
        protected virtual ActionState OnUpdate()
        {
            return state == ActionState.None ? ActionState.Success : state;
        }

        // default core function
        public virtual ActionState Execute(bool reset = true)
        {
            if (isDisabled)
                return ActionState.Fail;
            //Debug.Log($"Execute | [ {this.GetType()}, {reset}  ] : {name}");
            // start up 
            if (reset)
                OnReset();

            // update 
            var result = state = OnUpdate();

            if (result == ActionState.Success)
                OnSuccess();

            // post update
            OnPostUpdate();

            return result;
        }

        public void ExecuteInvoke()
        {
            Execute();
        }

        void OnApplicationQuit()
        {
            isDisabled = true;
        }
    }

    // Aync proc
    public class AsyncProcessing
    {
        public event System.Action onCompleted;
        bool _isDone;

        // properties
        public bool isDone
        {
            get {
                return _isDone;
            }
            set {
                _isDone = value;
                if (_isDone)
                {
                    onCompleted?.Invoke();
                    backgroundWorking = null;
                }
            }
        }

        public Coroutine backgroundWorking;
        public AsyncProcessing(Coroutine backgroundWorking)
        {
            this.backgroundWorking = backgroundWorking;
            _isDone = false;
        }
    }

}