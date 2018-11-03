
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
		static protected T _instance;
		static public T Instance
		{
			get { return _instance; }
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



	}

}