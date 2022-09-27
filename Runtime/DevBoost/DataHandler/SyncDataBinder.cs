/* ---------------------------------------------------------------------
 * Author : Benjamin Park
--------------------------------------------------------------------- */

namespace DevBoost.Data
{
    /// <summary>
    /// Async data look up
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class SyncDataBinder<T, TKey> : BaseDataBinder<T, TKey> where TKey : class, new()
    {
        public SyncDataBinder(System.Action<T> callback = null, T initValue = default(T)) : base(callback, initValue)
        {
        }

        protected override bool ReadData(out T value)
        {
            value = default(T);
            return false;
        }

        protected override void WritehData()
        {
        }
    }
}