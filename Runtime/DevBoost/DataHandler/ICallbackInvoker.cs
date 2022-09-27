/* ---------------------------------------------------------------------
 * Author : Benjamin Park
--------------------------------------------------------------------- */

using System;

namespace DevBoost.Data
{
    /// <summary>
    /// interface for data notification
    /// </summary>
    public interface ICallbackInvoker<T>
    {
        T Value { get; }
        void AddCallback(System.Action<T> callback);
        void RemoveCallback(System.Action<T> callback);
    }

    /// <summary>
    /// interface for data notification
    /// </summary>
    public interface IKeyInvoker<TKey> where TKey : class, new()
    {
        TKey BindKey { get; }
        void AddCallback(System.Action callback);
        void RemoveCallback(System.Action callback);
    }

    /// <summary>
    /// interface for data notification
    /// </summary>
    public interface ISyncDataInvoker<TKey> : IKeyInvoker<TKey> where TKey : class, new()
    {
        void Register(Action<ISyncDataInvoker<TKey>> callback);    // for surbscriber
        void Unregister(Action<ISyncDataInvoker<TKey>> callback);
        void ApplyData(); // for network link or database
        void LookupData(); // for network link or database
    }

}