/* *************************************************
*  Created:  2022-1-28 19:51:59
*  File:     LogNode.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/
#if FILE_LOG
#define SHOW_LOG_ACTION
#endif

using System.Diagnostics;
using UnityEngine;


namespace DevBoost.ActionBehaviour
{
    // Action Node class
    public class LogAction : ActionNode
    {
        [SerializeField]
        private bool logHide = false;
        [SerializeField]
        private bool isColor;
        [SerializeField]
        private Color color = Color.white;
        [SerializeField]
        private string logString;

        protected override ActionState OnUpdate()
        {
            // parent update
            ActionState result = base.OnUpdate();
            if (result != ActionState.Success && result != ActionState.Running)
                return result;

            if (!logHide)
                LogWrite();

            return ActionState.Success;
        }

        [Conditional("SHOW_LOG_ACTION")]
        private void LogWrite()
        {
            if (isColor)
                UnityEngine.Debug.Log(string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(color.r * 255f), (byte)(color.g * 255f), (byte)(color.b * 255f), logString));
            else
                UnityEngine.Debug.Log(logString);
        }

    }
}