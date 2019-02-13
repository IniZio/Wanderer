// Animancer // Copyright 2018 Kybernetik //

#pragma warning disable CS0618 // Type or member is obsolete (for CrossFade in Animancer Lite).

using UnityEngine;

namespace Animancer.Demo
{
    /// <summary>
    /// A <see cref="CreatureState"/> that performs an attack animation then returns to idle.
    /// </summary>
    [AddComponentMenu("Animancer/Demo/Attack")]
    [HelpURL(AnimancerController.APIDocumentationURL + ".Demo/Attack")]
    public sealed class Attack : CreatureState
    {
        /************************************************************************************************************************/

        [SerializeField]
        private AnimationClip _Animation;

        /************************************************************************************************************************/

        private void OnEnable()
        {
            // If an attack ends and we attack again immediately we want the new animation to fade in while the old one
            // fades out so we use CrossFadeNew instead of the regular CrossFade to ensure that it starts from the
            // beginning and duplicates the state (if necessary).

            var state = Creature.Anim.CrossFadeNew(_Animation);
            state.OnEnd += ReturnToIdle;
        }

        /************************************************************************************************************************/

        public override bool CanExitState(CreatureStateType nextState)
        {
            // Nothing can interript an attack until it is done.
            // In a real game we might want Flinch to be able to interrupt it in case you get hit.
            return false;
        }

        /************************************************************************************************************************/
    }
}
