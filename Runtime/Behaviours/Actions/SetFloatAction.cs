
/* *************************************************
*  Created:  2021-2-16 20:15:39
*  File:     LocalScale.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using UnityEngine;

namespace DevBoost.ActionBehaviour {

	public class SetFloatAction : ActionNode {

		[SerializeField] private FloatVar src;
		[SerializeField] private FloatVar dest;

		protected override ActionState OnUpdate() {

			// parent update
			ActionState result = base.OnUpdate();
			if(result != ActionState.Success)
				return result;

			dest.SetValue(src.Value);

			return ActionState.Success;
		}
	}

}