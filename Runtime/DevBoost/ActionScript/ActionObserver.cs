/* *************************************************
*  Created:  2023-2-25 14:51:00
*  File:     ActionObserver.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using UnityEngine;

namespace DevBoost.ActionScript {

	using NaughtyAttributes;

	public class ActionObserver : MonoBehaviour {

		[ReorderableList]
		[SerializeField]
		protected ActionTrigger[] list;
		// Use this for initialization
		void Start () 
		{
			if ((list?.Length ?? 0) == 0)
				list = GetComponents<ActionTrigger>();

			foreach(var ev in list)
            {
				ev?.Initialize();
				Debug.Log(ev?.Name);
			}
		}
		
		// Update is called once per frame
		void Update () {
			foreach (var item in list)
				item.Tick();
		}
	}

}