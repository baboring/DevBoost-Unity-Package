/* *************************************************
*  Created:  2018-1-28 19:46:32
*  File:     ExecuteNodes.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using UnityEngine;

namespace DevBoost.ActionBehaviour {

	public class ExecuteNodes : ActionNode {

		[SerializeField,NaughtyAttributes.ReorderableList]
		protected ActionNode[] Nodes;

        protected override void OnReset()
        {
            base.OnReset();
            state = ActionState.None;
            Debug.Assert(Nodes != null, "Node is null:");
        }

        protected override ActionState OnUpdate() {

            // parent update
            ActionState result = base.OnUpdate();
            if (result != ActionState.Success && result != ActionState.Running)
                return result;

            foreach(var item in Nodes)
            {
                Debug.Assert(item != null, "Node is null:");
                Debug.Assert(item != this, "Node is owner:");
                if (item == this || item == null)
                    return ActionState.Error;

                result = item.Execute();
            }

            return state;
		}

        [NaughtyAttributes.Button("Test Run")]
        private void DebugRun()
        {
            ExecuteInvoke();
        }
    }
}
