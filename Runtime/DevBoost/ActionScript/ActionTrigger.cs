/* *************************************************
*  Created:  2023-2-25 14:51:00
*  File:     ActionTrigger.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using UnityEngine;
using System;


namespace DevBoost.ActionScript {

	public abstract class ActionTrigger : MonoBehaviour {

		[SerializeField]
		protected ActionObject actObj;

		// condition
		protected Predicate<Variable> match = null;
		protected Action<ActionObject> action = null;

		public string Name;
		// Use this for initialization
		public void Initialize() {
			if(null != actObj)
				actObj.Initialize(this.gameObject);
		}
		
		// Update is called once per frame
		public void Tick() {
			if( null == match)
				return;
			if(match(actObj.Value))
            {
				actObj.TriggerEvent();
				action?.Invoke(actObj);
			}
		}
	}

}
