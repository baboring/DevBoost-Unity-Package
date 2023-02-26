/* ---------------------------------------------------------------------
 * Created on Mon Sep 16 2021 3:33:10 PM
 * Author : Benjamin Park
 * Description : Spawn prefab from Group Prefabs
--------------------------------------------------------------------- */

using DevBoost.Utilities;
using UnityEngine;

namespace DevBoost.ActionBehaviour
{
    public class SpawnPrefabGroup : ActionNode
    {
        [SerializeField]
        private string lable;

        [SerializeField]
        private PrefabGroup group;

        [SerializeField]
        private GameObject target;
        [SerializeField]
        private GameObjectVar gameObjectVar;

        [SerializeField]
        private bool reset_local_pos = true;
        [SerializeField]
        private bool reset_local_scale = true;

        // result spawned object
        public GameObject spawned { get; private set; }


        protected override ActionState OnUpdate()
        {
            // parent update
            ActionState result = base.OnUpdate();
            if (result != ActionState.Success && result != ActionState.Running)
                return result;

            // override game object
            if (gameObjectVar != null && gameObjectVar.Value != null)
                target = gameObjectVar.Value;

            SpawnOption option = SpawnOption.None;
            if (reset_local_pos)
                option |= SpawnOption.LocalPosZero;
            if (reset_local_scale)
                option |= SpawnOption.LocalScaleOne;

            spawned = group.Spawn(lable, target.transform, option);

            return result;

        }


    }

}