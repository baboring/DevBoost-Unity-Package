
/* *************************************************
*  Created:  2018-1-28 20:15:39
*  File:     ObjectActiveNode.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevBoost.ActionBehaviour {

    using NaughtyAttributes;

	[RequireComponent(typeof(Rigidbody2D))]
	public class Rigidbody2dAddForce : ActionNode {

		[SerializeField]
		protected Vector2 force;

		Rigidbody2D rigidbody;

        protected override ActionState OnUpdate() {

			// parent update
			ActionState result = base.OnUpdate();
			if(result != ActionState.Success)
				return result;
			if(null == rigidbody)
				rigidbody = GetComponent<Rigidbody2D>();
			rigidbody.AddForce(force);
			return ActionState.Success;
		}
	}

}