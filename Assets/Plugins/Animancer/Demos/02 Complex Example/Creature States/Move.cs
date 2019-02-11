// Animancer // Copyright 2018 Kybernetik //

#pragma warning disable CS0618 // Type or member is obsolete (for CrossFade in Animancer Lite).

using UnityEngine;

namespace Animancer.Demo
{
    /// <summary>
    /// A <see cref="CreatureState"/> that plays a move animation and applies some sideways force based on the
    /// <see cref="CreatureBrain.Movement"/>.
    /// </summary>
    [AddComponentMenu("Animancer/Demo/Move")]
    [HelpURL(AnimancerController.APIDocumentationURL + ".Demo/Move")]
    public sealed class Move : CreatureState
    {
        /************************************************************************************************************************/

        [SerializeField]
        private AnimationClip _Animation;

        [SerializeField]
        private float _Acceleration;

        /************************************************************************************************************************/

        private void OnEnable()
        {
            Creature.Anim.CrossFade(_Animation);
        }

        /************************************************************************************************************************/

        private void FixedUpdate()
        {
            if (Creature.Brain == null || Creature.Brain.Movement == 0)
            {
                ReturnToIdle();
                return;
            }

            UpdateMovement();
        }

        /************************************************************************************************************************/

        public void UpdateMovement()
        {
            if (Creature.Brain == null)
                return;

            float force = Creature.Brain.Movement * _Acceleration;

            Creature.Body.AddForce(force, 0, 0, ForceMode.Acceleration);
        }

        /************************************************************************************************************************/
    }
}
