/* *************************************************
*  Created:  2022-3-38 19:51:59
*  File:     ActionNodeVarTarget.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/
using NaughtyAttributes;
using UnityEngine;

namespace DevBoost.ActionBehaviour {


    // Action Node class with Var Target
    public class ActionNodeVarTarget : ActionNode {

        #region variable target
        [SerializeField]
        protected bool useObjectVar;
        [SerializeField, HideIf("useObjectVar")]
        protected GameObject targetObject;

        [SerializeField, ShowIf("useObjectVar")]
        protected GameObjectVar gameObjectVar;

        protected GameObject target => useObjectVar ? gameObjectVar?.Value : targetObject;

        #endregion

        /// <summary>
        /// Set Custom target
        /// </summary>
        /// <param name="obj"></param>
        public void SetTargetObject(GameObject obj)
        {
            if (gameObjectVar != null)
                gameObjectVar.Value = obj;

            targetObject = obj;
        }


    }
}