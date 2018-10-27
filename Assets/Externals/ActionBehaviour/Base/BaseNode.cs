/* *************************************************
*  Created:  2018-1-28 19:46:40
*  File:     BaseNode.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/
using System;
using System.Collections;
using UnityEngine;

namespace ActionBehaviour
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
        ActionState OnUpdate();
        Type agentType { get; }
        IBaseNode agent { get; }
    }

    // Base class
    public abstract class BaseNode : MonoBehaviour,IBaseNode {

        protected ActionState _curr_state;
        protected ActionState _prev_state;

        public virtual Type agentType { get { return GetType(); } }
        public IBaseNode agent { get { return this; } }
        // state
        public ActionState state {
            get { return _curr_state; }
             
            protected set
            {
                if(_curr_state != value) {
                    _prev_state = _curr_state;
                    _curr_state = value;
                    // notify state changed
                    if (null != onChangedState)
                        onChangedState(_curr_state,_prev_state);
                }
            }
        }

        // delegates
        public System.Action<ActionState,ActionState> onChangedState;


        // Hierarchy functions
        protected virtual void OnReset() {}


        // Interface methods
        public void Reset() { 
            _curr_state = ActionState.None; 
            _prev_state = ActionState.None; 

            OnReset(); 
        }

		// ready
        public virtual ActionState OnUpdate()
        {
            return state == ActionState.None ? ActionState.Success : state;
        }

		// default core function
		public virtual ActionState Execute() {

			// start up 
            OnReset();

			// update 
		    state = OnUpdate();

			return state;
		}
	}

    // Aync proc
    public class AsyncProcessing
    {
        public event System.Action<AsyncProcessing> onCompleted;
        bool _isDone;

        // properties
        public bool isDone
        {
            get
            {
                return _isDone;
            }
            set
            {
                _isDone = value;
                if (_isDone && null != onCompleted)
                {
                    onCompleted(this);
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