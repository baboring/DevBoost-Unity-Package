/* ---------------------------------------------------------------------
 * Created on Mon Aug 19 2021 3:33:10 PM
 * Author : Benjamin Park
 * Description : Set transform Sibling
--------------------------------------------------------------------- */

using UnityEngine;

namespace DevBoost.ActionBehaviour
{
    public class SetAsSiblingAction : ActionNodeVarTarget
    {
        public enum Option
        {
            First,
            Last,
            Custum,
        }

        [SerializeField]
        private Option option;
        [SerializeField]
        private int index;



        protected override void OnReset()
        {
            base.OnReset();

            if (!Application.isPlaying && targetObject.IsNull())
                targetObject = this.gameObject;
        }

        protected override ActionState OnUpdate()
        {
            // parent update
            ActionState result = base.OnUpdate();
            if (result != ActionState.Success && result != ActionState.Running)
                return result;

            if (option == Option.First)
                target.transform.SetAsFirstSibling();
            else if (option == Option.Last)
                target.transform.SetAsLastSibling();
            else if (option == Option.Custum)
                target.transform.SetSiblingIndex(index);

            return result;

        }



    }

}