/********************************************************************
	created:	2017/07/19
	filename:	Singleton.cs
	author:		Benjamin
	purpose:	[managed singleton for MonoBehaviour and classes]
*********************************************************************/
using UnityEngine;
using System;

namespace DevBoost.Utilities {

	public abstract class Singleton<T> : SingletonBase<T> where T : class, new()
	{
		static new public T instance {
			get {
				if (_instance == null)
					return Instantiate();
				return _instance;
			}
			set {
				if (_instance != null)
					throw new System.ApplicationException("cannot set Instance twice!");

				_instance = value;
			}
		}

		public Singleton() {
			if (_instance != null)
				throw new System.ApplicationException("cannot set Instance twice!");
		}
	}


    public enum SingletonType
    {
        DestroyOnLoad = 0,
        DontDestroyOnLoad
    }

    public class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
	{
        public static T Instance
        {
            get;
            protected set;
        }
        //callback called when it destroyed
        public System.Action OnDestroyListener;

        private static object _lock = new object();

		// Returns the instance of the singleton
		public static T SafeInstance
        {
			get
			{
				lock (_lock) {
					if (null == Instance)
						return Instantiate();
				}
				return Instance;
			}
		}

        // Awake
		protected void Awake()	{
            Log.Trace("Awake singleton : " + typeof(T));
            if (null == Instance)
                Instance = (T)this;
		}

		static protected T Instantiate() {

			Instance = (T)FindObjectOfType(typeof(T));
			if (null == Instance) {
				GameObject obj = new GameObject(typeof(T).ToString());
				Instance = obj.AddComponent<T>();
				if (null == Instance)
					Debug.LogError("FATAL! Cannot create an instance of " + typeof(T) + ".");
			}
			else {
				Debug.LogError("Aleady Instance of " + typeof(T) + " exists in the scene.");
			}
			return Instance;
		}

        // initializaion singleton
        public virtual void Initialize(SingletonType type = SingletonType.DestroyOnLoad)
        {
            if (Instance == null)
            {
                Instance = this.GetComponent<T>();
                Log.Trace("[ Singleton - Instantiated ] " + typeof(T).ToString());
            }

            if (type == SingletonType.DontDestroyOnLoad)
                DontDestroyOnLoad(this.gameObject);
        }
        // Destroy
        public static void SelfDestroy()
		{
            if (null != Instance)
            {
                if (null != Instance.OnDestroyListener)
                {
                    Instance.OnDestroyListener();
                    Instance.OnDestroyListener = null;
                }

                DestroyObject(Instance.gameObject);
                Instance = null;
            }

        }

        void OnApplicationQuit()
		{
			Instance = null;
			_lock = null;
		}

		void OnDestroy()
		{
			if (this == Instance)
				return;
            if (null != this.OnDestroyListener)
            {
                this.OnDestroyListener();
                this.OnDestroyListener = null;
            }

            Instance = null;

			//Debug.Log("Singleton object destroy");
		}
	}

}