/* ---------------------------------------------------------------------
 * Created on Mon Aug 19 2022 3:33:10 PM
 * Author : Benjamin Park
 * Description : Instantiate GameObject
--------------------------------------------------------------------- */

using NaughtyAttributes;
using UnityEngine;

namespace DevBoost.ActionBehaviour
{
    public class InstantiateGameObjectAction : ActionNodeVarTarget
    {
        [SerializeField]
        private Transform parent;

        [SerializeField]
        protected bool localScaleOne;
        [SerializeField]
        protected bool localPosZero;

        [SerializeField]
        protected bool setAsValue;

        [SerializeField, ShowIf("setAsValue")]
        private GameObjectVar clonedObjectVar;



        public GameObject cloned { get; private set; }

        protected override ActionState OnUpdate()
        {
            // parent update
            ActionState result = base.OnUpdate();
            if (result != ActionState.Success && result != ActionState.Running)
                return result;

            cloned = Instantiate(target, parent) as GameObject;

            if (localScaleOne)
                cloned.transform.localScale = Vector3.one;
            if (localPosZero)
                cloned.transform.localPosition = Vector3.zero;

            if (setAsValue)
                clonedObjectVar?.SetValue(cloned);

            cloned.SetActive(true);

            return result;

        }



    }

}