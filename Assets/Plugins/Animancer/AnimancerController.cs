// Animancer // Copyright 2018 Kybernetik //

using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.Playables;

namespace Animancer
{
    /// <summary>
    /// A substitute for a <see cref="RuntimeAnimatorController"/> which allows you to freely play animations on an
    /// <see cref="UnityEngine.Animator"/> without using the standard Mecanim state machine system.
    /// <para></para>
    /// This class can be used as a custom yield instruction to wait until all animations finish playing.
    /// </summary>
    /// <remarks>
    /// This class is mostly just a wrapper that connects an <see cref="AnimancerPlayable"/> to an
    /// <see cref="UnityEngine.Animator"/>.
    /// </remarks>
    [AddComponentMenu("Animancer/Animancer Controller")]
    [HelpURL(APIDocumentationURL + "/AnimancerController")]
    public class AnimancerController : MonoBehaviour, IAnimancer, IEnumerator
    {
        /************************************************************************************************************************/
        #region Fields and Properties
        /************************************************************************************************************************/

        /// <summary>The URL of the website where the Animancer documentation is hosted.</summary>
        public const string DocumentationURL = "https://animancer.github.io";

        /// <summary>The URL of the website where the Animancer API documentation is hosted.</summary>
        public const string APIDocumentationURL = DocumentationURL + "/api/Animancer";

        /************************************************************************************************************************/

        [SerializeField]
        [Tooltip("The Animator component which this script controls")]
        [Obfuscation(Exclude = false, Feature = "-rename")]
        private Animator _Animator;

        /// <summary>The <see cref="UnityEngine.Animator"/> component which this script controls.</summary>
        public Animator Animator
        {
            get { return _Animator; }
            set
            {
#if UNITY_EDITOR
                UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(_Animator, true);
                UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(value, false);
#endif

                // Disable the previous Animator so it stops playing the graph.
                if (_Playable != null && _Animator != null)
                    _Animator.enabled = false;

                _Animator = value;
                AnimancerPlayable.Play(_Animator, _Playable);
            }
        }

#if UNITY_EDITOR
        /// <summary>[Editor-Only] The name of the serialized backing field for the <see cref="Animator"/> property.</summary>
        string IAnimancer.AnimatorFieldName { get { return "_Animator"; } }
#endif

        /************************************************************************************************************************/

        private AnimancerPlayable _Playable;

        /// <summary>
        /// The internal system which manages the playing animations.
        /// Accessing this property will automatically initialise it.
        /// </summary>
        public AnimancerPlayable Playable
        {
            get
            {
                InitialisePlayable();
                return _Playable;
            }
        }

        /// <summary>Indicates whether the <see cref="Playable"/> has been initialised (is not null).</summary>
        public bool IsPlayableInitialised { get { return _Playable != null; } }

        /************************************************************************************************************************/
        #region Update Mode
        /************************************************************************************************************************/

        /// <summary>
        /// Determines when animations are updated and which time source is used. This property is mainly a wrapper
        /// around the <see cref="Animator.updateMode"/>.
        /// <para></para>
        /// Note that changing to or from <see cref="AnimatorUpdateMode.AnimatePhysics"/> at runtime has no effect.
        /// <para></para>
        /// Throws a <see cref="NullReferenceException"/> if no <see cref="Animator"/> is assigned.
        /// </summary>
        public AnimatorUpdateMode UpdateMode
        {
            get { return _Animator.updateMode; }
            set
            {
                _Animator.updateMode = value;

                // UnscaledTime on the Animator is actually identical to Normal when using the Playables API so we need
                // to set the graph's DirectorUpdateMode to determine how it gets its delta time.
                _Playable.UpdateMode = value == AnimatorUpdateMode.UnscaledTime ?
                    DirectorUpdateMode.UnscaledGameTime :
                    DirectorUpdateMode.GameTime;

#if UNITY_EDITOR
                if (InitialUpdateMode == null)
                {
                    InitialUpdateMode = value;
                }
                else if (UnityEditor.EditorApplication.isPlaying)
                {
                    if (AnimancerPlayable.HasChangedToOrFromAnimatePhysics(InitialUpdateMode, value))
                        Debug.LogWarning("Changing the Animator.updateMode to or from AnimatePhysics at runtime will have no effect." +
                            " You must set it in the Unity Editor or on startup.");
                }
#endif
            }
        }

        /************************************************************************************************************************/
#if UNITY_EDITOR
        /************************************************************************************************************************/

        /// <summary>[Editor-Only]
        /// The <see cref="UpdateMode"/> what was first used when this script initialised.
        /// This is used to give a warning when changing to or from <see cref="AnimatorUpdateMode.AnimatePhysics"/> at
        /// runtime since it won't work correctly.
        /// </summary>
        public AnimatorUpdateMode? InitialUpdateMode { get; private set; }

