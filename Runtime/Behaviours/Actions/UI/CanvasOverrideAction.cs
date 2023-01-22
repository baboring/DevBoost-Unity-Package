/* ---------------------------------------------------------------------
 * Created on Mon Aug 19 2021 3:33:10 PM
 * Author : Benjamin Park
 * Description : Canvas setup action
--------------------------------------------------------------------- */
using ActionBehaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevBoost.ActionBehaviour
{

    public class CanvasOverrideAction : ActionNode
    {
        [SerializeField]
        private Canvas canvas;

        [SerializeField]
        private bool overrideSorting;
        [SerializeField]
        private int sortingOrder;

        protected override ActionState OnUpdate()
        {
            // parent update
            ActionState result = base.OnUpdate();
            if (result != ActionState.Success && result != ActionState.Running)
                return result;
            if (canvas != null)
            {
                canvas.overrideSorting = overrideSorting;
                canvas.sortingOrder = sortingOrder;
            }
            else
                Debug.LogError("canvas is null : " + name);

            return result;

        }

    }

}