/* ---------------------------------------------------------------------
 * Created on Mon Aug 19 2021 3:33:10 PM
 * Author : Benjamin Park
 * Description : GabageCollect action
--------------------------------------------------------------------- */

namespace DevBoost.ActionBehaviour
{
    public class GabageCollectAction : ActionNode
    {
        protected override void OnReset()
        {
            base.OnReset();
        }
        protected override ActionState OnUpdate()
        {
            // parent update
            ActionState result = base.OnUpdate();
            if (result != ActionState.Success && result != ActionState.Running)
                return result;

            System.GC.Collect();
            return result;

        }

    }

}