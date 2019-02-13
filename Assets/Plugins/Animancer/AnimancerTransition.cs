// Animancer // Copyright 2018 Kybernetik //

using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

namespace Animancer
{
    /// <summary>
    /// Exposes an <see cref="AnimationClip"/> and various parameters to define how to play it.
    /// </summary>
    public interface IAnimancerTransition
    {
        /// <summary>The animation to play.</summary>
        AnimationClip Clip { get; }

        /// <summary>The index of the layer on which to play the animation.</summary>
        int LayerIndex { get; }

        /// <summary>The amount of time the transition should take.</summary>
        float FadeDuration { get; }

        /// <summary>Applies any additional modifications to the 'state'.</summary>
        void Apply(AnimancerState state);
    }

    /************************************************************************************************************************/

    /// <summary>
    /// A <see cref="ScriptableObject"/> which defines various details about how to play an animation so they can be
    /// configured in the Unity Editor instead of hard coded.
    /// <para></para>
    /// To trigger a transition, simply pass it as a parameter into the <see cref="AnimancerController"/> Play
    /// or CrossFade methods.
    /// </summary>
    [CreateAssetMenu(menuName = "Animancer Transition", order = 410)]// Group just under "Avatar Mask".
    public class AnimancerTransition : ScriptableObject, IAnimancerTransition
    {
        /************************************************************************************************************************/

        /// <summary>The animation to play.</summary>
        [Tooltip("The animation to play.")]
        public AnimationClip clip;

        /// <summary>The index of the layer on which to play the animation.</summary>
        [Tooltip("The index of the layer on which to play the animation.")]
        public int layerIndex;

        /// <summary>The amount of time the transition should take.</summary>
        [Tooltip("The amount of time the transition should take.")]
        public float fadeDuration = AnimancerPlayable.DefaultFadeDuration;

        /// <summary>The <see cref="AnimancerState.Time"/> at which to start the animation.</summary>
        [Tooltip("The AnimancerState.Time at which to start the animation.")]
        public float startTime;

        /// <summary>The <see cref="AnimancerState.Speed"/> at which to play the animation.</summary>
        [Tooltip("The AnimancerState.Speed at which to play the animation.")]
        public float speed = 1;

        /************************************************************************************************************************/

        AnimationClip IAnimancerTransition.Clip { get { return clip; } }

        int IAnimancerTransition.LayerIndex { get { return layerIndex; } }

        float IAnimancerTransition.FadeDuration { get { return fadeDuration; } }

        /************************************************************************************************************************/

        /// <summary>
        /// Applies the additional parameters of this transition to the 'state'. By default this is the
        /// <see cref="startTime"/> and <see cref="speed"/>, but this class could be inherited to add more parameters.
        /// </summary>
        public virtual void Apply(AnimancerState state)
        {
            state.Time = startTime;
            state.Speed = speed;
        }

        /************************************************************************************************************************/
    }
}
