
/* *************************************************
*  Created:  2021-2-16 20:15:39
*  File:     TimeScale.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevBoost.ActionBehaviour {

	public class TimeScale : ActionNode {

		[SerializeField] private float timeScale = 1;

        protected override ActionState OnUpdate() {

			// parent update
			ActionState result = base.OnUpdate();
			if(result != ActionState.Success)
				return result;
			
			Time.timeScale = timeScale;

			return ActionState.Success;
		}

        [NaughtyAttributes.Button("Test Run")]
        private void DebugRun()
        {
            ExecuteInvoke();
        }
    }

}