/* ---------------------------------------------------------------------
 * Created on Mon Aug 19 2021 3:33:10 PM
 * Author : Benjamin Park
 * Description : quit current application
--------------------------------------------------------------------- */

using UnityEngine;

namespace DevBoost.ActionBehaviour
{
    public class ApplicationQuit : ActionNode
    {
        [SerializeField] int exitCode;

        protected override ActionState OnUpdate()
        {
            // parent update
            ActionState result = base.OnUpdate();
            if (result != ActionState.Success)
                return result;

            Application.Quit(exitCode);

            return ActionState.Success;

        }
    }

}