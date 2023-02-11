/* ---------------------------------------------------------------------
 * Created on Mon Aug 19 2021 3:33:10 PM
 * Author : Benjamin Park
 * Description : Action node container
--------------------------------------------------------------------- */

using System.Collections.Generic;
using UnityEngine;

namespace DevBoost.ActionBehaviour
{
    public class ActionNodeContainer : ActionNode
    {
        [System.Serializable]
        public class Chunk
        {
            public string label;
            public ActionNode actionNode;
        }

        [SerializeField, NaughtyAttributes.ReorderableList]
        private List<Chunk> actionList = new List<Chunk>();

        public void Clear()
        {
            actionList.Clear();
        }

        /// <summary>
        /// Get Action Node by label
        /// </summary>
        /// <param label="label"></param>
        /// <returns></returns>
        public ActionNode GetNode(string label)
        {
            foreach (var chunk in actionList)
            {
                if (chunk.label == label)
                    return chunk.actionNode;
            }
            return null;
        }

        /// <summary>
        /// Get Action Node by label
        /// </summary>
        /// <param label="label"></param>
        /// <returns></returns>
        public T GetNode<T>(string label) where T : ActionNode
        {
            foreach (var chunk in actionList)
            {
                if (chunk.label == label)
                    return chunk.actionNode as T;
            }
            return null;
        }


        /// <summary>
        /// Copy from other container
        /// </summary>
        /// <param name="other"></param>
        public void Copy(ActionNodeContainer other)
        {
            if (other != null)
            {
                Clear();

                foreach (var chunk in other.actionList)
                    AddActionNode(chunk.label, chunk.actionNode);
            }
        }

        /// <summary>
        /// Add action node manually
        /// </summary>
        /// <param name="label"></param>
        /// <param name="node"></param>
        public void AddActionNode(string label, ActionNode node)
        {
            actionList.Add(new Chunk() { label = label, actionNode = node });
        }

    }

}