        /************************************************************************************************************************/
#endif
        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/

        /// <summary>
        /// The state of the animation currently being played.
        /// <para></para>
        /// Specifically, this is the state that was most recently started using any of the
        /// <see cref="AnimancerController"/> Play or CrossFade methods. States controlled individually will not
        /// register in this property.
        /// </summary>
        public AnimancerState CurrentState
        {
            get
            {
                if (_Playable != null)
                    return _Playable.CurrentState;
                else
                    return null;
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Gets or sets the state associated with the 'key'.
        /// <para></para>
        /// Get: if no state exists with the 'key', this method returns null.
        /// <para></para>
        /// Set: if a different state is already registered with the 'key', the existing one will be destroyed first.
        /// </summary>
        public AnimancerState this[object key]
        {
            get { return Playable[key]; }
            set { Playable[key] = value; }
        }

        /// <summary>
        /// Gets or sets the state associated with the 'clip' by using <see cref="GetKey"/> to determine its key.
        /// <para></para>
        /// Get: if no state exists with the key, this method returns null.
        /// <para></para>
        /// Set: if a different state is already registered with the key, the existing one will be destroyed first.
        /// </summary>
        public AnimancerState this[AnimationClip clip]
        {
            get { return this[GetKey(clip)]; }
            set { this[GetKey(clip)] = value; }
        }

        /************************************************************************************************************************/

        /// <summary>[Non-Serialized]
        /// If this is set to true, animations will be disconnected from the graph when they stop so that the
        /// <see cref="UnityEngine.Animator"/> stops writing the values of all their curves every frame.
        /// This disconnection has a significant performance cost, so use with caution.
        /// </summary>
        public bool DisconnectAnimationsOnStop
        {
            get
            {
                if (_Playable != null)
                    return _Playable.DisconnectAnimationsOnStop;
                else
                    return false;
            }
            set { Playable.DisconnectAnimationsOnStop = value; }
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Initialisation
        /************************************************************************************************************************/

#if UNITY_EDITOR
        /// <summary>[Editor-Only]
        /// Called by the Unity Editor when this component is first added (in edit mode) and whenever the Reset command
        /// is executed from its context menu.
        /// <para></para>
        /// Destroys the playable if one has been initialised.
        /// Searches for an <see cref="UnityEngine.Animator"/> on this object, or it's children or parents.
        /// Removes the <see cref="Animator.runtimeAnimatorController"/> if it finds one.
        /// <para></para>
        /// This method also contains editor-only code which prevents you from adding multiple copies of this component
        /// to a single object. Doing so will destroy the new one immediately and change the old one's type to match
        /// the new one, allowing you to change the type without losing the values of any serialized fields they share.
        /// </summary>
        protected virtual void Reset()
        {
            OnDestroy();

            if (_Animator == null)
            {
                _Animator = GetComponentInChildren<Animator>();
                if (_Animator == null)
                    _Animator = GetComponentInParent<Animator>();
            }

            if (_Animator != null)
            {
                _Animator.runtimeAnimatorController = null;
                UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(_Animator, true);
            }

            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            // If there is already another instance of this component on the same object, delete this new instance and
            // change the original's type to match this one.
            var components = GetComponents<AnimancerController>();
            if (components.Length > 1)
            {
                var oldComponent = components[0];
                var newComponent = components[1];

                if (oldComponent.GetType() != newComponent.GetType())
                {
                    // All we have to do is change the Script field to the new type and Unity will immediately deserialize
                    // the existing data as that type, so any fields shared between both types will keep their data.

                    using (var serializedObject = new UnityEditor.SerializedObject(oldComponent))
                    {
                        var scriptProperty = serializedObject.FindProperty("m_Script");
                        scriptProperty.objectReferenceValue = UnityEditor.MonoScript.FromMonoBehaviour(newComponent);
                        serializedObject.ApplyModifiedProperties();
                    }
                }

                // Destroy all components other than the first (the oldest).
                UnityEditor.EditorApplication.delayCall += () =>
                {
                    int i = 1;
                    for (; i < components.Length; i++)
                    {
                        DestroyImmediate(components[i]);
                    }
                };
            }
        }
#endif

        /************************************************************************************************************************/

        /// <summary>
        /// Called by Unity when this component becomes enabled and active.
        /// <para></para>
        /// Plays the <see cref="PlayableGraph"/> if it was stopped.
        /// </summary>
        protected virtual void OnEnable()
        {
            if (_Playable != null)
                _Playable.PlayGraph();
        }

        /// <summary>
        /// Called by Unity when this component becomes disabled or inactive.
        /// <para></para>
        /// Stops all currently playing animations and the <see cref="PlayableGraph"/> if it was playing.
        /// </summary>
        protected virtual void OnDisable()
        {
            StopAll();

            if (_Playable != null)
                _Playable.StopGraph();
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Creates a new playable if it doesn't already exist.
        /// </summary>
        private void InitialisePlayable()
        {
            if (_Playable != null)
                return;

            _Playable = AnimancerPlayable.CreatePlayable();

            if (_Animator != null)
            {
#if UNITY_EDITOR
                InitialUpdateMode = UpdateMode;
#endif

                if (UpdateMode == AnimatorUpdateMode.UnscaledTime)
                    _Playable.UpdateMode = DirectorUpdateMode.UnscaledGameTime;

                AnimancerPlayable.Play(_Animator, _Playable);
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Called by Unity when this component is destroyed. Ensures that the <see cref="Playable"/> is properly
        /// cleaned up.
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (_Playable != null)
            {
                _Playable.Dispose();
                _Playable = null;
            }
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region State Creation and Access
        /************************************************************************************************************************/

        /// <summary>
        /// Returns the 'clip' itself. This method is used to determine the dictionary key to use for an animation
        /// when none is specified by the user, such as in <see cref="Play(AnimationClip)"/>. It can be overridden by
        /// child classes to use something else as the key.
        /// </summary>
        public virtual object GetKey(AnimationClip clip)
        {
            return clip;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Creates and returns a new <see cref="ClipState"/> to play the 'clip'.
        /// </summary>
        public ClipState CreateState(AnimationClip clip, int layerIndex = 0)
        {
            return CreateState(GetKey(clip), clip, layerIndex);
        }

        /// <summary>
        /// Creates and returns a new <see cref="ClipState"/> to play the 'clip' and registers it with the 'key'.
        /// </summary>
        public ClipState CreateState(object key, AnimationClip clip, int layerIndex = 0)
        {
            return Playable.CreateState(key, clip, layerIndex);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Registers the 'state' so it can be accessed later on using the 'key'.
        /// </summary>
        public void AddState(object key, AnimancerState state)
        {
            Playable.AddState(key, state);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Returns the state associated with the 'key', or null if none exists.
        /// </summary>
        public AnimancerState GetState(object key)
        {
            if (_Playable != null)
                return _Playable.GetState(key);
            else
                return null;
        }

        /// <summary>
        /// If a state is registered with the 'key', this method outputs it as the 'state' and returns true. Otherwise
        /// 'state' is set to null and this method returns false.
        /// </summary>
        public bool TryGetState(object key, out AnimancerState state)
        {
            if (_Playable != null)
            {
                return _Playable.TryGetState(key, out state);
            }
            else
            {
                state = null;
                return false;
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Determines a key for the clip using the <see cref="GetKey"/> method (which uses the 'clip' itself unless
        /// that method is overridden).
        /// If a state is registered with that key, this method verifies that it has the correct 'clip' and returns
        /// it. If the <see cref="AnimancerState.Clip"/> doesn't match the specified 'clip', an exception is thrown. If
        /// no state was registered, this method creates a new one with the 'clip', registers it with the key, and
        /// returns it.
        /// <para></para>
        /// If you wish to change the registered <see cref="AnimancerState.Clip"/> to match the specified 'clip', you
        /// can call <see cref="ForceSetClip"/> instead.
        /// </summary>
        public AnimancerState GetOrCreateState(AnimationClip clip, int layerIndex = 0)
        {
            if (clip != null)
                return GetOrCreateState(GetKey(clip), clip, layerIndex);
            else
                return null;
        }

        /// <summary>
        /// If a state is registered with the 'key', this method verifies that it has the correct 'clip' and returns
        /// it. If the <see cref="AnimancerState.Clip"/> doesn't match the specified 'clip', an exception is thrown. If
        /// no state was registered, this method creates a new one with the 'clip', registers it with the 'key', and
        /// returns it.
        /// <para></para>
        /// If you wish to change the registered <see cref="AnimancerState.Clip"/> to match the specified 'clip', you
        /// can call <see cref="ForceSetClip"/> instead.
        /// </summary>
        public AnimancerState GetOrCreateState(object key, AnimationClip clip, int layerIndex = 0)
        {
            return Playable.GetOrCreateState(key, clip, layerIndex);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Calls <see cref="GetOrCreateState(AnimationClip)"/> for each of the specified clips.
        /// </summary>
        public void CreateStates(AnimationClip clip0, AnimationClip clip1, int layerIndex = 0)
        {
            GetOrCreateState(clip0, layerIndex);
            GetOrCreateState(clip1, layerIndex);
        }

        /// <summary>
        /// Calls <see cref="GetOrCreateState(AnimationClip)"/> for each of the specified clips.
        /// </summary>
        public void CreateStates(AnimationClip clip0, AnimationClip clip1, AnimationClip clip2, int layerIndex = 0)
        {
            GetOrCreateState(clip0, layerIndex);
            GetOrCreateState(clip1, layerIndex);
            GetOrCreateState(clip2, layerIndex);
        }

        /// <summary>
        /// Calls <see cref="GetOrCreateState(AnimationClip)"/> for each of the specified clips.
        /// </summary>
        public void CreateStates(AnimationClip clip0, AnimationClip clip1, AnimationClip clip2, AnimationClip clip3, int layerIndex = 0)
        {
            GetOrCreateState(clip0, layerIndex);
            GetOrCreateState(clip1, layerIndex);
            GetOrCreateState(clip2, layerIndex);
            GetOrCreateState(clip3, layerIndex);
        }

        /// <summary>
        /// Calls <see cref="GetOrCreateState(AnimationClip)"/> for each of the specified clips.
        /// </summary>
        public void CreateStates(params AnimationClip[] clips)
        {
            CreateStates(0, clips);
        }

        /// <summary>
        /// Calls <see cref="GetOrCreateState(AnimationClip)"/> for each of the specified clips.
        /// </summary>
        public void CreateStates(int layerIndex, params AnimationClip[] clips)
        {
            if (clips == null)
                return;

            var count = clips.Length;
            for (int i = 0; i < count; i++)
            {
                var clip = clips[i];
                if (clip != null)
                    GetOrCreateState(clip, layerIndex);
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// If a state is registered with the 'key', this method verifies that it has the correct 'clip' and returns
        /// it. If the <see cref="AnimancerState.Clip"/> doesn't match the specified 'clip', the state is destroyed and
        /// a new one is created in its place with the correct 'clip'. A new one is also created if no existing state
        /// was registered with the 'key'.
        /// <para></para>
        /// Note that destroying and creating states is more computationally expensive than most other operations and
        /// should be avoided at performance intensive times. Consider using <see cref="GetOrCreateState"/> instead,
        /// which will throw an exception instead of replacing the state.
        /// </summary>
        public AnimancerState ForceSetClip(object key, AnimationClip clip, int layerIndex = 0)
        {
            return Playable.ForceSetClip(key, clip, layerIndex);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Throws an exception if the <see cref="AnimancerState.Parent"/> is doesn't match the root <see cref="AnimancerPlayable"/>.
        /// </summary>
        public void ValidateState(AnimancerState state)
        {
            if (state.Parent.Root != _Playable)
                throw new ArgumentException("State ownership mismatch: you are attempting to use a state in an AnimancerPlayable that isn't it's parent.");
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Calls <see cref="GetKey"/> then passes the key to <see cref="DestroyState(object)"/> and
        /// returns the result.
        /// </summary>
        public bool DestroyState(AnimationClip clip)
        {
            return DestroyState(GetKey(clip));
        }

        /// <summary>
        /// If a state is registered with the 'key', this method destroys it and returns true.
        /// </summary>
        public bool DestroyState(object key)
        {
            if (_Playable != null)
                return _Playable.DisposeState(key);
            else
                return false;
        }

        /************************************************************************************************************************/

        /// <summary>Returns layer 0 of the <see cref="Playable"/>.</summary>
        public static implicit operator AnimancerLayer(AnimancerController animancer)
        {
            return animancer.Playable;
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Layers
        /************************************************************************************************************************/

        /// <summary>[Pro-Only]
        /// The number of animation layers in the graph.
        /// </summary>
#if !UNITY_EDITOR
        [Obsolete(AnimancerPlayable.ProOnlyMessage)]
#endif
        public int LayerCount
        {
            get
            {
                if (_Playable != null)
                    return _Playable.LayerCount;
                else
                    return 1;
            }
            set { Playable.LayerCount = value; }
        }

        /************************************************************************************************************************/

        /// <summary>[Pro-Only]
        /// If the <see cref="LayerCount"/> is below the specified 'min', this method sets it to that value.
        /// </summary>
#if !UNITY_EDITOR
        [Obsolete(AnimancerPlayable.ProOnlyMessage)]
#endif
        public void SetMinLayerCount(int min)
        {
            Playable.SetMinLayerCount(min);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Returns the layer at the specified index.
        /// </summary>
        public AnimancerLayer GetLayer(int layerIndex)
        {
            return Playable.GetLayer(layerIndex);
        }

        /************************************************************************************************************************/

        /// <summary>[Pro-Only]
        /// Creates and returns a new <see cref="AnimancerLayer"/>. New layers are set to override earlier layers by
        /// default.
        /// </summary>
#if !UNITY_EDITOR
        [Obsolete(AnimancerPlayable.ProOnlyMessage)]
#endif
        public AnimancerLayer AddLayer()
        {
            return Playable.AddLayer();
        }

        /************************************************************************************************************************/

        /// <summary>[Pro-Only]
        /// Checks whether the layer at the specified index is set to additive blending. Otherwise it will override any
        /// earlier layers.
        /// </summary>
#if !UNITY_EDITOR
        [Obsolete(AnimancerPlayable.ProOnlyMessage)]
#endif
        public bool IsLayerAdditive(int layerIndex)
        {
            return Playable.IsLayerAdditive(layerIndex);
        }

        /// <summary>[Pro-Only]
        /// Sets the layer at the specified index to blend additively with earlier layers (if true) or to override them
        /// (if false). Newly created layers will override by default.
        /// </summary>
#if !UNITY_EDITOR
        [Obsolete(AnimancerPlayable.ProOnlyMessage)]
#endif
        public void SetLayerAdditive(int layerIndex, bool value)
        {
            Playable.SetLayerAdditive(layerIndex, value);
        }

        /************************************************************************************************************************/

        /// <summary>[Pro-Only]
        /// Sets an <see cref="AvatarMask"/> to determine which bones the layer at the specified index will affect.
        /// </summary>
#if !UNITY_EDITOR
        [Obsolete(AnimancerPlayable.ProOnlyMessage)]
#endif
        public void SetLayerMask(int layerIndex, AvatarMask mask)
        {
            Playable.SetLayerMask(layerIndex, mask);
        }

        /************************************************************************************************************************/

        /// <summary>[Editor-Conditional]
        /// Sets the inspector display name of the layer at the specified index. Note that layer names are Editor-Only
        /// so any calls to this method will automatically be compiled out of a runtime build.
        /// </summary>
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public void SetLayerName(int layerIndex, string name)
        {
            Playable.SetLayerName(layerIndex, name);
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Play Management
        /************************************************************************************************************************/

        /// <summary>
        /// Stops all other animations, plays the 'clip' from the start, and returns its state.
        /// </summary>
        public AnimancerState Play(AnimationClip clip, int layerIndex = 0)
        {
            var state = GetOrCreateState(GetKey(clip), clip, layerIndex);
            return Play(state);
        }

        /// <summary>
        /// Stops all other animations, plays the animation registered with the 'key' from the start, and returns that
        /// state. If no state is registered with the 'key', this method does nothing and returns null.
        /// </summary>
        public virtual AnimancerState Play(object key)
        {
            _Animator.enabled = true;
            return Playable.Play(key);
        }

        /// <summary>
        /// Stops all other animations, plays the 'state' from the start, and returns it.
        /// </summary>
        public virtual AnimancerState Play(AnimancerState state)
        {
            _Animator.enabled = true;
            return Playable.Play(state);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Stops all other animations, plays the 'transition.clip' from the start, and returns its state.
        /// </summary>
        public AnimancerState Play(IAnimancerTransition transition)
        {
            var state = Play(transition.Clip, transition.LayerIndex);
            transition.Apply(state);
            return state;
        }

        /************************************************************************************************************************/

        /// <summary>[Pro-Only]
        /// Fades out all other animations, fades in the 'clip', and returns its state.
        /// <para></para>
        /// Note that if the 'state' is already playing, it will continue doing so from its current time. To play it
        /// from the start you can use <see cref="CrossFadeNew(AnimationClip, float)"/> instead or set the
        /// <see cref="AnimancerState.Time"/> to 0.
        /// </summary>
#if !UNITY_EDITOR
        [Obsolete(AnimancerPlayable.ProOnlyMessage)]
#endif
        public AnimancerState CrossFade(AnimationClip clip, float fadeDuration = AnimancerPlayable.DefaultFadeDuration, int layerIndex = 0)
        {
            var state = GetOrCreateState(GetKey(clip), clip, layerIndex);
            return CrossFade(state, fadeDuration);
        }

        /// <summary>[Pro-Only]
        /// Fades out all other animations, fades in the animation registered with the 'key', and returns that state.
        /// If no state is registered with the 'key', this method does nothing and returns null.
        /// <para></para>
        /// Note that if the 'state' is already playing, it will continue doing so from its current time. To play it
        /// from the start you can use <see cref="CrossFadeNew(object, float)"/> instead or set the
        /// <see cref="AnimancerState.Time"/> to 0.
        /// </summary>
#if !UNITY_EDITOR
        [Obsolete(AnimancerPlayable.ProOnlyMessage)]
#endif
        public virtual AnimancerState CrossFade(object key, float fadeDuration = AnimancerPlayable.DefaultFadeDuration)
        {
            _Animator.enabled = true;
            return Playable.CrossFade(key, fadeDuration);
        }

        /// <summary>[Pro-Only]
        /// Fades out all other animations, fades in the specified 'state', and returns it.
        /// <para></para>
        /// Note that if the 'state' is already playing, it will continue doing so from its current time. To play it
        /// from the start you can use <see cref="CrossFadeNew(AnimancerState, float)"/> instead or set the
        /// <see cref="AnimancerState.Time"/> to 0.
        /// </summary>
#if !UNITY_EDITOR
        [Obsolete(AnimancerPlayable.ProOnlyMessage)]
#endif
        public virtual AnimancerState CrossFade(AnimancerState state, float fadeDuration = AnimancerPlayable.DefaultFadeDuration)
        {
            _Animator.enabled = true;
            return Playable.CrossFade(state, fadeDuration);
        }

        /************************************************************************************************************************/

        /// <summary>[Pro-Only]
        /// Fades out all other animations, fades in the 'transition.clip', and returns its state.
        /// <para></para>
        /// Note that if the 'state' is already playing, it will continue doing so from its current time. To play it
        /// from the start you can use <see cref="CrossFadeNew(IAnimancerTransition)"/> instead or set the
        /// <see cref="AnimancerState.Time"/> to 0.
        /// </summary>
#if !UNITY_EDITOR
        [Obsolete(AnimancerPlayable.ProOnlyMessage)]
#endif
        public AnimancerState CrossFade(IAnimancerTransition transition)
        {
            var state = CrossFade(transition.Clip, transition.FadeDuration, transition.LayerIndex);
            transition.Apply(state);
            return state;
        }

        /************************************************************************************************************************/

        /// <summary>[Pro-Only]
        /// Fades out all other animations, fades in the specified 'clip' from the start, and returns it.
        /// <para></para>
        /// If the 'clip' isn't currently at 0 weight, this method will actually fade it to 0 along with the others
        /// and create and return a new state with the same clip to fade to 1. This ensures that calling this method
        /// will always fade out from all current states and fade in from the start of the desired animation. States
        /// created for this purpose are cached so they can be reused in the future.
        /// <para></para>
        /// Calling this method repeatedly on consecutive frames will probably have undesirable effects; you most likely
        /// want to call <see cref="CrossFade(object, float)"/> instead.
        /// </summary>
#if !UNITY_EDITOR
        [Obsolete(AnimancerPlayable.ProOnlyMessage)]
#endif
        public AnimancerState CrossFadeNew(AnimationClip clip, float fadeDuration = AnimancerPlayable.DefaultFadeDuration, int layerIndex = 0)
        {
            var state = GetOrCreateState(GetKey(clip), clip, layerIndex);
            return CrossFadeNew(state, fadeDuration);
        }

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
        /// Calling this method repeatedly on consecutive frames will probably have undesirable effects; you most likely
        /// want to call <see cref="CrossFade(object, float)"/> instead.
        /// </summary>
#if !UNITY_EDITOR
        [Obsolete(AnimancerPlayable.ProOnlyMessage)]
#endif
        public virtual AnimancerState CrossFadeNew(object key, float fadeDuration = AnimancerPlayable.DefaultFadeDuration)
        {
            _Animator.enabled = true;
            return Playable.CrossFadeNew(key, fadeDuration);
        }

        /// <summary>[Pro-Only]
        /// Fades out all other animations, fades in the specified 'state' from the start of its clip, and returns it.
        /// <para></para>
        /// If the 'state' isn't currently at 0 weight, this method will actually fade it to 0 along with the others
        /// and create and return a new state with the same clip to fade to 1. This ensures that calling this method
        /// will always fade out from all current states and fade in from the start of the desired animation. States
        /// created for this purpose are cached so they can be reused in the future.
        /// <para></para>
        /// Calling this method repeatedly on consecutive frames will probably have undesirable effects; you most likely
        /// want to call <see cref="CrossFade(AnimancerState, float)"/> instead.
        /// </summary>
#if !UNITY_EDITOR
        [Obsolete(AnimancerPlayable.ProOnlyMessage)]
#endif
        public virtual AnimancerState CrossFadeNew(AnimancerState state, float fadeDuration = AnimancerPlayable.DefaultFadeDuration)
        {
            _Animator.enabled = true;
            return Playable.CrossFadeNew(state, fadeDuration);
        }

        /************************************************************************************************************************/

        /// <summary>[Pro-Only]
        /// Fades out all other animations, fades in the specified 'transition.clip' from the start, and returns it.
        /// <para></para>
        /// If the 'transition.clip' isn't currently at 0 weight, this method will actually fade it to 0 along with the others
        /// and create and return a new state with the same clip to fade to 1. This ensures that calling this method
        /// will always fade out from all current states and fade in from the start of the desired animation. States
        /// created for this purpose are cached so they can be reused in the future.
        /// <para></para>
        /// Calling this method repeatedly on consecutive frames will probably have undesirable effects; you most likely
        /// want to call <see cref="CrossFade(IAnimancerTransition)"/> instead.
        /// </summary>
#if !UNITY_EDITOR
        [Obsolete(AnimancerPlayable.ProOnlyMessage)]
#endif
        public AnimancerState CrossFadeNew(IAnimancerTransition transition)
        {
            var state = CrossFadeNew(transition.Clip, transition.FadeDuration, transition.LayerIndex);
            transition.Apply(state);
            return state;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Calls <see cref="AnimancerState.Pause"/> on the 'clip' to freeze it at its current time and returns the
        /// state (if it exists).
        /// </summary>
        public AnimancerState Pause(AnimationClip clip)
        {
            return Pause(GetKey(clip));
        }

        /// <summary>
        /// Calls <see cref="AnimancerState.Pause"/> on the state registered with the 'key' to freeze it at its current
        /// time and returns the state (if it exists).
        /// </summary>
        public AnimancerState Pause(object key)
        {
            if (_Playable != null)
                return _Playable.Pause(key);
            else
                return null;
        }

        /// <summary>
        /// Calls <see cref="AnimancerState.Pause"/> on all animations in the specified layer to freeze them at their
        /// current time.
        /// </summary>
        public void PauseAll(int layerIndex)
        {
            if (_Playable != null)
                _Playable.PauseAll(layerIndex);
        }

        /// <summary>
        /// Calls <see cref="AnimancerState.Pause"/> on all animations to freeze them at their current time.
        /// </summary>
        public void PauseAll()
        {
            if (_Playable != null)
                _Playable.PauseAll();
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Calls <see cref="AnimancerState.Resume"/> on the 'clip' to continue playing from its current time and
        /// returns the state (if it exists).
        /// </summary>
        public AnimancerState Resume(AnimationClip clip, int layerIndex = 0)
        {
            var state = GetOrCreateState(GetKey(clip), clip, layerIndex);
            state.Resume();
            return state;
        }

        /// <summary>
        /// Calls <see cref="AnimancerState.Resume"/> on the state registered with the 'key' to continue playing from
        /// its current time and returns the state (if it exists).
        /// </summary>
        public AnimancerState Resume(object key)
        {
            _Animator.enabled = true;
            return Playable.Resume(key);
        }

        /// <summary>
        /// Calls <see cref="AnimancerState.Resume"/> on all animations in the specified layer with any weight to
        /// continue playing from their current time.
        /// </summary>
        public void ResumeAll(int layerIndex)
        {
            if (_Playable != null)
                _Playable.ResumeAll(layerIndex);
        }

        /// <summary>
        /// Calls <see cref="AnimancerState.Resume"/> on all animations with any weight to continue playing from their
        /// current time.
        /// </summary>
        public void ResumeAll()
        {
            if (_Playable != null)
                _Playable.ResumeAll();
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Calls <see cref="AnimancerState.Stop"/> on the 'clip' to stop it from playing and rewind it to the start.
        /// </summary>
        public AnimancerState Stop(AnimationClip clip)
        {
            return Stop(GetKey(clip));
        }

        /// <summary>
        /// Calls <see cref="AnimancerState.Stop"/> on the state registered with the 'key' to stop it from playing and
        /// rewind it to the start.
        /// </summary>
        public AnimancerState Stop(object key)
        {
            if (_Playable != null)
                return _Playable.Stop(key);
            else
                return null;
        }

        /// <summary>
        /// Calls <see cref="AnimancerState.Stop"/> on all animations in the specified layer to stop them from playing
        /// and rewind them to the start.
        /// </summary>
        public void StopAll(int layerIndex)
        {
            if (_Playable != null)
                _Playable.StopAll(layerIndex);
        }

        /// <summary>
        /// Calls <see cref="AnimancerState.Stop"/> on all animations to stop them from playing and rewind them to the
        /// start.
        /// </summary>
        public void StopAll()
        {
            if (_Playable != null)
                _Playable.StopAll();
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Returns true if the 'clip' is currently being played by at least one state.
        /// </summary>
        public bool IsPlayingClip(AnimationClip clip)
        {
            if (_Playable != null)
                return _Playable.IsPlayingClip(clip);
            else
                return false;
        }

        /// <summary>
        /// Returns true if the 'clip' is currently being played by at least one state.
        /// </summary>
        public bool IsPlayingClip(AnimationClip clip, int layerIndex)
        {
            if (_Playable != null)
                return _Playable.IsPlayingClip(clip, layerIndex);
            else
                return false;
        }

        /// <summary>
        /// Returns true if a state is registered with the 'key' and it is currently playing.
        /// </summary>
        public bool IsPlaying(object key)
        {
            AnimancerState state;

            return
                TryGetState(key, out state) &&
                state.IsPlaying;
        }

        /// <summary>
        /// Returns true if at least one animation is being played.
        /// </summary>
        public bool IsPlaying()
        {
            if (_Playable != null)
                return _Playable.IsPlaying();
            else
                return false;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Evaluates all of the currently playing animations to apply their states to the animated objects.
        /// </summary>
        public void Evaluate()
        {
            if (_Playable != null)
                _Playable.Evaluate();
        }

        /// <summary>
        /// Advances all currently playing animations by the specified amount of time (in seconds) and evaluates the
        /// graph to apply their states to the animated objects.
        /// </summary>
        public void Evaluate(float deltaTime)
        {
            if (_Playable != null)
                _Playable.Evaluate(deltaTime);
        }

        /************************************************************************************************************************/
        #region Key Error Methods
#if UNITY_EDITOR
        /************************************************************************************************************************/
        // These are overloads of other methods that take a System.Object key to ensure the user doesn't try to use an
        // AnimancerState as a key, since the whole point of a key is to identify a state in the first place.
        /************************************************************************************************************************/
        // State Creation and Access.
        /************************************************************************************************************************/

        /// <summary>[Warning] You cannot use an AnimancerState as a key. The whole point of a key is to identify a state in the first place.</summary>
        [Obsolete("You cannot use an AnimancerState as a key. The whole point of a key is to identify a state in the first place.", true)]
        public AnimancerState GetState(AnimancerState key)
        {
            return key;
        }

        /// <summary>[Warning] You cannot use an AnimancerState as a key. The whole point of a key is to identify a state in the first place.</summary>
        [Obsolete("You cannot use an AnimancerState as a key. The whole point of a key is to identify a state in the first place.", true)]
        public bool TryGetState(AnimancerState key, out AnimancerState state)
        {
            state = key;
            return true;
        }

        /// <summary>[Warning] You cannot use an AnimancerState as a key. The whole point of a key is to identify a state in the first place.</summary>
        [Obsolete("You cannot use an AnimancerState as a key. The whole point of a key is to identify a state in the first place.", true)]
        public AnimancerState GetOrCreateState(AnimancerState key, AnimationClip clip, int layerIndex = 0)
        {
            return key;
        }

        /// <summary>[Warning] You cannot use an AnimancerState as a key. To destroy a state you must specify the key used to register it, not the state itself.</summary>
        [Obsolete("You cannot use an AnimancerState as a key. To destroy a state you must specify the key used to register it, not the state itself.", true)]
        public bool DestroyState(AnimancerState key)
        {
            throw new InvalidOperationException("You cannot use an AnimancerState as a key. To destroy a state you must specify the key used to register it, not the state itself.");
        }

        /************************************************************************************************************************/
        // Play Management.
        /************************************************************************************************************************/

        /// <summary>[Warning] You cannot use an AnimancerState as a key. Just call Pause() on the state itself.</summary>
        [Obsolete("You cannot use an AnimancerState as a key. Just call Pause() on the state itself.", true)]
        public AnimancerState Pause(AnimancerState key)
        {
            key.Pause();
            return key;
        }

        /// <summary>[Warning] You cannot use an AnimancerState as a key. Just call Resume() on the state itself.</summary>
        [Obsolete("You cannot use an AnimancerState as a key. Just call Resume() on the state itself.", true)]
        public AnimancerState Resume(AnimancerState key)
        {
            key.Resume();
            return key;
        }

        /// <summary>[Warning] You cannot use an AnimancerState as a key. Just call Stop() on the state itself.</summary>
        [Obsolete("You cannot use an AnimancerState as a key. Just call Stop() on the state itself.", true)]
        public AnimancerState Stop(AnimancerState key)
        {
            key.Stop();
            return key;
        }

        /// <summary>[Warning] You cannot use an AnimancerState as a key. Just check IsPlaying on the state itself.</summary>
        [Obsolete("You cannot use an AnimancerState as a key. Just check IsPlaying on the state itself.", true)]
        public bool IsPlaying(AnimancerState key)
        {
            return key.IsPlaying;
        }

        /************************************************************************************************************************/
#endif
        #endregion
        /************************************************************************************************************************/
        // IEnumerator.
        /************************************************************************************************************************/

        /// <summary>Returns <see cref="IsPlaying()"/> so this object can be used as a custom yield instruction to wait until all animations finish playing.</summary>
        bool IEnumerator.MoveNext()
        {
            return IsPlaying();
        }

        /// <summary>Returns null.</summary>
        object IEnumerator.Current { get { return null; } }

        /// <summary>Does nothing.</summary>
        void IEnumerator.Reset() { }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
    }
}
