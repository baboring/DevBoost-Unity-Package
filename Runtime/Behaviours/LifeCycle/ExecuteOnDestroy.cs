/* *************************************************
*  Created:  2018-1-28 19:46:32
*  File:     ExecuteOnDestroy.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using UnityEngine;

namespace DevBoost.ActionBehaviour {

	public class ExecuteOnDestroy : Execute {

		void OnDestroy() {
            ExecuteInvoke();
		}
	}
}
