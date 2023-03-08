/* ---------------------------------------------------------------------
 * Author : Benjamin Park
--------------------------------------------------------------------- */

using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace DevBoost.Data
{
    // general value observer
    public class DataTable
    {
        private Dictionary<Type, object> dataTable = new Dictionary<Type, object>();

        #region Util Functions

        /// <summary>
        /// Update data table with new data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public void UpdateTable<T>(Dictionary<string, T> list)
        {
            UnityEngine.Debug.Assert(list != null, $"'{typeof(T)}' data sheet is null");
            if (list == null)
                return;
            if (!dataTable.ContainsKey(typeof(T)))
            {
                dataTable.Add(typeof(T), list);
                return;
            }

            // update
            var table = dataTable[typeof(T)] as Dictionary<string, T>;
            foreach (var item in list)
            {
                if (table.ContainsKey(item.Key))
                    table[item.Key] = item.Value;
                else
                    table.Add(item.Key, item.Value);
            }

        }

        public Dictionary<string, T> GetTable<T>()
        {
            if (dataTable.ContainsKey(typeof(T)))
                return dataTable[typeof(T)] as Dictionary<string, T>;
            return null;
        }




        /// <summary>
        /// Get Data by key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TryGet<T>(object key, out T result)
        {
            result = default(T);
            if (key == null)
            {
                Debug.LogError("key is null");
                return false;
            }
            var table = GetTable<T>();
            return null != table && table.TryGetValue(key.ToString(), out result);
        }

        /// <summary>
        /// Get data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TryGet<T>(Predicate<T> predicate, out T result)
        {
            var table = FetchAll<T>();
            if (null != table)
            {
                var find = table.Where(va => predicate(va));
                if (find.Count() > 0)
                {
                    result = find.First();
                    return true;
                }
            }
            //UnityEngine.Debug.Assert(false, $"not found data from the table '{typeof(T)}'");
            result = default(T);
            return false;
        }

        /// <summary>
        /// Get data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            var table = GetTable<T>();
            return null != table && table.ContainsKey(key) ? table[key] : default(T);
        }

        /// <summary>
        /// Get data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public T Get<T>(object key)
        {
            var table = GetTable<T>();
            return null != table && table.ContainsKey(key.ToString()) ? table[key.ToString()] : default(T);
        }

        /// <summary>
        /// Get data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public T Find<T>(Predicate<T> predicate)
        {
            var table = FetchAll<T>();
            if (null != table)
                return table.Where(va => predicate(va)).FirstOrDefault();
            return default(T);
        }

        /// <summary>
        /// Get All elements with enumerable interface
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> FetchAll<T>()
        {
            if (dataTable.ContainsKey(typeof(T)))
                return (dataTable[typeof(T)] as Dictionary<string, T>).Values;
            return null;
        }

        /// <summary>
        /// Fetch Data with condition
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public List<T> Fetch<T>(Predicate<T> predicate)
        {
            var table = FetchAll<T>();
            if (null != table)
            {
                return table.Where(va => predicate(va)).ToList();
            }
            return new List<T>(0);
        }


        /// <summary>
        /// Fetch Data with condition
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public IEnumerable<T> FetchItems<T>(Predicate<T> predicate)
        {
            var table = FetchAll<T>();
            if (null != table)
            {
                return table.Where(va => predicate(va));
            }
            return null;
        }

        #endregion
    }

}