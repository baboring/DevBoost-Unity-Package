
/* *************************************************
*  Created:  2018-3-28 20:15:39
*  File:     DestroyGameObject.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevBoost.ActionBehaviour {

    using NaughtyAttributes;

	public class DestroyGameObjectVars : ActionNode {

        [ReorderableList]
		[SerializeField]
		protected GameObjectVar[] objects;

        protected override ActionState OnUpdate() {

			// parent update
			ActionState result = base.OnUpdate();
			if(result != ActionState.Success)
				return result;

            for (int i = 0; i < objects.Length; ++i)
                if (objects[i] != null)
                    GameObject.Destroy(objects[i].Value);
            return ActionState.Success;
		}
	}

}