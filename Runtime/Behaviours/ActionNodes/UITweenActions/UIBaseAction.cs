/* ---------------------------------------------------------------------
 * Created on Mon Aug 19 2021 3:33:10 PM
 * Author : Benjamin Park
 * Description : Making scale animation
--------------------------------------------------------------------- */
using DevBoost.Effects;
using NaughtyAttributes;
using UnityEngine;

namespace DevBoost.ActionBehaviour
{

    public abstract class UIBaseAction : ActionNode
    {
        [SerializeField]
        protected bool useObjectVar;
        [SerializeField,HideIf("useObjectVar")]
        private GameObject targetObject;

        [SerializeField,ShowIf("useObjectVar")]
        private GameObjectVar gameObjectVar;

        [SerializeField]
        protected float time = 1;

        [SerializeField]
        protected EasingType tweenType;

        protected float speed = 1;
        protected float lastTime = 0;


        protected GameObject target => (gameObjectVar != null && gameObjectVar.Value != null) ? gameObjectVar.Value : targetObject;



        protected abstract bool DoUpdateFrame(float elapsed);


        protected override void OnReset()
        {
            base.OnReset();

            lastTime = 0;

            if (!Application.isPlaying && targetObject.IsNull())
                targetObject = this.gameObject;
            if (time == 0f)
                speed = 0;
            else
                speed = 1f / time;
        }

        /// <summary>
        /// Update base anmation frame
        /// </summary>
        /// <returns></returns>
        protected override ActionState OnUpdate()
        {
            // parent update
            ActionState result = base.OnUpdate();
            if (result != ActionState.Success && result != ActionState.Running)
                return result;

            if (lastTime == 0)
                lastTime = Time.time;
            float elapsed = Mathf.Clamp01((Time.time - lastTime) * speed);

            if (!DoUpdateFrame(elapsed) || elapsed == 1f)
                return ActionState.Success;

            return ActionState.Running;

        }


        /// <summary>
        /// Set Custom target
        /// </summary>
        /// <param name="obj"></param>
        public void SetTargetObject(GameObject obj)
        {
            if (gameObjectVar != null)
                gameObjectVar.Value = obj;
            else
                targetObject = obj;
        }
    }

}