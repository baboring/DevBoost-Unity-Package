/* *************************************************
*  Created:  2018-4-01 19:46:32
*  File:     ViewController.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DevBoost.ActionBehaviour {

    using DevBoost.Utilities;

    public class UIViewManager : SingletonMono<UIViewManager> {

		protected new void Awake()
		{
            base.Awake();
		}

		public void Execute() {
            
        }

		
	}

#if UNITY_EDITOR
    [CustomEditor(typeof(UIViewManager), true)]
    [CanEditMultipleObjects]
    public class UIViewManagerEditor : Editor
    {
      public override void OnInspectorGUI()
      {
            DrawDefaultInspector();

            UIViewManager myScript = (UIViewManager)target;
            if(GUILayout.Button("Execute"))
            {
                myScript.Execute();
            }
      }
    }
#endif
}
