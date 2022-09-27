/* ---------------------------------------------------------------------
 * Created on Mon Feb 11 2021 3:33:10 PM
 * Author : Benjamin Park
 * Description : Tracking Data Class
--------------------------------------------------------------------- */


namespace DevBoost.Utilities
{
    using System;
    using System.Diagnostics;
    using UnityEngine.Networking;

    /// <summary>
    /// Data Tracking Class
    /// </summary>
    public class Tracer : IDisposable
    {
        private bool disposedValue;
        protected Stopwatch timeStamp;

        public Tracer()
        {
            timeStamp = Stopwatch.StartNew();
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~Tracer()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// Tracking request time
    /// </summary>
    public class TracerWebRequest : Tracer
    {
        UnityWebRequest webReqeust = null;

        public TracerWebRequest(UnityWebRequest request) : base()
        {
#if DEBUG_CONFIG
            webReqeust = request;
#endif
        }

        protected override void Dispose(bool disposing)
        {
            if (null != webReqeust && null != webReqeust.downloadHandler)
            {
                int size = webReqeust.downloadHandler.text?.Length ?? webReqeust.downloadHandler.text?.Length ?? 0;

                Log.Trace($"[ Trace ] Time : {timeStamp.ElapsedMilliseconds} ms, size : {size}, URL : {webReqeust.url}");
            }
            webReqeust = null;
            base.Dispose(disposing);

        }

    }

}