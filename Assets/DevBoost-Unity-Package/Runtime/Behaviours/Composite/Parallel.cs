/* *************************************************
*  Created:  2018-1-28 19:46:46
*  File:     SequenceNode.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using System.Collections;
using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;

namespace ActionBehaviour
{

    public class Parallel : ActionNode
    {

        [ReorderableList]
        [SerializeField]
        protected ActionNode[] childNodes;

        AsyncProcessing async = null;

        public IEnumerable<ActionNode> Children => childNodes.Cast<ActionNode>();

        protected override void OnReset()
        {
            base.OnReset();

            if (null != async)
            {
                if (null != async.backgroundWorking)
                    StopCoroutine(async.backgroundWorking);
                async = null;
            }

            // reset children
            ResetChildren();
        }

        // reset children
        protected void ResetChildren()
        {

            if (childNodes != null && childNodes.Length > 0)
            {
                foreach (var node in childNodes)
                    if (node != null)
                        node.Reset();
            }
        }


        protected override ActionState OnUpdate()
        {

            if (null == async || !async.isDone)
            {
                ActionState result = base.OnUpdate();
                if (result != ActionState.Success && result != ActionState.Running)
                    return result;
            }

            state = this.UpdateParallel();
            if (ActionState.Running == state && null == async)
            {
                async = new AsyncProcessing(StartCoroutine(CoUpdateParallel()));
                async.onCompleted += OnSuccess;
            }

            return state;
        }

        // Coroutine for updating sequence multi
        IEnumerator CoUpdateParallel()
        {
            // performs to update Sequence
            while (true)
            {
                state = this.UpdateParallel();
                if (ActionState.Running != state)
                    break;
                yield return null;
            }
            async.isDone = true;
        }

        // inner update sequnce
        ActionState UpdateParallel()
        {
            int index = 0;
            ActionState result = ActionState.Success;
            while (index < childNodes.Length)
            {
                // exception infinite loop
                Debug.Assert(childNodes[index] != this, "child node is ownself!! " + this.name);
                if (childNodes[index] == this)
                    result =  ActionState.Error;

                var temp = childNodes[index].Execute(false);
                if (ActionState.Success != temp && result == ActionState.Success)
                    result = temp;
                ++index;
            }

            return result;
        }

    }


}