
/* *************************************************
*  Created:  2018-1-28 20:15:39
*  File:     Comment.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using UnityEngine;

namespace DevBoost.ActionBehaviour {

	public class Comment : ActionNode {

		[SerializeField]
		protected string logText;

        protected override ActionState OnUpdate() {

			// parent update
			ActionState result = base.OnUpdate();
			if(result != ActionState.Success)
				return result;
            if(logText.Length > 0)
                Debug.Log(logText);
			return ActionState.Success;

		}
	}

}