/* ---------------------------------------------------------------------
 * Created on Mon Aug 19 2021 3:33:10 PM
 * Author : Benjamin Park
 * Description : Dissolve Sprite Renderer
--------------------------------------------------------------------- */

using DevBoost.Effects;
using NaughtyAttributes;
using UnityEngine;

namespace DevBoost.ActionBehaviour
{
    /// <summary>
    /// Set Objective Type sprite to the sprite renderer
    /// </summary>
    public class DissolveSpriteRenderer : UIBaseAction
    {
        [SerializeField]
        [Dropdown("GetMode"), Label("Mode")]
        private bool reverse;

        private SpriteRenderer[] renderers;
        //private TMPro.TextMeshPro [] textRenderers;

        private DropdownList<bool> GetMode() => new DropdownList<bool>() {
                { "Fade out",   true },
                { "Fade in",   false },
            };

        protected override void OnReset()
        {
            base.OnReset();
            renderers = target.GetComponentsInChildren<SpriteRenderer>();
            //textRenderers = target.GetComponentsInChildren<TMPro.TextMeshPro>();
        }

        protected override bool DoUpdateFrame(float t)
        {
            if (reverse)
                t = 1 - t;
            if (tweenType != EasingType.None)
                t = Tween.Ease(t, 1, tweenType);


            for (int i = 0; i < renderers.Length; i++)
            {
                var color = renderers[i].color;
                renderers[i].color = new Color(color.r, color.g, color.b, t); /// Here, be sure to reset the color a value in Fade mode to 1, otherwise it will always be 0 after one display.
            }

            //for (int i = 0; i < textRenderers.Length; i++)
            //    textRenderers[i].alpha = t;

            return true;
        }


    }

}