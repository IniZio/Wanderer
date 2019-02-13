// Animancer // Copyright 2018 Kybernetik //

#pragma warning disable CS0618 // Type or member is obsolete (for CrossFade in Animancer Lite).

using UnityEngine;

namespace Animancer.Demo
{
    /// <summary>
    /// An example of how to direct an <see cref="AnimancerController"/> with a script.
    /// </summary>
    [AddComponentMenu("Animancer/Demo/Simple Example")]
    [HelpURL(AnimancerController.APIDocumentationURL + ".Demo/SimpleExample")]
    public sealed class SimpleExample : MonoBehaviour
    {
        /************************************************************************************************************************/

        // Use an AnimancerController instead of a regular Animator.
        public AnimancerController animancer;

        // Reference the AnimationClips you want.
        public AnimationClip idle;
        public AnimationClip attack;

        /************************************************************************************************************************/

        private void OnEnable()
        {
            // Play the idle on startup.
            animancer.Play(idle);
        }

        /************************************************************************************************************************/

        private void Update()
        {
            // If the attack animation isn’t already playing when the user
            // clicks the left mouse button.
            if (!animancer.IsPlaying(attack) && Input.GetMouseButtonDown(0))
            {
                // Stop all other animations and play the attack animation.
                var state = animancer.CrossFade(attack);

                // Register a callback for when it reaches its end.
                // Calling Play already cleared any previous callbacks.
                state.OnEnd += OnAttackEnd;

                // If this were a coroutine, we could wait for the animation to
                // finish using 'yield return state;'.
            }
        }

        /************************************************************************************************************************/

        private void OnAttackEnd()
        {
            // Now that the attack is done, go back to idle.
            animancer.CrossFade(idle);
        }

        /************************************************************************************************************************/
    }
}
