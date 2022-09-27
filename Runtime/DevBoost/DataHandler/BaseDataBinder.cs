/* ---------------------------------------------------------------------
 * Author : Benjamin Park
--------------------------------------------------------------------- */

using System;

namespace DevBoost.Data
{

    /// <summary>
    /// base class for steam data notification
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseDataBinder<T, TKey> : DataInvokeHandler<T>, ISyncDataInvoker<TKey> where TKey : class, new()
    {
        /// <summary>
        /// Bind Key (share)
        /// </summary>
        protected TKey m_Bindkey;

        /// <summary>
        /// option whether to be able to update
        /// </summary>
        private bool m_isAllowedUpload = true;
        /// <summary>
        /// 
        /// </summary>
        private Action<ISyncDataInvoker<TKey>> m_NotifierKey;

        public BaseDataBinder(System.Action<T> callback = null, T initValue = default(T)) : base(callback, initValue)
        {
        }

        public bool IsAllowedUpload
        {
            get { return m_isAllowedUpload; }
            set { m_isAllowedUpload = value; }
        }

        // id key
        public TKey BindKey { get { return m_Bindkey; } }


        protected abstract void WritehData();
        protected abstract bool ReadData(out T value);

        public virtual void SetValue(T value)
        {
            Value = value;
        }
        /// <summary>
        /// Notify Updated
        /// </summary>
        /// <param name="newValue"></param>
        protected override void Notify(T newValue)
        {
            base.Notify(newValue);
            if(null != m_NotifierKey)
                m_NotifierKey(this);
        }

        /// <summary>
        /// cause updated the inside of the value
        /// </summary>
        public void NotifyForced()
        {
            Notify(m_data);
        }

        // Add Callback
        public void Register(Action<ISyncDataInvoker<TKey>> callback)
        {
            m_NotifierKey += callback;
        }

        public void Unregister(Action<ISyncDataInvoker<TKey>> callback)
        {
            m_NotifierKey -= callback;
        }

        public void ApplyData()
        {
            WritehData();
        }

        /// <summary>
        /// Look up and Notify
        /// </summary>
        /// <param name="steamHubLobby"></param>
        public virtual void LookupData()
        {
            T value;
            if (ReadData(out value))
            {
                if (!Equals(value, m_data))
                    Value = value;
            }
        }

  
    }
}