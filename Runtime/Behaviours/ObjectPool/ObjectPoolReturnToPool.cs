/* *************************************************
*  File:     LoadSceneNode.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using UnityEngine;
using UnityEngine.SceneManagement;

namespace DevBoost.ActionBehaviour {

	using DevBoost.Utilities;


	public class ObjectPoolReturnToPool : ActionNode {


		[SerializeField]
		protected PooledObject target;

        // Action Script
        protected override ActionState OnUpdate() {

			if (target == null)
				return ActionState.Error;

			target.ReturnToPool();

			return ActionState.Success;

		}

	}

}