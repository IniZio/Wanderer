// Animancer // Copyright 2018 Kybernetik //

#pragma warning disable CS0618 // Type or member is obsolete (for CrossFade in Animancer Lite).

using UnityEngine;

namespace Animancer.Demo
{
    /// <summary>
    /// A <see cref="CreatureState"/> that plays an idle animation.
    /// </summary>
    [AddComponentMenu("Animancer/Demo/Idle")]
    [HelpURL(AnimancerController.APIDocumentationURL + ".Demo/Idle")]
    public sealed class Idle : CreatureState
    {
        /************************************************************************************************************************/

        [SerializeField]
        private AnimationClip _Animation;

        /************************************************************************************************************************/

        private void OnEnable()
        {
            Creature.Anim.CrossFade(_Animation);
        }

        /************************************************************************************************************************/
    }
}
