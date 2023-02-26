
/* *************************************************
*  Created:  2018-1-28 20:15:39
*  File:     SetImageSpriteAction.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using UnityEngine;
using UnityEngine.UI;

namespace DevBoost.ActionBehaviour {

	public class SetImageSpriteAction : ActionNode {

		[SerializeField]
		public Image image;

		[SerializeField]
		public Sprite sprite;

        protected override ActionState OnUpdate() {

			// parent update
			ActionState result = base.OnUpdate();
			if(result != ActionState.Success)
				return result;

			SetSprite(sprite);
			return ActionState.Success;
		}

		public void SetSprite(Sprite spr)
        {
			if (image != null)
				image.sprite = spr;

		}
	}

}