/* ---------------------------------------------------------------------
 * Created on Mon Sep 16 2021 3:33:10 PM
 * Author : Benjamin Park
 * Description : Spawn Particle Effect
--------------------------------------------------------------------- */

using DevBoost.Utilities;
using NaughtyAttributes;
using UnityEngine;

namespace DevBoost.ActionBehaviour
{
    public class SpawnParticleEffect : ActionNode
    {
        [SerializeField]
        private PrefabGroup prefabGroup;

        [SerializeField]
        private string Key;

        [SerializeField]
        private Transform parent;

        [SerializeField]
        protected bool localScaleOne;
        [SerializeField]
        protected bool localPosZero;

        [SerializeField]
        protected bool setAsValue;

        [SerializeField, ShowIf("setAsValue")]
        private GameObjectVar clonedObjectVar;

        public GameObject cloned { get; private set; }

        protected override ActionState OnUpdate()
        {
            // parent update
            ActionState result = base.OnUpdate();
            if (result != ActionState.Success && result != ActionState.Running)
                return result;

            SpawnOption option = SpawnOption.None;
            if (localScaleOne)
                option |= SpawnOption.LocalScaleOne;
            if (localPosZero)
                option |= SpawnOption.LocalPosZero;
            
            cloned = prefabGroup.Spawn(Key, parent, option);

            if (setAsValue)
                clonedObjectVar?.SetValue(cloned);

            return result;

        }


    }

}