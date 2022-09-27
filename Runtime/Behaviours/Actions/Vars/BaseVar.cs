/* *************************************************
*  Created:  2018-1-28 19:51:59
*  File:     BaseVar.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/
using System;
using UnityEngine;

namespace DevBoost.ActionBehaviour
{

    abstract public class NullableVar<T> : BaseVar where T : class
    {
        sealed public override Type agentType { get { return typeof(T); } }
        new public T agent { get { return base.agent as T; } }

        [SerializeField]
        public T Value;

        public void SetValue(T val) { Value = val; }

    }


    abstract public class StrcutVar<T> : BaseVar where T : struct
    {
        sealed public override Type agentType { get { return typeof(T); } }

        [SerializeField]
        public T Value;

        public void SetValue(T val) { Value = val; }
    }

    public abstract class BaseVar : MonoBehaviour, IBaseNode
    {
        public virtual Type agentType { get { return GetType(); } }
        public IBaseNode agent { get { return this; } }

    }
}