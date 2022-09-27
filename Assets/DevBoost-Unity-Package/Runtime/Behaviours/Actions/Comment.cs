
/* *************************************************
*  Created:  2018-1-28 20:15:39
*  File:     ActionNode.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActionBehaviour {

    using DevBoost.Utilities;

	public class Comment : ActionNode {

		[SerializeField]
		protected string logText;


        protected override ActionState OnUpdate() {

			// parent update
			ActionState result = base.OnUpdate();
			if(result != ActionState.Success)
				return result;
            if(logText.Length > 0)
                Log.Trace(logText);
			return ActionState.Success;

		}
	}

}