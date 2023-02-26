/* *************************************************
*  Created:  2023-2-25 14:51:00
*  File:     ActionNotifier.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevBoost.ActionScript {

	using DevBoost.Utilities;
	public class ActionNotifier : Singleton<ActionNotifier>, ISingleton
	{
		protected Dictionary<Object, Operation> events = new Dictionary<Object, Operation>();

		public override void OnInstantiated()
		{
			base.OnInstantiated();
		}

		// register
		static public bool Register(Object obj, ActionHandler handler) {
			Operation op;
			if(!Instance.events.TryGetValue(obj, out op)) {
				op = new Operation();
				Instance.events.Add(obj,op);
			}
			op.handler += handler;
			return true;
		}

		// Unregister
		static public bool Unregister(Object obj, ActionHandler handler) {
			Operation op;
			if(!Instance.events.TryGetValue(obj, out op))
				return false;
			op.handler -= handler;
			return true;
		}


		static public bool Invoke(Object obj, object arg) {
			Operation op;
			if(!Instance.events.TryGetValue(obj, out op)) 
				return false;
			op.Action(arg);
			return true;			
		}

    }

	public delegate void ActionHandler(object arg);  
  
    public class Operation  
    {  
        public event ActionHandler handler;  
  
        public void Action(object arg)  
        {  
            if (handler != null)  
            {  
                handler(arg);  
                Debug.Log(arg);   
            }  
            else  
            {  
                Debug.Log("Not Registered");   
            }  
        }  
    }  
		
}