/* ---------------------------------------------------------------------
 * Created on Mon Aug 19 2021 3:33:10 PM
 * Author : Benjamin Park
 * Description : Unload Unused Assets action
--------------------------------------------------------------------- */

using System.Collections;
using UnityEngine;

namespace DevBoost.ActionBehaviour
{
    public class UnloadUnusedAssetsAction : ActionNode
    {
        [SerializeField]
        private bool editor;

        private bool isDone;

        protected override void OnReset()
        {
            isDone = false;
            base.OnReset();
        }
        protected override ActionState OnUpdate()
        {
            // parent update
            ActionState result = base.OnUpdate();
            if (result != ActionState.Success && result != ActionState.Running)
                return result;

            if (result == ActionState.Success && !isDone)
            {
                state = ActionState.Running;
                StartCoroutine(Fire());
            }
#if UNITY_EDITOR
            if (editor)
                UnityEditor.EditorUtility.UnloadUnusedAssetsImmediate();
#endif 
            return result;

        }


        IEnumerator Fire()
        {
            System.GC.Collect();

            yield return Resources.UnloadUnusedAssets();
            isDone = true;
            state = ActionState.Success;
        }


    }

}