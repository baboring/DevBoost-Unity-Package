/* *************************************************
*  Created:  2018-1-28 19:46:32
*  File:     ExecuteOnAwake.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevBoost.ActionBehaviour {

    public class ExecuteOnAwake : Execute {

		void Awake() {
            ExecuteInvoke();
		}		
	}
}
