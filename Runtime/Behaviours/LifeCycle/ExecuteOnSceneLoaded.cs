/* *************************************************
*  Created:  2018-1-28 19:46:32
*  File:     ExecuteOnSceneLoaded.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using UnityEngine;
using UnityEngine.SceneManagement;

namespace DevBoost.ActionBehaviour {

    using NaughtyAttributes;

    public class ExecuteOnSceneLoaded : Execute {

        [BoxGroup("Setting")]
        [SerializeField] protected bool isAny = true;

        [BoxGroup("Setting")]
        [Dropdown("SceneNameSet")]
        [HideIf("isAny")]
        [SerializeField] protected string SceneName; // Scene Name

        [BoxGroup("Setting")]
        [HideIf("isAny")]
        [SerializeField] protected StringSet SceneNameSet;

        // called first
        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        // called when the game is terminated
        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        } 

        // called second
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.LogFormat("OnSceneLoaded: {0},Mode = {1}", scene.name, mode.ToString());

            if(isAny)
                ExecuteInvoke();
            else if(SceneName == scene.name)
                ExecuteInvoke();
        }
    }
}
