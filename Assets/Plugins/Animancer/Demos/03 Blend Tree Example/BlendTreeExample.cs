// Animancer // Copyright 2018 Kybernetik //

#pragma warning disable CS0618 // Type or member is obsolete (for CrossFade in Animancer Lite).

using UnityEngine;

namespace Animancer.Demos
{
    /// <summary>
    /// An example of how you can use a <see cref="RuntimeAnimatorController"/> containing a single blend tree and
    /// still mix it with other individual animations using Animancer.
    /// <para></para>
    /// More information about using animator controllers with Animancer can be found in
    /// <see href="https://animancer.github.io/docs/using-animancer/animator-controller-states">the documentation.</see>.
    /// </summary>
    [AddComponentMenu("Animancer/Demo/Blend Tree Example")]
    [HelpURL(AnimancerController.APIDocumentationURL + ".Demo/BlendTreeExample")]
    public sealed class BlendTreeExample : MonoBehaviour
    {
        /************************************************************************************************************************/
        #region Animations
        /************************************************************************************************************************/

        public AnimancerController animancer;
        public RuntimeAnimatorController controller;
        public AnimationClip attack;

        [Range(0, 1)]
        public float speedBlend;

        private FloatParameterControllerState _MovementState;

        /************************************************************************************************************************/

        /// <summary>Wrapper property for speedBlend so it can be set by a UnityEvent.</summary>
        public float SpeedBlend { get { return speedBlend; } set { speedBlend = value; } }

        /************************************************************************************************************************/

        private void OnEnable()
        {
            _MovementState = new FloatParameterControllerState(animancer, controller, "Speed");
            animancer.Play(_MovementState);
        }

        /************************************************************************************************************************/

        private void Update()
        {
            _MovementState.Parameter = speedBlend;

            // Attack on click - same as the SimpleExample script.
            // This shows how you can blend between an AnimatorController state and other separate animations.
            if (!animancer.IsPlaying(attack) && Input.GetMouseButtonDown(0))
            {
                var state = animancer.CrossFade(attack);
                state.OnEnd += () => animancer.CrossFade(_MovementState);
            }
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Movement
        /************************************************************************************************************************/

        public Rigidbody body;
        public float runSpeed;
        public float xPositionLimit;

        /************************************************************************************************************************/

        private void FixedUpdate()
        {
            var velocity = body.velocity;
            velocity.x = runSpeed * speedBlend;
            body.velocity = velocity;

            // If we go too far to the right, teleport back.
            var position = transform.position;
            if (position.x >= xPositionLimit)
            {
                position.x = -xPositionLimit;
                transform.position = position;
            }
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
    }
}
