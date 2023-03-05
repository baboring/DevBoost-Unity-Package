/* ---------------------------------------------------------------------
 * Author : Benjamin Park
--------------------------------------------------------------------- */


namespace DevBoost.Data
{

    /// <summary>
    /// General class for BaseDataInvokeHandler
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataInvokeHandler<T> : BaseDataInvokeHandler<T>
    {
        // constructor
        public DataInvokeHandler(System.Action<T> callback = null, T initValue = default(T))
        {
            AddCallback(callback);
            m_data = initValue;
        }

    }

    /// <summary>
    /// base class for steam data notification
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseDataInvokeHandler<T> : ICallbackInvoker<T>
    {
        /// <summary>
        /// Value
        /// </summary>
        protected T m_data;

        /// <summary>
        /// Callback actions
        /// </summary>
        protected System.Action<T> m_NotifierValue;
        protected System.Action m_NotifierCall;

        /// <summary>
        /// Value
        /// </summary>
        public T Value
        {
            get { return m_data; }
            set {
                if (!Equals(value, m_data))
                {
                    m_data = value;
                    Notify(value);
                }
            }
        }


        /// <summary>
        /// Notify Updated for the writting or callback receiver
        /// </summary>
        /// <param name="newValue"></param>
        protected virtual void Notify(T newValue)
        {
            if(null != m_NotifierValue)
                m_NotifierValue(newValue);
            if(null != m_NotifierCall)
                m_NotifierCall();
        }

        // Add Callback
        public void AddCallback(System.Action<T> callback)
        {
            if (callback != null)
                m_NotifierValue += callback;
        }

        // Remove Callback
        public void RemoveCallback(System.Action<T> callback)
        {
            if (callback != null)
                m_NotifierValue -= callback;
        }

        // add callback
        public void AddCallback(System.Action callback)
        {
            m_NotifierCall += callback;
        }

        // Remove Callback
        public void RemoveCallback(System.Action callback)
        {
            m_NotifierCall -= callback;
        }

    }
}