/* ---------------------------------------------------------------------
 * Author : Benjamin Park
--------------------------------------------------------------------- */

using System.Collections.Generic;
using UnityEngine;

namespace DevBoost.Data
{
    /// <summary>
    /// Base class for Subscriber
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class InvokerCallbackList<TKey> where TKey : class, new()
    {
        private List<IKeyInvoker<TKey>> m_List = new List<IKeyInvoker<TKey>>();

        // properties
        public List<IKeyInvoker<TKey>> List
        {
            get { return m_List; }
        }

        // find Invoker
        public IKeyInvoker<TKey> Find(TKey key)
        {
            return m_List.Find(va => va.BindKey == key);
        }
        // find Invoker
        public IKeyInvoker<TKey> Find(int hash)
        {
            return m_List.Find(va => va.GetHashCode() == hash);
        }
        public IKeyInvoker<TKey> Find(System.Type type)
        {
            return m_List.Find(va => va.GetType() == type);
        }

        // Add Invoker
        public void Add(IKeyInvoker<TKey> callback)
        {
            Debug.Assert(null != callback, "callback is null");
            if(null != callback)
                m_List.Add(callback);
        }
        public void Remove(IKeyInvoker<TKey> callback)
        {
            Debug.Assert(null != callback, "callback is null");
            if (null != callback)
                m_List.Remove(callback);
        }

        /// <summary>
        /// Add Callback event
        /// </summary>
        /// <param name="bindkey"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public bool AddCallback(TKey bindkey, System.Action callback)
        {
            var foundCallback = Find(bindkey);
            //Debug.Assert(null != foundCallback, "Not found Key : " + key);
            if (null != foundCallback)
            {
                foundCallback.AddCallback(callback);
                return true;
            }
            return false;
        }

        // Remove Callback
        public bool RemoveCallback(TKey bindkey, System.Action callback)
        {
            var found = Find(bindkey);
            //Debug.Assert(null != found, "Not found Key : " + bindkey);
            if (null != found)
            {
                found.RemoveCallback(callback);
                return true;
            }
            return false;
        }

    }

}