// Animancer // Copyright 2018 Kybernetik //

using UnityEngine;

namespace Animancer.Demo
{
    /// <summary>
    /// Base class for <see cref="MonoBehaviour"/> states to be used in a <see cref="StateMachine{TKey, TState}"/>.
    /// </summary>
    [HelpURL(AnimancerController.APIDocumentationURL + ".Demo/StateBehaviour_1")]
    public abstract class StateBehaviour<TKey> : MonoBehaviour, IState<TKey>
    {
        /************************************************************************************************************************/

        /// <summary>
        /// Determines whether the <see cref="StateMachine{TKey, TState}"/> can enter this state.
        /// Returns true when this component isn't enabled (unless overridden).
        /// </summary>
        public virtual bool CanEnterState(TKey previousState) { return !enabled; }

        /// <summary>
        /// Determines whether the <see cref="StateMachine{TKey, TState}"/> can exit this state. Always returns true
        /// unless overridden.
        /// </summary>
        public virtual bool CanExitState(TKey nextState) { return true; }

        /************************************************************************************************************************/

        /// <summary>
        /// Assert that this component isn't already enabled, then enable it.
        /// </summary>
        public virtual void OnEnterState()
        {
            Debug.Assert(!enabled, this + " was already enabled when entering its state", this);
            enabled = true;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Assert that this component isn't already disabled, then disable it.
        /// </summary>
        public virtual void OnExitState()
        {
            if (this == null) return;

            Debug.Assert(enabled, this + " was already disabled when exiting its state", this);
            enabled = false;
        }

        /************************************************************************************************************************/
    }
}
