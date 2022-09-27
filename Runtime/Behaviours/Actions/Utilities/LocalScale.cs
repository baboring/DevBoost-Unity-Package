
/* *************************************************
*  Created:  2021-2-16 20:15:39
*  File:     LocalScale.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevBoost.ActionBehaviour {

	public class LocalScale : ActionNode {

		[SerializeField] private Vector3 newScale = Vector3.one;

        protected override ActionState OnUpdate() {

			// parent update
			ActionState result = base.OnUpdate();
			if(result != ActionState.Success)
				return result;

			transform.localScale = newScale;

			return ActionState.Success;
		}
	}

}