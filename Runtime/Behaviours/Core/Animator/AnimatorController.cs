/* *************************************************
*  Created:  2018-1-28 19:51:59
*  File:     AbnimatorController.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DevBoost.Extensions;
using NaughtyAttributes;

namespace DevBoost.ActionBehaviour
{       
    /// <summary>
    /// Used to handle character animations 
    /// </summary> 
	public class AnimatorController : MonoBehaviour
    {
        /// <summary>
        /// Animator instance
        /// </summary>   
        [SerializeField]
        private Animator m_Animator = null;

        /// <summary>
        /// List of animation events registered
        /// </summary>
        [SerializeField, ReorderableList]
        private AnimationEvent[] m_AnimationEvents = null;

        // used it for puase and result
        float? m_LastAniSpeed;

        /// <summary>
        /// Hash set of registered events
        /// </summary>
        private Dictionary<string, AnimationEvent> m_RegisteredEvents = null;

        public System.Action<string> onEventRunAnimation = null;

        /// <summary>
        /// Get instance of animator
        /// </summary>
        public Animator Animator
        {
            get
            {
                if (m_Animator == null)
                    m_Animator = GetComponent<Animator>();
                return m_Animator;
            }
        }

        // speedß
        public float Speed {
            get { return m_Animator.speed; }
            set {
                m_Animator.speed = value;
                m_LastAniSpeed = null;
            }
        }

        public bool IsPasue
        {
            get
            {
                return m_LastAniSpeed != null;
            }
            set
            {
                if (value)
                    Pause();
                else
                    Resume();
            }
        }
        // initialize animator
        private void Awake()
        {
            if(m_Animator == null)
                m_Animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Initailize this animator controller
        /// </summary>
        protected virtual void Start()
        {
            //------
            // Register animation events if found
            // -----
            RegisterEvent(m_AnimationEvents);
        }



        private void RegisterEvent(AnimationEvent[] events)
        {
            if (events != null)
            {
                foreach (var info in events)
                    RegisterEvent(info);
            }
        }

        // Register Event
        public bool RegisterEvent(AnimationEvent animEvent)
        {
            if(null == m_RegisteredEvents)
                m_RegisteredEvents = new Dictionary<string, AnimationEvent>();

            if(m_RegisteredEvents.ContainsKey(animEvent.animationEventId))
                return false;
            m_RegisteredEvents.Add(animEvent.animationEventId, animEvent);
            return true;
        }

        /// <summary>
        /// Merges the event.
        /// </summary>
        /// <returns><c>true</c>, if event was merged, <c>false</c> otherwise.</returns>
        /// <param name="key">Key.</param>
        /// <param name="action">Action.</param>
        public bool MergeEvent(string key, System.Action action)
        {
            if (null == m_RegisteredEvents)
                m_RegisteredEvents = new Dictionary<string, AnimationEvent>();

            if (!m_RegisteredEvents.ContainsKey(key))
            {
                RegisterEvent(new AnimationEvent(key, action));
                return true;
            }
            m_RegisteredEvents[key].AddEvent(action);
            return true;
        }

        /// <summary>
        /// Set animator toggle
        /// </summary>        
        public virtual void SetTrigger(string triggrName)
        {
            Debug.Assert(!string.IsNullOrEmpty(triggrName), "triggrName is null");
            Debug.Assert(null != m_Animator, "m_Animator is null");
            m_Animator.SetTrigger(triggrName);
        }

        /// <summary>
        /// Reset trigger
        /// </summary>      
        public virtual void ResetTrigger(string triggerName)
        {
            Debug.Assert(null != m_Animator, "m_Animator is null");
            if (null != m_Animator)
                m_Animator.ResetTrigger(triggerName);
        }

        /// <summary>
        /// Set bool property in animator
        /// </summary>      
        public virtual void SetBool(string propertyName, bool value)
        {
            Debug.Assert(null != m_Animator, "m_Animator is null");
            if (null != m_Animator)
                m_Animator.SetBool(propertyName, value);
        }

        /// <summary>
        /// Set int property in animator
        /// </summary>  
        public virtual void SetInt(string propertyName, int value)
        {
            Debug.Assert(null != m_Animator, "m_Animator is null");
            if (null != m_Animator)
                m_Animator.SetInteger(propertyName, value);
        }
        
        /// <summary>
        /// Get the int value of a property
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public int GetInt(string propertyName)
        {
            Debug.Assert(null != m_Animator, "m_Animator is null");
            return m_Animator.GetInteger(propertyName);
        }

        /// <summary>
        /// Set float property in animator
        /// </summary>  
        public virtual void SetFloat(string propertyName, float value)
        {
            Debug.Assert(null != m_Animator, "m_Animator is null");
            if (null != m_Animator)
                m_Animator.SetFloat(propertyName, value);
        }

        public virtual void SetFloat(string propertyName, float value, float dampTime, float deltaTime)
        {
            Debug.Assert(null != m_Animator, "m_Animator is null");
            if (m_Animator != null)
                m_Animator.SetFloat(propertyName, value, dampTime, deltaTime);          
        }

        public float GetFloat(string propertyName)
        {
            Debug.Assert(null != m_Animator, "m_Animator is null");
            return m_Animator.GetFloat(propertyName);
        }

        // Get whether this state is in the current state 
        public bool IsState(int layerIndex, string stateName)
        {
            AnimatorStateInfo info = m_Animator.GetCurrentAnimatorStateInfo(layerIndex);
            return info.IsName(stateName);
        }

        public float GetCurrentTime(int layerIndex = 0)
        {
            AnimatorStateInfo info = m_Animator.GetCurrentAnimatorStateInfo(layerIndex);
            return info.normalizedTime;
        }

        /// <summary>
        /// Run animation event with provided id
        /// </summary>
        public virtual void RunAnimationEvent(string animationEventId)
        {
            // validate animation event
            if (m_RegisteredEvents != null)
                return;
            if (m_RegisteredEvents.TryGetValue(animationEventId, out var aniEvent))
                ExecuteEvent(aniEvent);
        }

        private void ExecuteEvent(AnimationEvent evt)
        {
            if (null != evt)
            {
                evt.RunEvent();

                // notify animation event
                if (null != onEventRunAnimation)
                    onEventRunAnimation(evt.animationEventId);
            }
        }



        public void Pause()
        {
            if (m_LastAniSpeed == null)
                m_LastAniSpeed = m_Animator.speed;

            m_Animator.speed = 0;
        }

        public void Resume()
        {
            if(m_LastAniSpeed != null)
            {
                Speed = (float)m_LastAniSpeed;
            }
        }

        // Reset All Parameters (Clear all)
        public void ResetAllParameters()
        {
            var animator = m_Animator;
            if (null == animator)
                return;
            animator.ResetAllParameters();
        }

    }

    /// <summary>
    /// Class used to define an animation event
    /// </summary>
    [System.Serializable]
    public class AnimationEvent
    {
        /// <summary>
        /// Id used to identify this animation event
        /// </summary>
        [SerializeField]
        public string animationEventId;

        [SerializeField]
        public UnityEvent animationEvents = null;

        private System.Action m_OnEvent = null;

        public int hash { get; set; }

        public AnimationEvent(string id, System.Action action = null)
        {
            animationEventId = id;
            m_OnEvent = action;
        }

        public void AddEvent(System.Action action)
        {
            m_OnEvent += action;
        }
        public void RemoveEvent(System.Action action)
        {
            m_OnEvent -= action;
        }
        /// <summary>
        /// Get wether this event has provided eventId
        /// </summary>
        public bool HasId(string eventId)
        {
            return eventId == animationEventId;
        }

        /// <summary>
        /// Used to fire animation event
        /// </summary>
        public virtual void RunEvent()
        {
            if (null != m_OnEvent)
                m_OnEvent();

            animationEvents?.Invoke();
            //else if(m_OnEvent == null)
            //{
            //    Debug.LogWarning("No Animation Event was provided for '" + animationEventId +"'");
            //}
        }
    }
}
