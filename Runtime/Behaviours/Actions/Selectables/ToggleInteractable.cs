
/* *************************************************
*  Created:  2022-9-26 20:15:39
*  File:     ToggleInteractable.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using UnityEngine;

namespace DevBoost.ActionBehaviour
{
    using NaughtyAttributes;
    using UnityEngine.UI;

    public class ToggleInteractable : ActionNode {

        [ReorderableList]
		[SerializeField]
		protected Selectable[] Selectables;

        [SerializeField]
        protected bool force;
        [SerializeField,ShowIf("force")]
        protected bool value;

        /// <summary>
        /// Toggle Objects
        /// </summary>
        public void Toggle()
        {
            for (int i = 0; i < Selectables.Length; ++i)
            {
                Debug.Assert(Selectables[i] != null, gameObject.name + " has an error nodes !!");
                if (Selectables[i] != null)
                {
                    if (force)
                        Selectables[i].interactable = value;
                    else
                        Selectables[i].interactable = !Selectables[i].interactable;
                }
            }
        }

        protected override ActionState OnUpdate() {

			// parent update
			ActionState result = base.OnUpdate();
			if(result != ActionState.Success)
				return result;

            Toggle();

			return ActionState.Success;
		}
	}

}