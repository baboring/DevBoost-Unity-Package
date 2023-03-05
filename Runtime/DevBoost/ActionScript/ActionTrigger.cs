/* *************************************************
*  Created:  2023-2-25 14:51:00
*  File:     ActionTrigger.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using UnityEngine;
using System;


namespace DevBoost.ActionScript 
{

	public class ActionTrigger : BaseActionTrigger
	{

		// condition
		protected Predicate<Variable> match = null;

		// Use this for initialization
		public void Initialize()
		{
			if (null != actObj)
				actObj.Initialize(this.gameObject);
		}
		public void Tick()
		{
			Debug.Assert(actObj != null);
			Debug.Assert(match != null);
			if (null == match || actObj == null)
				return;
			if (match(actObj.Value))
			{
				actObj.TriggerEvent();
				InvokeAction();
			}
		}

	}

	/// <summary>
	/// Base Tigger
	/// </summary>
	public abstract class BaseActionTrigger : MonoBehaviour
	{
		public string Name;

		[SerializeField]
		protected ActionObject actObj;

		protected Action<ActionObject> action = null;

		public void SetListener(Action<ActionObject> callback)
		{
			action = callback;
		}

		public void AddListener(Action<ActionObject> callback)
		{
			action += callback;
		}
		public void RemoveListener(Action<ActionObject> callback)
		{
			action -= callback;
		}

		public void RemoveAllListener()
		{
			action = null;
		}

		protected void InvokeAction()
		{
			action?.Invoke(actObj);
		}
	}


}
