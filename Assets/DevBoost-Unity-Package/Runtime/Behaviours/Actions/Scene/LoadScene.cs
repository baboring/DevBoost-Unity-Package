/* *************************************************
*  File:     LoadSceneNode.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using UnityEngine;
using UnityEngine.SceneManagement;

namespace ActionBehaviour {

    using NaughtyAttributes;

	public enum LOAD_SCENE_METHOD {
		Sync = 0,
		Async = 1
	}

    public class LoadScene : ActionNode {

		[BoxGroup("Setting")]
        [SerializeField] protected bool isIndex	= true;

        [BoxGroup("Setting")]
		[Dropdown("LevelNameSet")]
        [HideIf("isIndex")]
        [SerializeField] protected string LevelName; // Scene Name

        [BoxGroup("Setting")]
        [ShowIf("isIndex")]
		[SerializeField] protected int LeveIndex;		// Scene Build Index

		[BoxGroup("Setting")]
        [SerializeField] protected StringSet LevelNameSet;

        [BoxGroup("Setting")]
		[SerializeField] protected LOAD_SCENE_METHOD loadMethod;	// Sync, Async

        [BoxGroup("Setting")]
		[SerializeField] protected LoadSceneMode loadMode;	// Addictive, Single

        [BoxGroup("Actions")]
        [SerializeField] protected ActionNode NodeOnStartLoad;

        [BoxGroup("Actions")]
        [SerializeField] protected ActionNode NodeOnSceneLoaded;

        // async operator
        AsyncOperation operation = null;

        // reset state all 
        protected override void OnReset()
        {
            base.OnReset();

            if (null != operation)
                operation.completed -= OnComplete;

            operation = null;
        }

        void OnComplete(AsyncOperation async)
        {
            state = ActionState.Success;
            if (null != NodeOnSceneLoaded)
                NodeOnSceneLoaded.ExecuteInvoke();
        }

        // Action Script
        protected override ActionState OnUpdate() {

			// parent update or woring on async
			ActionState result = base.OnUpdate();
			if(result != ActionState.Success || operation != null)
				return result;

            //SceneManager.sceneLoaded += OnSceneLoaded;

            // Run Before load Scene
            if (null != NodeOnStartLoad)
                NodeOnStartLoad.ExecuteInvoke();

			// Load scene index
            if(isIndex){
                if(loadMethod == LOAD_SCENE_METHOD.Async) {
                    state = ActionState.Running;
                    operation = SceneManager.LoadSceneAsync(LeveIndex, loadMode);
                    operation.completed += OnComplete;
                }
                else {
                    SceneManager.LoadScene(LeveIndex, loadMode);
                    state = ActionState.Success;
                }
			}
			// Load scene name
			else {
                if(loadMethod == LOAD_SCENE_METHOD.Async) {
                    state = ActionState.Running;
                    operation = SceneManager.LoadSceneAsync(LevelName, loadMode);
                    operation.completed += OnComplete;
                }
                else {
                    SceneManager.LoadScene(LevelName, loadMode);
                    state = ActionState.Success;
                }
			}

            return state;

		}

	}

}