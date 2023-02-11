using UnityEngine;

namespace DevBoost.Effects
{

	[System.Serializable]
    public abstract class TweenBase 
    {
        public float time = 1;

        public EasingType tweenType;

        public float speed = 1;

        protected float lastTime = 0;

        public bool IsDone { get; private set; }

        protected abstract bool OnUpdateFrame(float elapsed);


        protected virtual void OnReset()
        {
            lastTime = 0;

            if (time == 0f)
                speed = 0;
            else
                speed = 1f / time;
        }


        /// <summary>
        /// Update base anmation frame
        /// </summary>
        /// <returns></returns>
        protected virtual void OnUpdate()
        {
            if (lastTime == 0)
                lastTime = Time.time;

            float elapsed = Mathf.Clamp01((Time.time - lastTime) * speed);

            if (!OnUpdateFrame(elapsed) || elapsed == 1f)
                IsDone = true;
        }

    }

}