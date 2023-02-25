/* *************************************************
*  Created:  2018-1-28 20:15:39
*  File:     DisableBehaviourAction.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/


using UnityEngine;

namespace DevBoost.ActionBehaviour {
	
    using NaughtyAttributes;

	public class DisableBehaviourAction : ActionNode {

        [ReorderableList]
		[SerializeField]
		protected ActionNode[] targets;

        protected override ActionState OnUpdate() {

			// parent update
			ActionState result = base.OnUpdate();
			if(result != ActionState.Success)
				return result;

			for( int i=0;i < targets.Length; ++i )
				targets[i].enabled = false;

			return ActionState.Success;
		}
	}

}