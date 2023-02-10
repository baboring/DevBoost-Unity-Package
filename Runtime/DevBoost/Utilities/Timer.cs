using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace DevBoost.Utilities {


	public class Timer : SingletonMono<Timer> {

        private const float intervalTick = 0.016f;
        readonly int interval = (int)(intervalTick * 1000f);
		/// <summary>
		/// Timer 
		/// </summary>
		void decreaseTimeRemaining() 
        {
			foreach (var t in lstIimer)
				t.Elapsed(interval);
			lstIimer.RemoveAll(item => item.IsTimeOver || item.isCanceled);
		}

		protected new void Awake() 
        {
            base.Awake();
			InvokeRepeating("decreaseTimeRemaining", 0, intervalTick);
		}

		class TIMER 
        {
			public int id;
			public Action<int> onListener;
			public int remainTime;
			public int interval;
			public int repeatCount = 1;
            public bool isCanceled;

			public bool IsTimeOver { get { return (remainTime <= 0); } }
			public bool Elapsed(int time) {

                if (isCanceled)
                    return false;
				remainTime -= time;
				//Debug.Log(string.Format("time {0} {1}",id,remainTime));
				if (IsTimeOver) {
					onListener(id);
					if (repeatCount > 0) --repeatCount;
					if (repeatCount == 0 || interval == 0)
					{
						onListener = null;
						return true;
					}
					while(remainTime <= 0)
						remainTime += interval;
				}
				return false;
			}
		}
		List<TIMER> lstIimer = new List<TIMER>();

		static int time_uid = 0;
		public static int SetTimer(int msElapse, Action<int> listener, int repeat = -1) {

			if (Instance == null)
				SafeInstance.Initialize(SingletonType.DontDestroyOnLoad);

			SafeInstance.lstIimer.Add(new TIMER() {
				id = ++time_uid,
				remainTime = msElapse,
				interval = msElapse,
				repeatCount = repeat,
				onListener = listener
			});

			return time_uid;
		}

		// cancel timer
		public static void CancelTimer(int id) {
			int idx = SafeInstance.lstIimer.FindIndex(va => va.id == id);
            if (idx > -1)
                SafeInstance.lstIimer[idx].isCanceled = true;
		}

        public static Coroutine WaitTime(float time, Action callback)
        {
            return SafeInstance.StartCoroutine(SafeInstance.WaitRoutine(time, callback));
        }
        public static Coroutine WaitTime(MonoBehaviour mono, float time, Action callback)
        {
            return (mono ?? SafeInstance).StartCoroutine(SafeInstance.WaitRoutine(time, callback));
        }

        public static Coroutine WaitUntil(Func<bool> condition, Action callback)
        {
            return SafeInstance.StartCoroutine(SafeInstance.WaitRoutine(condition, callback));
        }
        public static Coroutine WaitUntil(MonoBehaviour mono, Func<bool> condition, Action callback)
        {
            return (mono ?? SafeInstance).StartCoroutine(SafeInstance.WaitRoutine(condition, callback));
        }

        public static Coroutine WaitEndOfFrame(Action callback)
        {
            return SafeInstance.StartCoroutine(SafeInstance.WaitEndOfFrameRoutine(callback));
        }
        public static Coroutine WaitEndOfFrame(MonoBehaviour mono, Action callback)
        {
            return (mono ?? SafeInstance).StartCoroutine(SafeInstance.WaitEndOfFrameRoutine(callback));
        }

        public static Coroutine DoNextFrame(Action callback)
        {
            return SafeInstance.StartCoroutine(SafeInstance.WaitRoutine(callback));
        }
        public static Coroutine DoNextFrame(MonoBehaviour mono, Action callback)
        {
            return (mono ?? SafeInstance).StartCoroutine(SafeInstance.WaitRoutine(callback));
        }

        public static Coroutine DoWhile(float during, Action<float> loop)
        {
            return SafeInstance.StartCoroutine(SafeInstance.WhileRoutine(during, 0, loop));
        }
        public static Coroutine DoWhile(MonoBehaviour mono, float during, Action<float> loop)
        {
            return (mono ?? SafeInstance).StartCoroutine(SafeInstance.WhileRoutine(during, 0, loop));
        }

        public static Coroutine DoWhile(float during, float interval, Action<float> loop)
        {
            return SafeInstance.StartCoroutine(SafeInstance.WhileRoutine(during, interval, loop));
        }
        public static Coroutine DoWhile(MonoBehaviour mono, float during, float interval, Action<float> loop)
        {
            return (mono ?? SafeInstance).StartCoroutine(SafeInstance.WhileRoutine(during, interval, loop));
        }

        private IEnumerator WaitEndOfFrameRoutine(Action callback)
        {
            yield return new WaitForEndOfFrame();
            callback?.Invoke();
        }
        private IEnumerator WaitRoutine(Action callback)
        {
            yield return null;
            callback?.Invoke();
        }

        private IEnumerator WaitRoutine(float time, Action callback)
        {
            yield return new WaitForSeconds(time);
            callback?.Invoke();
        }
        private IEnumerator WaitRoutine(Func<bool> condition, Action callback)
        {
            while (condition != null && !condition.Invoke())
                yield return null;

            callback?.Invoke();
        }
        private IEnumerator WhileRoutine(float during, float interval, Action<float> loop)
        {
            float elpased = 0;
            loop(elpased);
            while ( elpased < during)
            {
                if (interval == 0)
                    yield return null;
                else
                    yield return new WaitForSeconds(interval);
                elpased += Time.deltaTime;
                loop(Mathf.Min(elpased, during));
            }
        }
    }
}