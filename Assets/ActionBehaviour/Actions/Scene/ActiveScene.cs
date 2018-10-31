/* *************************************************
*  File:     ActiveScene.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using UnityEngine;
using UnityEngine.SceneManagement;

namespace ActionBehaviour
{
    using NaughtyAttributes;

    public class ActiveScene : ActionNode
    {
        [BoxGroup("Setting")]
        [Dropdown("SceneNameSet")]
        [SerializeField] protected string SceneName; // Scene Name

        [BoxGroup("Setting")]
        [SerializeField] protected StringSet SceneNameSet;

        // Action Script
        public override ActionState OnUpdate()
        {
            // parent update or woring on async
            ActionState result = base.OnUpdate();
            if (result != ActionState.Success)
                return result;

            if (string.IsNullOrEmpty(SceneName))
                return ActionState.Error;

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneName));

            return ActionState.Success;

        }
    }
}