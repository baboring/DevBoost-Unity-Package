
/* *************************************************
*  Created:  2022-1-28 20:15:39
*  File:     SetCanvasAction.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using UnityEngine;

namespace DevBoost.ActionBehaviour {

	public class SetCanvasAction : ActionNode {

		[SerializeField]
		public Canvas canvas;

		[SerializeField]
		private bool overrideSorting = false;
		[SerializeField]
		private int sortingOrder = 0;

		protected override ActionState OnUpdate() {

			// parent update
			ActionState result = base.OnUpdate();
			if(result != ActionState.Success)
				return result;

			canvas.overrideSorting = overrideSorting;
			canvas.sortingOrder = sortingOrder;
			return ActionState.Success;
		}

	}

}