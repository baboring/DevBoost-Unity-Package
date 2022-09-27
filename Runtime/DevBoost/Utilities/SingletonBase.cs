
/********************************************************************
	created:	2017/07/19
	filename:	Singleton.cs
	author:		Benjamin
	purpose:	[managed singleton for MonoBehaviour and classes]
*********************************************************************/
using UnityEngine;
using System;

namespace DevBoost.Utilities 
{ 

	public abstract class SingletonBase<T> where T : class, new()
	{
		static protected bool m_ShuttingDown = false;

		static protected T _instance;
		static public T Instance
		{
			get {
				if (m_ShuttingDown)
				{
					Debug.LogWarning("[Singleton] Instance '" + typeof(T) + "' already destroyed. Returning null.");
					return null;
				}
				return _instance; 
			}
			set
			{
				if (_instance != null)
					throw new System.ApplicationException("cannot set Instance twice!");

				_instance = value;
			}
		}

		protected SingletonBase() {
			if (_instance != null)
				throw new System.ApplicationException("cannot set Instance twice!");
		}

		static public bool IsInstanced { get { return  null != _instance; } }

		static protected T Instantiate()
		{
			Instance = new T();
			return Instance;
		}

		static public void Destroy()
		{
			_instance = null;
		}

		/// <summary>
		/// set the setting shutting down
		/// </summary>
		private void OnApplicationQuit()
		{
			m_ShuttingDown = true;
		}

	}

}