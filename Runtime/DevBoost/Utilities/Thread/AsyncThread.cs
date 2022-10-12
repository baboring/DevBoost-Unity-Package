/* ---------------------------------------------------------------------
 * Author : Benjamin Park
--------------------------------------------------------------------- */

using System.Threading;
using System;

namespace DevBoost.Data
{
    public interface IAsyncTask
    {
        void Dispose();
    }
    /// <summary>
    /// Async Thread processor
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public class AsyncThread<T> : IDisposable, IAsyncTask
    {
        // Flag: Has Dispose already been called?
        public bool IsDisposed { get; private set; }

        private Thread m_Thread = null;
        private Func<T> m_AsyncUpdate = null;
        private Action<T> m_AsyncCallback = null;

        /// <summary>
        /// start thread and call back the result
        /// </summary>
        /// <param name="asyncUpdate"></param>
        /// <param name="callback"></param>
        public AsyncThread(Func<T> asyncUpdate, Action<T> callback = null)
        {
            m_AsyncCallback = callback;
            if (null == asyncUpdate)
            {
                Dispose();
                return;
            }
            m_AsyncUpdate = asyncUpdate;
            m_Thread = new Thread(new ThreadStart(UpdateCallback));
            m_Thread.Start();
        }

        ~AsyncThread()
        {
            Dispose(false);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    // Free any other managed objects here.
                    m_AsyncUpdate = null;
                    m_AsyncCallback = null;
                }

                // Free any unmanaged objects here.
                if (null != m_Thread)
                {
                    // Stop the thread when disabled, or it will keep running in the background
                    m_Thread.Abort();
                    m_Thread = null;
                }
                IsDisposed = true;
            }
        }
        // free resources
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void UpdateCallback()
        {
            var res = m_AsyncUpdate();

            if (null != m_AsyncCallback)
                m_AsyncCallback(res);


            Dispose();
        }
    }

}