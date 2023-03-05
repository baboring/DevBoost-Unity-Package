
/* *************************************************
*  Created:  2018-3-31 20:15:39
*  File:     DontDestroyObject.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevBoost.ActionBehaviour {

    using NaughtyAttributes;

	public class DontDestroyObject : ActionNodeVarTarget
	{
        protected override void OnReset()
        {
            base.OnReset();

			if (targetObject == null)
				targetObject = this.gameObject;
		}

        protected override ActionState OnUpdate() {

			// parent update
			ActionState result = base.OnUpdate();
			if(result != ActionState.Success)
				return result;

			if (target != null)
                DontDestroyOnLoad(target);
            
			return ActionState.Success;
		}
	}

}