/* *************************************************
*  Created:  2018-1-28 19:51:59
*  File:     ActionNode.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActionBehaviour {

	using DevBoost.Utilities;
    using NaughtyAttributes;

    abstract public class ActionNode<T> : ActionNode where T : class
    {
        sealed public override Type agentType { get { return typeof(T); } }
        new public T agent { get { return base.agent as T; } }
    }

    // Action Node class
    public class ActionNode : BaseNode {

		public enum LogMode { All, JustErrors };
		static LogMode m_LogMode = LogMode.All;
	

		[SerializeField]
		public string comments = null;

        [Button("Execute(Click)")]
        private void _RunInEdtior() {
            Execute();
        }

        protected override ActionState OnUpdate() {
            if (state != ActionState.None)
                return state;

            if(null != comments && comments.Length > 0)
                Log(LogType.Log, comments);

			return ActionState.Success;
		}

		private static void Log(LogType logType, string text)
		{
			if (logType == LogType.Error)
                Debug.LogError("[ActionNode] " + text);
			else if (m_LogMode == LogMode.All)
                Debug.Log("[ActionNode] " + text);
		}
		
	}
}