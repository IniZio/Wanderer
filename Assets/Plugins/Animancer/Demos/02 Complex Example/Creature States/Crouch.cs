// Animancer // Copyright 2018 Kybernetik //

#pragma warning disable CS0618 // Type or member is obsolete (for CrossFade in Animancer Lite).

using UnityEngine;

namespace Animancer.Demo
{
    /// <summary>
    /// A <see cref="CreatureState"/> that plays a crouch animation while grounded.
    /// </summary>
    [AddComponentMenu("Animancer/Demo/Crouch")]
    [HelpURL(AnimancerController.APIDocumentationURL + ".Demo/Crouch")]
    public sealed class Crouch : CreatureState
    {
        /************************************************************************************************************************/

        [SerializeField]
        private AnimationClip _Animation;

        private AnimancerState _AnimancerState;

        /************************************************************************************************************************/

        public override bool CanEnterState(CreatureStateType previousState)
        {
            return
                base.CanEnterState(previousState) &&
                Creature.Feet.IsGrounded;
        }

        /************************************************************************************************************************/

        private void OnEnable()
        {
            _AnimancerState = Creature.Anim.CrossFade(_Animation);
        }

        /************************************************************************************************************************/

        public float NormalizedTime
        {
            get
            {
                if (_AnimancerState == null)
                    return 0;

                return Mathf.Clamp01(_AnimancerState.NormalizedTime);
            }
        }

        /************************************************************************************************************************/
    }
}
