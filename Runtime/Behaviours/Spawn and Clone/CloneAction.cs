/* ---------------------------------------------------------------------
 * Created on Mon Aug 19 2021 3:33:10 PM
 * Author : Benjamin Park
 * Description : Clone object
--------------------------------------------------------------------- */

using NaughtyAttributes;
using UnityEngine;

namespace DevBoost.ActionBehaviour
{
    public class CloneAction : ActionNode
    {
        [SerializeField]
        private GameObject target;

        [SerializeField]
        private float spreadMin = 0;
        [SerializeField]
        private float spreadMax = 1;

        [SerializeField]
        private Transform context;

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

            cloned = Instantiate(target, context) as GameObject;

            cloned.transform.localScale = Vector3.one;
            cloned.transform.localPosition = (Random.insideUnitCircle * Random.Range(spreadMin, spreadMax));
            cloned.SetActive(true);

            if (setAsValue)
                clonedObjectVar?.SetValue(cloned);

            return result;

        }



    }

}