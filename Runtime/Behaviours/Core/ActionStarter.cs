/* *************************************************
*  Created:  2018-06-02 19:46:32
*  File:     ActionStarter.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using UnityEngine;

namespace DevBoost.ActionBehaviour
{

    public enum StartOption
    {
        None = 0,   // nothing to start
        Start,  // called on Start
        Enabled,  // called on enabled
        Disabled,  // called on disabled
        Destroy,  // called on destroy
        Awake,  // called on Awake
    }

    /// <summary>
    /// Action starter.
    /// </summary>
    public class ActionStarter : Execute
    {

        [SerializeField]
        protected StartOption startType;

        private void Awake()
        {
            if (StartOption.Awake == startType)
                base.Execute();
        }
        void Start()
        {
            if (StartOption.Start == startType)
                base.Execute();
        }

        private void OnEnable()
        {
            if (StartOption.Enabled == startType)
                base.Execute();
        }
        private void OnDisable()
        {
            if (StartOption.Disabled == startType)
                base.Execute();
        }

        private void OnDestroy()
        {
            if (StartOption.Destroy == startType)
                base.Execute();
        }

        public void StartManually()
        {
            base.Execute();
        }
    }


}
