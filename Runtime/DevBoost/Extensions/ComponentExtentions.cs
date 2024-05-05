using UnityEngine;

namespace DevBoost.Extensions
{
    public static class ComponentExtention
    {
        public static ActionBehaviour.ActionNode AddToOnDestroy(this MonoBehaviour obj, System.Action callback)
        {
            var act = ActionBehaviour.ActionNode.AddTo<ActionBehaviour.ExecuteOnDestroy>(obj);
            act.AddListener(callback);
            return act;
        }

        public static ActionScript.MonoActionTrigger AddTo(this MonoBehaviour obj, System.Action callback, ActionBehaviour.StartOption startOption = ActionBehaviour.StartOption.Destroy)
        {
            var act = obj.GetOrAddComponent<ActionScript.MonoActionTrigger>();
            act.startType = startOption;
            act.AddListener((v) => callback());
            return act;
        }

    }
}
