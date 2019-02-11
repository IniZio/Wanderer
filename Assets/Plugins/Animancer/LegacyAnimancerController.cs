// Animancer // Copyright 2018 Kybernetik //

using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.Playables;
using Object = UnityEngine.Object;

namespace Animancer
{
    /// <summary>
    /// An <see cref="AnimancerController"/> which uses the <see cref="Object.name"/>s of <see cref="AnimationClip"/>s
    /// to register them in its dictionary so they can be referenced using strings as well as the clips themselves.
    /// <para></para>
    /// It also has fields to automatically register animations on startup and play the first one automatically without
    /// needing another script to control it, much like Unity's Legacy <see cref="Animation"/> component.
    /// </summary>
    /// <remarks>
    /// Despite the word 'Legacy' in the name, there are no plans to deprecate this component in the forseeable future.
    /// The name was simply chosen for its similarities with Unity's Legacy <see cref="Animation"/> component.
    /// </remarks>
    [AddComponentMenu("Animancer/Legacy Animancer Controller")]
    [HelpURL(APIDocumentationURL + "/LegacyAnimancerController")]
    public class LegacyAnimancerController : AnimancerController
    {
        /************************************************************************************************************************/
        #region Fields and Properties
        /************************************************************************************************************************/

        [SerializeField]
        [Tooltip("If true, the first clip in the Animations array will be automatically played by OnEnable")]
        [Obfuscation(Exclude = false, Feature = "-rename")]
        private bool _PlayAutomatically = true;

        /// <summary>If true, the first clip in the <see cref="Animations"/> array will be automatically played by <see cref="OnEnable"/>.</summary>
        public bool PlayAutomatically
        {
            get { return _PlayAutomatically; }
            set { _PlayAutomatically = value; }
        }

        /************************************************************************************************************************/

        [SerializeField]
        [Tooltip("Animations in this array will be automatically registered by Awake")]
        [Obfuscation(Exclude = false, Feature = "-rename")]
        private AnimationClip[] _Animations;

        /// <summary>Animations in this array will be automatically registered by <see cref="Awake"/>.</summary>
        public AnimationClip[] Animations
        {
            get { return _Animations; }
            set
            {
                CreateStates(_Animations);
                _Animations = value;
            }
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Initialisation
        /************************************************************************************************************************/
#if UNITY_EDITOR
        /************************************************************************************************************************/

        /// <summary>[Editor-Only]
        /// Called by the Unity Editor in edit mode whenever an instance of this script is loaded or a value is changed
        /// in the inspector.
        /// <para></para>
        /// Uses <see cref="ClipState.ValidateClip"/> to ensure that all of the clips in the
        /// <see cref="Animations"/> array are supported by the <see cref="Animancer"/> system.
        /// </summary>
        protected virtual void OnValidate()
        {
            if (_Animations == null)
                return;

            for (int i = 0; i < _Animations.Length; i++)
            {
                var clip = _Animations[i];
                if (clip != null)
                    ClipState.ValidateClip(clip);
            }
        }

        /************************************************************************************************************************/
#endif
        /************************************************************************************************************************/

        /// <summary>
        /// Called by Unity when this component is being loaded.
        /// <para></para>
        /// Creates a state for each clip in the <see cref="Animations"/> array.
        /// </summary>
        protected virtual void Awake()
        {
            CreateStates(_Animations);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Called by Unity when this component becomes enabled and active.
        /// <para></para>
        /// Plays the first clip in the <see cref="Animations"/> array if <see cref="PlayAutomatically"/> is true.
        /// <para></para>
        /// Plays the <see cref="PlayableGraph"/> if it was stopped.
        /// </summary>
        protected override void OnEnable()
        {
            if (_PlayAutomatically && _Animations != null && _Animations.Length > 0)
            {
                var clip = _Animations[0];
                if (clip != null)
                {
                    Play(clip);
                    return;
                }
            }

            base.OnEnable();
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Play Management
        /************************************************************************************************************************/

        /// <summary>
        /// Returns the clip's name. This method is used to determine the dictionary key to use for an animation when
        /// none is specified by the user, such as in <see cref="AnimancerController.Play(AnimationClip)"/>.
        /// </summary>
        public override object GetKey(AnimationClip clip)
        {
            return clip.name;
        }

        /************************************************************************************************************************/

        /// <summary>[Coroutine]
        /// Plays each clip in the <see cref="Animations"/> array one after the other. Mainly useful for testing and
        /// showcasing purposes.
        /// </summary>
        public IEnumerator PlayAnimationsInSequence()
        {
            for (int i = 0; i < _Animations.Length; i++)
            {
                var state = Play(_Animations[i]);

                if (state != null)
                    yield return state;
            }

            StopAll();
        }

        /// <summary>[Pro-Only] [Coroutine]
        /// Cross fades between each clip in the <see cref="Animations"/> array one after the other. Mainly useful for
        /// testing and showcasing purposes.
        /// </summary>
#if !UNITY_EDITOR
        [Obsolete(AnimancerPlayable.ProOnlyMessage)]
#endif
        public IEnumerator CrossFadeAnimationsInSequence(float fadeDuration = AnimancerPlayable.DefaultFadeDuration)
        {
            for (int i = 0; i < _Animations.Length; i++)
            {
                var state = CrossFade(_Animations[i], fadeDuration);

                if (state != null)
                {
                    state.Time = 0;
                    yield return state;
                }
            }

            StopAll();
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
    }
}
