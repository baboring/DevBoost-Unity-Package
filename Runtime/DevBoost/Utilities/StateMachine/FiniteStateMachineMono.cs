
/********************************************************************
	created:	2021/07/05
	filename:	FiniteStateMachineMono.cs
	author:		Benjamin
	purpose:	[state machine for mono]
*********************************************************************/
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;

namespace DevBoost.Utilities
{
    public abstract class FiniteStateNode<T>
    {
        public T State { get; private set; }

        public FiniteStateNode(T state)
        {
            State = state;
        }

        public abstract void OnEnter();
        public abstract void OnLeave();
        public abstract void OnUpdate();
        public abstract void OnReset();
    }


    public class FiniteStateMachineMono<T> : MonoBehaviour where T : System.Enum
    {
        [ShowNativeProperty]
        public T Current { get; private set; }
        [ShowNativeProperty]
        public T Previous { get; private set; }
        public System.Action<T> ChangeListener { get; set; }

        protected IEnumerable<FiniteStateNode<T>> StateAll { get { return states.Values; } }
        private Dictionary<T, FiniteStateNode<T>> states = new Dictionary<T, FiniteStateNode<T>>();
        private FiniteStateNode<T> currNode;

        private Queue<T> queue = new Queue<T>();
        private bool isStartup = true;
        private bool inTransaction = false;
        public bool Advance(T newState)
        {
            //Debug.Log($"[ FSM ] Advance : {newState}");
            if (inTransaction)
            {
                queue.Enqueue(newState);
                return false;
            }
            if (Current.Equals(newState) && !isStartup)
                return false;
            // check state
            if (states.ContainsKey(newState))
            {
                isStartup = false;
                inTransaction = true;
                try
                {
                    // exit previous state
                    currNode?.OnLeave();
                    currNode = null;

                    Previous = Current;
                    Current = newState;

                    currNode = states[newState];
                    // enter current state
                    currNode.OnEnter();
                    isStartup = true;
                    ChangeListener?.Invoke(Current);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    throw e;
                }
                inTransaction = false;
                if (queue.Count > 0)
                    Advance(queue.Dequeue());

                return true;
            }

            Debug.LogWarning($"not found key !! : {newState}");

            return false;
        }

        /// <summary>
        /// initial state
        /// </summary>
        /// <param name="prev"></param>
        /// <param name="curr"></param>
        public void Initial(T curr, T prev)
        {
            if (states.ContainsKey(prev))
            {
                Previous = prev;
                currNode = states[prev];
            }
            Advance(curr);
        }

        /// <summary>
        /// register state node
        /// </summary>
        /// <param name="stateNode"></param>
        /// <returns></returns>
        public bool Register(FiniteStateNode<T> stateNode)
        {
            if (states.ContainsKey(stateNode.State))
            {
                Debug.LogError($"Duplicated key !! : {stateNode.State}");
                return false;
            }
            states.Add(stateNode.State, stateNode);
            return true;
        }

        public FiniteStateNode<T> GetState(T state)
        {
            if (states.ContainsKey(state))
                return states[state];
            return null;
        }

        public FiniteStateNode<T> Find(Type stateValue)
        {
            return StateAll.FirstOrDefault(va => va.GetType() == stateValue);
        }

        public void ResetAll()
        {
            foreach (var val in StateAll)
                val.OnReset();
        }

        // Update current state
        protected virtual void Update()
        {
            currNode?.OnUpdate();
        }
    }

}