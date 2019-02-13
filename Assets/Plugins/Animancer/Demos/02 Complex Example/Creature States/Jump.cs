// Animancer // Copyright 2018 Kybernetik //

#pragma warning disable CS0618 // Type or member is obsolete (for CrossFade in Animancer Lite).

using UnityEngine;

namespace Animancer.Demo
{
    /// <summary>
    /// A <see cref="CreatureState"/> that plays a jump animation and applies some upwards force based on the duration
    /// that the <see cref="Crouch"/> state was held.
    /// </summary>
    [AddComponentMenu("Animancer/Demo/Jump")]
    [HelpURL(AnimancerController.APIDocumentationURL + ".Demo/Jump")]
    public sealed class Jump : CreatureState
    {
        /************************************************************************************************************************/

        [SerializeField]
        private AnimationClip _Animation;

        [SerializeField]
        private Crouch _Crouch;

        [SerializeField]
        private Move _Move;

        [SerializeField]
        private float _MinForce;

        [SerializeField]
        private float _MaxForce;

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
            var force = _Crouch != null ?
                Mathf.Lerp(_MinForce, _MaxForce, _Crouch.NormalizedTime) :
                _MinForce;

            Creature.Body.AddForce(0, force, 0, ForceMode.VelocityChange);

            var state = Creature.Anim.CrossFade(_Animation);
            state.OnEnd += ReturnToIdle;
            // Generally you would enter a "Fall" state after jumping,
            // but we don't have a separate animation for it so we just use idle.
        }

        /************************************************************************************************************************/

        private void FixedUpdate()
        {
            if (_Move != null)
                _Move.UpdateMovement();
        }

        /************************************************************************************************************************/
    }
}
