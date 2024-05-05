/* *************************************************
*  Created:  2018-11-02 19:46:32
*  File:     Singleton For ActionNode.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using DevBoost.Utilities;
using UnityEngine;

namespace DevBoost.ActionBehaviour
{
    /// <summary>
    /// Action starter.
    /// </summary>
    public class SingletonActionNode<T> : ActionNode where T : SingletonActionNode<T>, ISingleton
    {

        [SerializeField] private SingletonType m_SingletonType = SingletonType.DestroyOnLoad;

        public static T Instance
        {
            get;
            protected set;
        }
        //callback called when it destroyed
        public System.Action OnDestroyListener;

        static object _lock = new object();

        // Returns the instance of the singleton
        public static T SafeInstance
        {
            get
            {
                lock (_lock)
                {
                    if (null == Instance)
                        return Instantiate();
                }
                return Instance;
            }
        }

        // Awake
        protected void Awake()
        {
            DevBoost.Utilities.Logger.Trace("Awake singleton AnctionNode : {0},{1}", typeof(T), m_SingletonType);
            if (null == Instance)
            {
                Instance = this as T;
                if (m_SingletonType == SingletonType.DontDestroyOnLoad)
                    DontDestroyOnLoad(this.gameObject);
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
                    Debug.LogError("FATAL! Cannot create an instance of " + typeof(T) + ".");
            }
            else
            {
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
                DevBoost.Utilities.Logger.Trace("[ Singleton - Instantiated ] {0}", typeof(T).ToString());
            }

            if (type == SingletonType.DontDestroyOnLoad)
            {
                m_SingletonType = type;
                DontDestroyOnLoad(this.gameObject);
            }
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

                Object.Destroy(Instance.gameObject);
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
