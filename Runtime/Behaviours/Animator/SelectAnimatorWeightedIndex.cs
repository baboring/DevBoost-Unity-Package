using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// State behaviour to select a random integer based on weight biases and set the given paramName Animator parameter to the new value
/// Chooses value OnStateEnter
/// </summary>
public class SelectAnimatorWeightedIndex : StateMachineBehaviour
{
    [SerializeField]
    private List<float> weights = new List<float>();

    [SerializeField]
    public string paramName;

    private List<float> internalWeights = new List<float>();
    private float totalWeight;

    private void Awake()
    {
        totalWeight = 0;
        if (weights.Count > 0)
        {
            foreach (var val in weights)
            {
                totalWeight += val;
                internalWeights.Add(totalWeight);
            }
        }
    }

    /// <summary>
    /// Determine the random child to choose
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="stateInfo"></param>
    /// <param name="layerIndex"></param>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator != null && paramName != null)
        {

            int paramVal = -1;
            if (internalWeights.Count > 0)
            {

                float val = Random.Range(0.0f, totalWeight);
                for (int i = 0; i < internalWeights.Count; i++)
                {
                    if (val <= internalWeights[i])
                    {
                        paramVal = i;
                        break;
                    }
                }
                if (paramVal >= 0)
                {
                    animator.SetInteger(paramName, paramVal);
                }
            }
        }
    }
}
