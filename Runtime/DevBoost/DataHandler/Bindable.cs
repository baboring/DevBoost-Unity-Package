/* ---------------------------------------------------------------------
 * Author : Benjamin Park
--------------------------------------------------------------------- */

using System.ComponentModel;
using System.Collections.Generic;
using UnityEngine;

namespace DevBoost.Data
{
    public class Bindable : INotifyPropertyChanged
    {
        private Dictionary<string, object> _properties = new Dictionary<string, object>();

        /// <summary>
        /// Gets the value of a property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        protected T Get<T>(string name = null)
        {
            Debug.Assert(name != null, "name != null");
            object value = null;
            if (_properties.TryGetValue(name, out value))
                return value == null ? default(T) : (T)value;
            return default(T);
        }

        /// <summary>
        /// Sets the value of a property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <remarks>Use this overload when implicitly naming the property</remarks>
        protected void Set<T>(T value, string name = null)
        {
            Debug.Assert(name != null, "name != null");
            if (Equals(value, Get<T>(name)))
                return;
            _properties[name] = value;
            OnPropertyChanged(name);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class Contact : Bindable
    {
        public string FirstName
        {
            get { return Get<string>("FirstName"); }
            set { Set(value, "FirstName"); }
        }
    }

}