/********************************************************************
	created:	2017/07/19
	filename:	Singleton.cs
	author:		Benjamin
	purpose:	[managed singleton for MonoBehaviour and classes]
*********************************************************************/
using UnityEngine;
using System.Diagnostics;

namespace DevBoost.Utilities 
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public abstract class Singleton<T> : SingletonBase<T> where T : class, ISingleton, new()
    {
        static new public T Instance
        {
            get {
                return _instance ?? Instantiate();
            }
            set {
                if (_instance != null)
                    throw new System.ApplicationException("cannot set Instance twice!");

                _instance = value;
            }
        }

        protected Singleton()
        {
            if (_instance != null)
                throw new System.ApplicationException("cannot set Instance twice!");
        }

        private string GetDebuggerDisplay()
        {
            return ToString();
        }


    }


    public enum SingletonType
    {
        DestroyOnLoad = 0,
        DontDestroyOnLoad
    }

    public class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
    {
        [SerializeField] private SingletonType m_SingletonType = SingletonType.DestroyOnLoad;

        public static T Instance
        {
            get;
            protected set;
        }

        //callback called when it destroyed
        public System.Action OnDestroyListener;
        public System.Action OnInstantiateListener;

        static object _lock = new object();

        static public bool IsInstanced => !Instance.IsNull();
        // Returns the instance of the singleton
        public static T SafeInstance
        {
            get {
                if (_lock != null)
                {
                    lock (_lock)
                    {
                        if (null == Instance)
                            return Instantiate();
                    }
                }
                return Instance;
            }
        }

        // Awake
        protected void Awake()
        {
            Log.Trace(" [ Singleton ] Awake : {0},{1}", typeof(T), m_SingletonType);
            if (null == Instance)
            {
                Instance = this as T;
                if (m_SingletonType == SingletonType.DontDestroyOnLoad)
                {
                    if (!gameObject.IsDontDestroyOnLoad())
                        DontDestroyOnLoad(this.gameObject);
                }
                OnInstantiateListener?.Invoke();
            }
            else
            {
                enabled = false;
            }
        }

        static protected T Instantiate()
        {

            Instance = (T)FindObjectOfType(typeof(T));
            if (null == Instance)
            {
                GameObject obj = new GameObject(typeof(T).ToString());
                Instance = obj.AddComponent<T>();
                if (null == Instance)
                    UnityEngine.Debug.LogError("FATAL! Cannot create an instance of " + typeof(T) + ".");
                Instance?.OnInstantiated();
            }
            else
            {
                UnityEngine.Debug.LogError("Aleady Instance of " + typeof(T) + " exists in the scene.");
            }
            return Instance;
        }

        // initializaion singleton
        public virtual void Initialize(SingletonType type = SingletonType.DestroyOnLoad)
        {
            if (Instance == null)
            {
                Instance = Instantiate();
                Log.Trace(" [ Singleton ] Instantiated :: " + typeof(T).ToString());
            }

            if (type == SingletonType.DontDestroyOnLoad)
            {
                m_SingletonType = type;
                if (!gameObject.IsDontDestroyOnLoad())
                    DontDestroyOnLoad(this.gameObject);
            }
        }
        public virtual void OnInstantiated()
        {

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

                Destroy(Instance.gameObject);
                Instance = null;
            }

        }

        void OnApplicationQuit()
        {
            Instance = null;
            _lock = null;
        }

        protected void OnDestroy()
        {
            if (_lock == null)
                return;
            if (null != this.OnDestroyListener)
            {
                this.OnDestroyListener();
                this.OnDestroyListener = null;
            }

            if (Instance == this)
                Instance = null;

            //Debug.Log("Singleton object destroy");
        }
    }

}