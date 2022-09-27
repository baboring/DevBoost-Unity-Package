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

    public class Sequence : ActionNode
    {

        [ReorderableList]
        [SerializeField]
        protected ActionNode[] childNodes;

        private int m_index = 0;
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

            m_index = 0;

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

            state = this.UpdateSequence();
            if (ActionState.Running == state && null == async)
            {
                async = new AsyncProcessing(StartCoroutine(CoUpdateSequence()));
                async.onCompleted += OnSuccess;
            }

            return state;
        }

        // Coroutine for updating sequence multi
        IEnumerator CoUpdateSequence()
        {
            // performs to update Sequence
            while (m_index < childNodes.Length)
            {
                state = this.UpdateSequence();
                if (ActionState.Running != state)
                    break;
                yield return null;
            }
            async.isDone = true;
        }

        // inner update sequnce
        ActionState UpdateSequence()
        {

            ActionState result = ActionState.Success;
            while (m_index < childNodes.Length)
            {
                // exception infinite loop
                Debug.Assert(childNodes[m_index] != this, "child node is ownself!! " + this.name);
                if (childNodes[m_index] == this)
                    return ActionState.Error;

                result = childNodes[m_index]?.Execute(false) ?? ActionState.Error;
                if (ActionState.Success != result)
                    return result;
                // log..
                //ActionNode.Log(LogType.Log, $"{childNodes[m_index]} success!");
                ++m_index;
            }

            return result;
        }

    }


}