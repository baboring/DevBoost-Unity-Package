using UnityEngine;
using System.Collections;

namespace DevBoost.ActionBehaviour
{
    using NaughtyAttributes;

    public class StringId : BaseNode
    {

        [Dropdown("Set")]
        [SerializeField]
        protected string id;

        [SerializeField]
        protected StringSet Set;

        // external method
        public string Value {
            get { return id; }
        }


        protected override ActionState OnUpdate() { return ActionState.Success; }


	}


}