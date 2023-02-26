/* ---------------------------------------------------------------------
 * Created on Mon Aug 19 2021 3:33:10 PM
 * Author : Benjamin Park
 * Description : Set Local Scale Action
--------------------------------------------------------------------- */

using UnityEngine;
using NaughtyAttributes;

namespace DevBoost.ActionBehaviour
{
    public class CopyPositionAction : ActionNode
    {
        [SerializeField]
        protected bool useObjectVar;
        [SerializeField, HideIf("useObjectVar")]
        private GameObject Target;
        [SerializeField, ShowIf("useObjectVar")]
        private GameObjectVar TargetVar;
        [SerializeField, HideIf("useObjectVar")]
        private Transform Source;
        [SerializeField, ShowIf("useObjectVar")]
        private GameObjectVar SourceVar;

        protected override void OnReset()
        {
            base.OnReset();

            if (Target == null)
                Target = this.gameObject;
        }

        protected override ActionState OnUpdate()
        {
            // parent update
            ActionState result = base.OnUpdate();
            if (result != ActionState.Success && result != ActionState.Running)
                return result;

            if (SourceVar != null && SourceVar.Value != null)
                Source = SourceVar.Value.transform;
            if (TargetVar != null && TargetVar.Value != null)
                Target = TargetVar.Value;

            if (Target != null && Source != null)
                Target.transform.position = Source.position;

            return result;

        }



    }

}