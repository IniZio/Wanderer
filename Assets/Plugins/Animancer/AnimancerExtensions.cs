// Animancer // Copyright 2018 Kybernetik //

using System;
using UnityEngine;

namespace Animancer
{
    /// <summary>Various extension methods.</summary>
    public static class AnimancerExtensions
    {
        /************************************************************************************************************************/

        /// <summary>[Animancer Extension]
        /// Adds the specified type of <see cref="AnimancerController"/>, links it to the 'animator', and returns it.
        /// </summary>
        public static T AddAnimancerController<T>(this Animator animator) where T : AnimancerController
        {
            var animancer = animator.gameObject.AddComponent<T>();
            animancer.Animator = animator;
            return animancer;
        }

        /// <summary>
        /// Adds an <see cref="AnimancerController"/>, links it to the 'animator', and returns it.
        /// </summary>
        public static AnimancerController AddAnimancerController(this Animator animator)
        {
            return animator.AddAnimancerController<AnimancerController>();
        }

        /************************************************************************************************************************/

        /// <summary>[Animancer Extension]
        /// Returns the <see cref="AnimancerController"/> on the same <see cref="GameObject"/> as the 'animator' if
        /// there is one. Otherwise this method adds a new one and returns it.
        /// </summary>
        public static AnimancerController GetOrAddAnimancerController(this Animator animator)
        {
            var animancer = animator.GetComponent<AnimancerController>();
            if (animancer != null)
                return animancer;
            else
                return animator.AddAnimancerController<AnimancerController>();
        }

        /************************************************************************************************************************/

        /// <summary>[Animancer Extension]
        /// Stops all other animations, plays the 'clip' from the start, and returns its state.
        /// </summary>
        public static AnimancerState Play(this Animator animator, AnimationClip clip, int layerIndex = 0)
        {
            var animancer = animator.GetOrAddAnimancerController();
            return animancer.Play(clip, layerIndex);
        }

        /************************************************************************************************************************/

        /// <summary>[Pro-Only] [Animancer Extension]
        /// Fades out all other animations, fades in the 'clip', and returns its state.
        /// <para></para>
        /// Note that if the 'clip' is already playing, it will continue doing so from the current time. If you wish
        /// to ensure that it starts from the beginning you should use
        /// <see cref="CrossFadeNew(AnimationClip, float)"/> instead.
        /// </summary>
#if !UNITY_EDITOR
        [Obsolete(AnimancerPlayable.ProOnlyMessage)]
#endif
        public static AnimancerState CrossFade(this Animator animator, AnimationClip clip, float fadeDuration = AnimancerPlayable.DefaultFadeDuration, int layerIndex = 0)
        {
            var animancer = animator.GetOrAddAnimancerController();
            return animancer.CrossFade(clip, fadeDuration, layerIndex);
        }

        /************************************************************************************************************************/

        /// <summary>[Pro-Only] [Animancer Extension]
        /// Fades out all other animations, fades in the specified 'clip' from the start, and returns it.
        /// <para></para>
        /// If the 'clip' isn't currently at 0 weight, this method will actually fade it to 0 along with the others
        /// and create and return a new state with the same clip to fade to 1. This ensures that calling this method
        /// will always fade out from all current states and fade in from the start of the desired animation. States
        /// created for this purpose are cached so they can be reused in the future.
        /// <para></para>
        /// Calling this method repeatedly on subsequent frames will probably have undesirable effects; you most likely
        /// want to call <see cref="CrossFade(object, float)"/> instead.
        /// </summary>
#if !UNITY_EDITOR
        [Obsolete(AnimancerPlayable.ProOnlyMessage)]
#endif
        public static AnimancerState CrossFadeNew(this Animator animator, AnimationClip clip, float fadeDuration = AnimancerPlayable.DefaultFadeDuration, int layerIndex = 0)
        {
            var animancer = animator.GetOrAddAnimancerController();
            return animancer.CrossFadeNew(clip, fadeDuration, layerIndex);
        }

        /************************************************************************************************************************/

        /// <summary>[Pro-Only] [Animancer Extension]
        /// Calculates all thresholds using the <see cref="Motion.averageSpeed"/> of each state.
        /// </summary>
#if !UNITY_EDITOR
        [Obsolete(AnimancerPlayable.ProOnlyMessage)]
#endif
        public static void CalculateThresholdsFromAverageVelocityXZ(this AnimationMixer<Vector2> mixer)
        {
            mixer.ValidateThresholdCount();

            var count = mixer.States.Length;
            for (int i = 0; i < count; i++)
            {
                var state = mixer.States[i];
                if (state == null)
                    continue;

                var averageVelocity = state.AverageVelocity;
                mixer.SetThreshold(i, new Vector2(averageVelocity.x, averageVelocity.z));
            }
        }

        /************************************************************************************************************************/
    }
}
