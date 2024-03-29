﻿/* *************************************************
*  Created:  2023-2-25 14:51:00
*  File:     ActionObject.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/
using System.Collections;
using UnityEngine;

namespace DevBoost.ActionScript {

	public abstract class ActionObject : ScriptableObject {

		public string Name = "New Action";

		public Variable Value;

		public abstract void Initialize(GameObject obj);
		public abstract void TriggerEvent(object arg = null);

	}

}