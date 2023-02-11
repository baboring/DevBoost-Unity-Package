/* ---------------------------------------------------------------------
 * Created on Mon Aug 19 2021 3:33:10 PM
 * Author : Benjamin Park
 * Description : Set color to sprite renderer
--------------------------------------------------------------------- */

using UnityEngine;

namespace DevBoost.ActionBehaviour
{
    /// <summary>
    /// Set Objective Type sprite to the sprite renderer
    /// </summary>
    public class SetColorSpriteRenderer : ActionNode
    {
        [SerializeField]
        private GameObject targetObject;
        [SerializeField]
        private Color color = Color.white;

        private SpriteRenderer[] renderers;
        //private TMPro.TextMeshPro [] textRenderers;

        protected override void OnReset()
        {
            base.OnReset();
            renderers = targetObject.GetComponentsInChildren<SpriteRenderer>();
            //textRenderers = targetObject.GetComponentsInChildren<TMPro.TextMeshPro>();
        }

        protected override ActionState OnUpdate()
        {
            // parent update
            ActionState result = base.OnUpdate();
            if (result != ActionState.Success && result != ActionState.Running)
                return result;

            for (int i = 0; i < renderers.Length; i++)
                renderers[i].color = color; 

            //for (int i = 0; i < textRenderers.Length; i++)
            //    textRenderers[i].color = color;

            return result;
        }


    }

}