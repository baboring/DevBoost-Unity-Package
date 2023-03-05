/* ---------------------------------------------------------------------
 * Author : Benjamin Park
--------------------------------------------------------------------- */
using System.Collections.Generic;

namespace DevBoost.Data
{
    public static class SyncDataDeliver
    {
        public class Facade
        {
            protected Dictionary<System.Type, DataInvokeHandler<object>> syncData = new Dictionary<System.Type, DataInvokeHandler<object>>();

            public DisposeCallback<T> Register<T>(System.Action<T> callback) where T : class
            {
                if (syncData.ContainsKey(typeof(T)))
                    syncData[typeof(T)].AddCallback(callback as System.Action<object>);
                else
                    syncData.Add(typeof(T), new DataInvokeHandler<object>(callback as System.Action<object>));

                return new DisposeCallback<T>() { callback = callback };
            }

            public void Unregister<T>(System.Action<T> callback) where T : class
            {
                if (syncData.ContainsKey(typeof(T)))
                    syncData[typeof(T)].RemoveCallback(callback as System.Action<object>);
            }
            public void UpdateSyncData<T>(T msg)
            {
                if (syncData.ContainsKey(msg.GetType()))
                {
                    syncData[msg.GetType()].Value = msg;
                }
            }
        }

        public class DisposeCallback<T> where T : class
        {
            public System.Action<T> callback;

            public void Assign(System.Action disposer)
            {
                disposer += ()=> facade.Unregister<T>(callback);
            }
        }

        static private Facade facade = new Facade();
        static public DisposeCallback<T> Receiver<T>(System.Action<T> callback) where T : class => facade.Register<T>(callback);
        static public void Remove<T>(System.Action<T> callback) where T : class => facade.Unregister<T>(callback);
        static public void Publish<T>(T obj) => facade.UpdateSyncData<T>(obj);

    }

}