/* *************************************************
*  Created:  2018-1-28 19:51:59
*  File:     ActionNode.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/
using System;
using UnityEngine;

namespace DevBoost.ActionBehaviour {

    abstract public class ActionNode<T> : ActionNode where T : class
    {
        sealed public override Type agentType { get { return typeof(T); } }
        new public T agent { get { return base.agent as T; } }
    }

    // Action Node class
    public class ActionNode : BaseNode {

		public enum LogMode { All, JustErrors };
		static LogMode m_LogMode = LogMode.All;

        protected override ActionState OnUpdate() {
            if (state != ActionState.None)
                return state;

			return ActionState.Success;
		}

        [System.Diagnostics.Conditional("FILE_LOG")]
		protected static void Log(LogType logType, string text)
		{
			if (logType == LogType.Error)
                Debug.LogError("[ActionNode] " + text);
			else if (m_LogMode == LogMode.All)
                Debug.Log("[ActionNode] " + text);
		}
		
	}
}