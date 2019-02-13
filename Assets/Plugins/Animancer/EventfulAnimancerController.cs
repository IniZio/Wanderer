// Animancer // Copyright 2018 Kybernetik //

using System;
using System.Text;
using UnityEngine;

namespace Animancer
{
    /// <summary>
    /// Base class for <see cref="AnimancerController"/>s which need to do something whenever an animation is played.
    /// It adds an <see cref="OnBeforePlay"/> method and overrides all the usual Play and CrossFade methods to call it
    /// before performing their regular function.
    /// </summary>
    [HelpURL(APIDocumentationURL + "/EventfulAnimancerController")]
    public abstract class EventfulAnimancerController : AnimancerController
    {
        /************************************************************************************************************************/

        /// <summary>
        /// Called by all of the Play and CrossFade overloads.
        /// </summary>
        protected abstract void OnBeforePlay();

        /************************************************************************************************************************/

        /// <summary>
        /// Stops all other animations, plays the 'state', and returns it.
        /// <para></para>
        /// Note that if the state is already playing, it will continue doing so from the current time. If you wish to
        /// ensure that it starts from the beginning you can set the <see cref="AnimancerState.Time"/> of the returned
        /// state to 0.
        /// </summary>
        public override AnimancerState Play(AnimancerState state)
        {
            OnBeforePlay();
            return base.Play(state);
        }

        /************************************************************************************************************************/

        /// <summary>[Pro-Only]
        /// Fades out all other animations, fades in the animation registered with the 'key', and returns that state.
        /// If no state is registered with the 'key', this method does nothing and returns null.
        /// <para></para>
        /// Note that if the animation is already playing, it will continue doing so from the current time. If you wish
        /// to ensure that it starts from the beginning you should use
        /// <see cref="CrossFadeNew(object, float)"/> instead.
        /// </summary>
#if !UNITY_EDITOR
        [Obsolete(AnimancerPlayable.ProOnlyMessage)]
#endif
        public override AnimancerState CrossFade(object key, float fadeDuration = AnimancerPlayable.DefaultFadeDuration)
        {
            OnBeforePlay();
            return base.CrossFade(key, fadeDuration);
        }

        /// <summary>[Pro-Only]
        /// Starts fading all other animations to weight 0, starts fading the 'state' to weight 1, and returns it.
        /// </summary>
#if !UNITY_EDITOR
        [Obsolete(AnimancerPlayable.ProOnlyMessage)]
#endif
        public override AnimancerState CrossFade(AnimancerState state, float fadeDuration = AnimancerPlayable.DefaultFadeDuration)
        {
            OnBeforePlay();
            return base.CrossFade(state, fadeDuration);
        }

        /************************************************************************************************************************/

        /// <summary>[Pro-Only]
        /// Fades out all other animations, fades in the animation registered with the 'key' from the start of its
        /// clip, and returns that state. If no state is registered with the 'key', this method does nothing and
        /// returns null.
        /// <para></para>
        /// If the animation isn't currently at 0 weight, this method will actually fade it to 0 along with the others
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
        public override AnimancerState CrossFadeNew(object key, float fadeDuration = AnimancerPlayable.DefaultFadeDuration)
        {
            OnBeforePlay();
            return base.CrossFadeNew(key, fadeDuration);
        }

        /// <summary>[Pro-Only]
        /// Fades out all other animations, fades in the specified 'state' from the start of its clip, and returns it.
        /// <para></para>
        /// If the 'state' isn't currently at 0 weight, this method will actually fade it to 0 along with the others
        /// and create and return a new state with the same clip to fade to 1. This ensures that calling this method
        /// will always fade out from all current states and fade in from the start of the desired animation. States
        /// created for this purpose are cached so they can be reused in the future.
        /// <para></para>
        /// Calling this method repeatedly on subsequent frames will probably have undesirable effects; you most likely
        /// want to call <see cref="CrossFade(AnimancerState, float)"/> instead.
        /// </summary>
#if !UNITY_EDITOR
        [Obsolete(AnimancerPlayable.ProOnlyMessage)]
#endif
        public override AnimancerState CrossFadeNew(AnimancerState state, float fadeDuration = AnimancerPlayable.DefaultFadeDuration)
        {
            OnBeforePlay();
            return base.CrossFadeNew(state, fadeDuration);
        }

        /************************************************************************************************************************/

        /// <summary>[Editor-Conditional]
        /// Checks if any currently playing animation has an event with the specified 'functionName' and logs a warning
        /// if there is no such event. This method is only run in the Unity Editor because it is inefficient.
        /// </summary>
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public void EditorAssertRegisterEvent(Delegate oldEvent, Delegate newEvent, string functionName)
        {
#if UNITY_EDITOR
            if (newEvent == null)
                return;

            if (oldEvent != null)
            {
                var message = new StringBuilder();

                message.Append("Registering an event callback over the top of an existing one that wasn't cleared:" +
                    "\nOld Event Target=");
                message.Append(oldEvent.Target);
                message.Append(", Method=");
                message.Append(oldEvent.Method);
                message.Append("New Event Target=");
                message.Append(newEvent.Target);
                message.Append(", Method=");
                message.Append(newEvent.Method);
                message.Append("\n");
                Playable.AppendDescription(message);

                Debug.LogWarning(message);
                return;
            }

            if (IsPlayableInitialised)
            {
                foreach (var state in Playable)
                {
                    if (state.IsPlaying && state.HasEvent(functionName))
                        return;
                }
            }

            // If no event was found, log a message.
            {
                var message = new StringBuilder();
                message.Append("No Animation Event was found in any currently playing clip with the Function Name '");
                message.Append(functionName);
                message.Append("'\n");
                if (IsPlayableInitialised)
                    Playable.AppendDescription(message);
                Debug.LogWarning(message);
            }
#endif
        }

        /************************************************************************************************************************/
    }
}
