/* ---------------------------------------------------------------------
 * Created on Mon Aug 19 2021 3:33:10 PM
 * Author : Benjamin Park
 * Description : Set ui elements with color
--------------------------------------------------------------------- */

using UnityEngine;
using UnityEngine.UI;

namespace DevBoost.ActionBehaviour
{
    /// <summary>
    /// Set ui elements with color
    /// </summary>
    public class SetColorUIElement : ActionNode
    {
        [SerializeField]
        private Graphic[] graphic;
        [SerializeField]
        private Color color = Color.white;


        protected override ActionState OnUpdate()
        {
            // parent update
            ActionState result = base.OnUpdate();
            if (result != ActionState.Success && result != ActionState.Running)
                return result;

            foreach(var item in graphic)
                if (item != null)
                    item.color = color; 

            return result;
        }


    }

}