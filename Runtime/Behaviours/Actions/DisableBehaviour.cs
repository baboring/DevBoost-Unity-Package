/* *************************************************
*  Created:  2018-1-28 20:15:39
*  File:     DisableBehaviour.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevBoost.ActionBehaviour {
	
    using NaughtyAttributes;

	public class DisableBehaviour : ActionNode {

        [ReorderableList]
		[SerializeField]
		protected Behaviour[] targets;

        protected override ActionState OnUpdate() {

			// parent update
			ActionState result = base.OnUpdate();
			if(result != ActionState.Success)
				return result;

			for( int i=0;i < targets.Length; ++i )
				if (targets[i] != null)
                    targets[i].enabled = false;

			return ActionState.Success;
		}
	}

}