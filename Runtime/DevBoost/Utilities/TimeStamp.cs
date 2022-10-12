using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevBoost.Utilities
{
    /// <summary>
    /// Utility script used to save the time to compare to current and save time
    /// </summary>
	
    public class TimeStamp
    {
        public enum TimeMode
        {
            Scaled,
            Unscaled
        }

        // notification about updating elapsed time
        private System.Action<float> _OnUpdateTick;

        // the state whether stop timer or on going
        private bool m_isPaused = false;

        // the time recorded when the pause happened
        private float m_timePaused;
        private float m_timeLastTick;

        private TimeMode m_TimeMode = TimeMode.Scaled;


        public bool isDebug = false;

        // unscaled timer
        public bool IsUnscaledTime {
            get { return m_TimeMode == TimeMode.Unscaled; }
            set
            {
                m_TimeMode = value ? TimeMode.Unscaled : TimeMode.Scaled;
            }
        }



        private float CurrentSystemTime {
            get
            {
                return m_TimeMode == TimeMode.Unscaled? Time.unscaledTime : Time.time;
            }
        }

        // Pasue timer
        public bool IsPause {
            get { return m_isPaused; }

            set {
                if (isDebug)
                    Debug.Log("Pause:"+value + "," + CurrentSystemTime);
                if(value)
                {
                    m_timePaused = Elapsed;
                }
                m_isPaused = value;
            }
        }
        // the time to save the time
        public float Last { get; private set; }

        // the time to be elapsed
        public float Elapsed {
            get {
                if(IsPause)
                    return m_timePaused;
                return CurrentSystemTime - Last;
            }
            protected set { Last = CurrentSystemTime + value; }
        }

        // constructor
        public TimeStamp(bool isResetTimer = false, TimeMode mode = TimeMode.Scaled)
		{
            m_TimeMode = mode;
            if(isResetTimer)
                Reset(true);

            _OnUpdateTick = (elapsed) => { };
        }

        public TimeStamp(float time)
        {
            Set(time);
            _OnUpdateTick = (elapsed) => { };
        }

        // Clear or set state
        public void Reset(bool enable = true)
        {
            Last = CurrentSystemTime;
            IsPause = !enable;
            m_timeLastTick = CurrentSystemTime;
        }

        // Clear or set state
        public void Set(float givingTime)
        {
            this.Elapsed = givingTime;
            m_timeLastTick = CurrentSystemTime;
        }


        // Register tick event
        public void RegisterOnTickUpdate(System.Action<float> callback)
        {
            if(null == _OnUpdateTick)
                _OnUpdateTick = callback;
            else
                _OnUpdateTick += callback;
        }

        public bool IsElapsed(float time, bool reset = false)
        {
            if (IsPause)
                return false;
            if(Elapsed > time)
            {
                if (isDebug)
                    Debug.Log("Elapsed:"+ CurrentSystemTime);
                if (reset)
                    Reset();
                return true;
            }
            return false;
        }

        // Updating tick event 
        public void UpdateTick(float interval = 0)
        {
            // Checking 
            if (m_timeLastTick == 0)
                m_timeLastTick = CurrentSystemTime - interval;

            // fixed interval tick
            if (interval > 0) {
                float currentTime = CurrentSystemTime;
                while (m_timeLastTick + interval <= currentTime) {
                    _OnUpdateTick(interval);
                    m_timeLastTick += interval;
                }
            }
            else {
                _OnUpdateTick(CurrentSystemTime - m_timeLastTick);
                m_timeLastTick = CurrentSystemTime;
            }
        }
    }
}
