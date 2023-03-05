using DevBoost.ActionBehaviour;
using UnityEngine;

namespace DevBoost.ActionScript
{
	public class MonoActionTrigger : ActionTrigger {

        [SerializeField]
        public StartOption startType;

        private void Awake()
        {
            if (StartOption.Awake == startType)
                base.InvokeAction();
        }
        void Start()
        {
            if (StartOption.Start == startType)
                base.InvokeAction();
        }

        private void OnEnable()
        {
            if (StartOption.Enabled == startType)
                base.InvokeAction();
        }
        private void OnDisable()
        {
            if (StartOption.Disabled == startType)
                base.InvokeAction();
        }

        private void OnDestroy()
        {
            if (StartOption.Destroy == startType)
                base.InvokeAction();
        }

        public void StartManually()
        {
            base.InvokeAction();
        }
    }


}
