/* ---------------------------------------------------------------------
 * Created on Mon Aug 19 2021 3:33:10 PM
 * Author : Benjamin Park
 * Description : Action vars container
--------------------------------------------------------------------- */

using System.Collections.Generic;
using UnityEngine;

namespace DevBoost.ActionBehaviour
{
    public class ActionVarsContainer : ActionNode
    {
        [System.Serializable]
        public class Chunk
        {
            public string label;
            public BaseVar variable;
        }

        [SerializeField, NaughtyAttributes.ReorderableList]
        private List<Chunk> actionList = new List<Chunk>();

        public void Clear()
        {
            actionList.Clear();
        }

        /// <summary>
        /// Get var node by label
        /// </summary>
        /// <param label="label"></param>
        /// <returns></returns>
        public BaseVar GetVar(string label)
        {
            foreach (var chunk in actionList)
            {
                if (chunk.label == label)
                    return chunk.variable;
            }
            return null;
        }

        /// <summary>
        /// Get var node by label
        /// </summary>
        /// <param label="label"></param>
        /// <returns></returns>
        public T GetVar<T>(string label) where T : BaseVar
        {
            foreach (var chunk in actionList)
            {
                if (chunk.label == label)
                    return chunk.variable as T;
            }
            return null;
        }


        /// <summary>
        /// Copy from other container
        /// </summary>
        /// <param name="other"></param>
        public void Copy(ActionVarsContainer other)
        {
            if (other != null)
            {
                Clear();

                foreach (var chunk in other.actionList)
                    AddVar(chunk.label, chunk.variable);
            }
        }

        /// <summary>
        /// Add var node manually
        /// </summary>
        /// <param name="label"></param>
        /// <param name="node"></param>
        public void AddVar(string label, BaseVar node)
        {
            actionList.Add(new Chunk() { label = label, variable = node });
        }

    }

}