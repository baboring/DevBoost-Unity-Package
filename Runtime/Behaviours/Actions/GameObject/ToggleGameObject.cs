
/* *************************************************
*  Created:  2018-1-28 20:15:39
*  File:     ToggleGameObject.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevBoost.ActionBehaviour
{
    using NaughtyAttributes;

	public class ToggleGameObject : ActionNode {

        [ReorderableList]
		[SerializeField]
		protected GameObject[] objects;

        /// <summary>
        /// Toggle Objects
        /// </summary>
        public void Toggle()
        {
            for (int i = 0; i < objects.Length; ++i)
            {
                Debug.Assert(objects[i] != null, gameObject.name + " has an error nodes !!");
                if (objects[i] != null)
                    objects[i].SetActive(!objects[i].activeSelf);
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