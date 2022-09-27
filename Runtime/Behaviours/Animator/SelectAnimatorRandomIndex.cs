using UnityEngine;

/// <summary>
/// State behaviour to select a random integer between a given range and set the paramName Animator parameter to that value
/// Chooses value OnStateEnter
/// </summary>
public class SelectAnimatorRandomIndex: StateMachineBehaviour
{
    [SerializeField]
    private Vector2Int range;   // x ann y means in range of min max

    [SerializeField]
    public string paramName;

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
            var val = Random.Range(range.x, range.y);
            animator.SetInteger(paramName, val);
        }

    }
}
