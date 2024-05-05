using UnityEngine;

namespace DevBoost.Extensions
{

    public static class AnimatorExtensions
    {
        // Reset All Parameters (Clear all)
        public static void ResetAllParameters(this Animator animator)
        {
            if (null == animator)
                return;
            // Reset All animator flag
            AnimatorControllerParameter[] parameters = animator.parameters;
            for (int i = 0; i < parameters.Length; i++)
            {
                AnimatorControllerParameter parameter = parameters[i];
                switch (parameter.type)
                {
                    case AnimatorControllerParameterType.Int:
                        animator.SetInteger(parameter.name, parameter.defaultInt);
                        break;
                    case AnimatorControllerParameterType.Float:
                        animator.SetFloat(parameter.name, parameter.defaultFloat);
                        break;
                    case AnimatorControllerParameterType.Bool:
                        animator.SetBool(parameter.name, parameter.defaultBool);
                        break;
                    case AnimatorControllerParameterType.Trigger:
                        animator.ResetTrigger(parameter.name);
                        break;
                }
            }
        }

        public static bool IsContainParam(this Animator animator, string param)
        {
            Debug.Assert(animator != null, "animator is null");

            if (animator == null || !animator.isActiveAndEnabled || animator.runtimeAnimatorController == null)
                return false;

            // Reset All animator flag
            AnimatorControllerParameter[] parameters = animator.parameters;
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].name == param)
                    return true;
            }
            return false;
        }
    }

}
