/* *************************************************
*  Created:  2018-1-28 19:46:32
*  File:     ExecuteOnParticleSystemStopped.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevBoost.ActionBehaviour
{

    [RequireComponent(typeof(ParticleSystem)), DisallowMultipleComponent]
    public class ExecuteOnParticleSystemStopped : Execute
    {
        void Start()
        {
            var main = GetComponent<ParticleSystem>().main;
            main.stopAction = ParticleSystemStopAction.Callback;
        }

        void OnParticleSystemStopped()
        {
            ExecuteInvoke();
        }
    }
}
