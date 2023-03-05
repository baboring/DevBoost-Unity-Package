/* *************************************************
*  Created:  2023-3-05 22:46:32
*  File:     ActionUnityEvent.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using UnityEngine;
using UnityEngine.Events;

namespace DevBoost.ActionBehaviour {

	public class ActionUnityEvent : ActionNode
	{
		[SerializeField]
		protected UnityEvent events = new UnityEvent();

		protected override ActionState OnUpdate()
		{

			// parent update
			ActionState result = base.OnUpdate();
			if (result != ActionState.Success)
				return result;

			events.Invoke();

			return ActionState.Success;
		}
	}
}
