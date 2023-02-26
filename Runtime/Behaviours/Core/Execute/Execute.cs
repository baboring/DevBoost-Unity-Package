/* *************************************************
*  Created:  2018-1-28 19:46:32
*  File:     Execute.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using UnityEngine;

namespace DevBoost.ActionBehaviour
{

    public class Execute : ActionNode
    {

        [SerializeField]
        protected ActionNode Node;

        [SerializeField]
        protected ActionState response = ActionState.None;

        protected override void OnReset()
        {
            base.OnReset();
            state = ActionState.None;
            Debug.Assert(Node != null, "Node is null : " + name);
        }

        protected override ActionState OnUpdate()
        {

            // parent update
            ActionState result = base.OnUpdate();
            if (result != ActionState.Success && result != ActionState.Running)
                return result;

            Debug.Assert(Node != null, "Node is null : " + name);
            Debug.Assert(Node != this, "Node is owner : " + name);
            if (Node == this || Node == null)
                return ActionState.Error;

            var res = Node.Execute();
            if (response == ActionState.None)
                state = res;
            else
                state = response;
            return state;
        }

        [NaughtyAttributes.Button("Test Run")]
        private void DebugRun()
        {
            ExecuteInvoke();
        }

        /// <summary>
        /// Get or add an event on Destroy event
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Execute AddTo<T>(Component obj) where T : Execute
        {
            return obj.GetOrAddComponent<T>();
        }
    }
}
