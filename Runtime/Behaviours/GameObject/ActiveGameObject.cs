
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

	public class ActiveGameObject : ActionNode {

        [ReorderableList]
		[SerializeField]
		protected GameObject[] objects;

        protected override ActionState OnUpdate() {

			// parent update
			ActionState result = base.OnUpdate();
			if(result != ActionState.Success)
				return result;
			
			for( int i=0;i < objects.Length; ++i )
            {
                Debug.Assert(objects[i] != null, gameObject.name + " has an error nodes !!");
                if (null != objects[i])
                    objects[i].SetActive(true);
            }
            return ActionState.Success;
		}
	}

}