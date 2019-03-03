// Animancer // Copyright 2018 Kybernetik //

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animancer.Demo
{
    /// <summary>
    /// A state that can be used in a <see cref="StateMachine{TKey, TState}"/>.
    /// </summary>
    public interface IState<TKey>
    {
        bool CanEnterState(TKey previousState);
        bool CanExitState(TKey nextState);
        void OnEnterState();
        void OnExitState();
    }

    /************************************************************************************************************************/

    /// <summary>
    /// A simple Finite State Machine system.
    /// </summary>
    [HelpURL(AnimancerController.APIDocumentationURL + ".Demo/StateMachine_2")]
    public partial class StateMachine<TKey, TState> : IDictionary<TKey, TState>
        where TState : class, IState<TKey>
    {
        /************************************************************************************************************************/

        public IDictionary<TKey, TState> Dictionary { get; set; }
        public TKey CurrentKey { get; private set; }
        public TState CurrentState { get; private set; }

        /************************************************************************************************************************/

        /// <summary>
        /// Constructs a new <see cref="StateMachine{TKey, TState}"/> with a new <see cref="Dictionary"/>.
        /// </summary>
        public StateMachine()
        {
            Dictionary = new Dictionary<TKey, TState>();
        }

        /// <summary>
        /// Constructs a new <see cref="StateMachine{TKey, TState}"/> with a new <see cref="Dictionary"/> that starts
        /// with the specified 'capacity'.
        /// </summary>
        public StateMachine(int capacity)
        {
            Dictionary = new Dictionary<TKey, TState>(capacity);
        }

        /// <summary>
        /// Constructs a new <see cref="StateMachine{TKey, TState}"/> with a new <see cref="Dictionary"/> that uses the
        /// specified 'comparer'.
        /// </summary>
        public StateMachine(IEqualityComparer<TKey> comparer)
        {
            Dictionary = new Dictionary<TKey, TState>(comparer);
        }

        /// <summary>
        /// Constructs a new <see cref="StateMachine{TKey, TState}"/> with a new <see cref="Dictionary"/> that starts
        /// with the specified 'capacity' and uses the specified 'comparer'.
        /// </summary>
        public StateMachine(int capacity, IEqualityComparer<TKey> comparer)
        {
            Dictionary = new Dictionary<TKey, TState>(capacity, comparer);
        }

        /// <summary>
        /// Constructs a new <see cref="StateMachine{TKey, TState}"/> which uses the specified 'dictionary'.
        /// </summary>
        public StateMachine(IDictionary<TKey, TState> dictionary)
        {
            Dictionary = dictionary;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Attempts to enter the specified state and returns true if successful.
        /// </summary>
        public bool TrySetState(TKey key, TState state)
        {
            if (CurrentState != null && !CurrentState.CanExitState(key))
                return false;

            if (state == null || !state.CanEnterState(CurrentKey))
                return false;

            ForceSetState(key, state);
            return true;
        }

        /// <summary>
        /// Attempts to enter the specified state (if there is one associated with the 'key') and returns true if
        /// successful.
        /// </summary>
        public TState TrySetState(TKey key)
        {
            TState state;
            if (Dictionary.TryGetValue(key, out state))
            {
                if (TrySetState(key, state))
                    return state;
            }

            return null;
        }

        /// <summary>
        /// Attempts to enter the specified state and returns true if successful.
        /// Keeps the current key.
        /// </summary>
        public bool TrySetState(TState state)
        {
            return TrySetState(CurrentKey, state);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Calls <see cref="IState{TKey}.OnExitState"/> on the current state then changes to the specified key and
        /// state and calls <see cref="IState{TKey}.OnEnterState"/> on it.
        /// <para></para>
        /// Note that this method does not check <see cref="IState{TKey}.CanExitState(TKey)"/> or
        /// <see cref="IState{TKey}.CanEnterState(TKey)"/>. To do that, you should use
        /// <see cref="TrySetState(TKey, TState)"/> instead.
        /// </summary>
        public void ForceSetState(TKey key, TState state)
        {
            if (CurrentState != null)
                CurrentState.OnExitState();

            CurrentKey = key;
            CurrentState = state;

            if (CurrentState != null)
                CurrentState.OnEnterState();
        }

        /// <summary>
        /// Uses <see cref="ForceSetState(TKey, TState)"/> to change to the state mapped to the 'key'. If nothing is mapped,
        /// it changes to default(TState).
        /// </summary>
        public TState ForceSetState(TKey key)
        {
            TState state;
            Dictionary.TryGetValue(key, out state);
            ForceSetState(key, state);
            return state;
        }

        /// <summary>
        /// Calls <see cref="IState{TKey}.OnExitState"/> on the current state then changes to the specified
        /// state and calls <see cref="IState{TKey}.OnEnterState"/> on it.
        /// </summary>
        public void ForceSetState(TState state)
        {
            ForceSetState(CurrentKey, state);
        }

        /************************************************************************************************************************/
        #region Dictionary Wrappers
        /************************************************************************************************************************/

        public TState this[TKey key] { get { return Dictionary[key]; } set { Dictionary[key] = value; } }

        public ICollection<TKey> Keys { get { return Dictionary.Keys; } }
        public ICollection<TState> Values { get { return Dictionary.Values; } }
        public int Count { get { return Dictionary.Count; } }
        public bool IsReadOnly { get { return Dictionary.IsReadOnly; } }

        public void Add(TKey key, TState state) { Dictionary.Add(key, state); }
        public bool Remove(TKey key) { return Dictionary.Remove(key); }
        public void Clear() { Dictionary.Clear(); }
        public bool ContainsKey(TKey key) { return Dictionary.ContainsKey(key); }
        public bool TryGetValue(TKey key, out TState state) { return Dictionary.TryGetValue(key, out state); }

        public void Add(KeyValuePair<TKey, TState> item) { Dictionary.Add(item); }
        public bool Remove(KeyValuePair<TKey, TState> item) { return Dictionary.Remove(item); }
        public bool Contains(KeyValuePair<TKey, TState> item) { return Dictionary.Contains(item); }
        public IEnumerator<KeyValuePair<TKey, TState>> GetEnumerator() { return Dictionary.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return Dictionary.GetEnumerator(); }
        public void CopyTo(KeyValuePair<TKey, TState>[] array, int arrayIndex) { Dictionary.CopyTo(array, arrayIndex); }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/

        /// <summary>
        /// Returns the state associated with the specified 'key', or null if none is present.
        /// </summary>
        public TState GetState(TKey key)
        {
            TState state;
            TryGetValue(key, out state);
            return state;
        }

        /************************************************************************************************************************/

        /// <summary>Adds the specified 'keys' and 'states'. Both arrays must be the same size.</summary>
        public void AddRange(TKey[] keys, TState[] states)
        {
            Debug.Assert(keys.Length == states.Length);

            for (int i = 0; i < keys.Length; i++)
            {
                Dictionary.Add(keys[i], states[i]);
            }
        }

        /************************************************************************************************************************/

        /// <summary>Sets the <see cref="CurrentKey"/> without actually changing the state.</summary>
        public void SetFakeKey(TKey key)
        {
            CurrentKey = key;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Returns a string describing the type of this state machine and its <see cref="CurrentKey"/> and
        /// <see cref="CurrentState"/>.
        /// </summary>
        public override string ToString()
        {
            return string.Concat(
                base.ToString(),
                ": ",
                CurrentKey != null ? CurrentKey.ToString() : "",
                " -> ",
                CurrentState != null ? CurrentState.ToString() : "");
        }

        /************************************************************************************************************************/
    }
}
