// Animancer // Copyright 2018 Kybernetik //

using UnityEngine;

namespace Animancer.Demo
{
    /// <summary>
    /// An identifier for a <see cref="CreatureState"/>.
    /// </summary>
    public enum CreatureStateType
    {
        Idle,
        Move,
        Crouch,
        Jump,
        Attack,

        // Flinch,
        // Skill,
        // Special,
        // Taunt,
        // etc.
    }

    /// <summary>
    /// Base class for the various states a <see cref="Demo.Creature"/> can be in and actions they can perform.
    /// </summary>
    [HelpURL(AnimancerController.APIDocumentationURL + ".Demo/CreatureState")]
    public abstract class CreatureState : StateBehaviour<CreatureStateType>
    {
        /************************************************************************************************************************/

        [SerializeField]
        private CreatureStateType _Type;

        /// <summary>The dictionary key of this state.</summary>
        public CreatureStateType Type { get { return _Type; } }

        /************************************************************************************************************************/

        // Not serialized. Assigned when adding this action to the Creature. Most of the time actions will be
        // pre-configured as part of a prefab, but this ensures that actions can be added dynamically at runtime.
        private Creature _Creature;

        /// <summary>
        /// The <see cref="Demo.Creature"/> which this state is attached to.
        /// </summary>
        public Creature Creature
        {
            get { return _Creature; }
            set
            {
                _Creature = value;
                OnCreatureAssigned();
            }
        }

        /// <summary>
        /// Use this for initialisation since Awake might get called before the creature has been assigned.
        /// </summary>
        protected virtual void OnCreatureAssigned() { }

        /************************************************************************************************************************/

#if UNITY_EDITOR
        /// <summary>
        /// States start disabled and only the current state gets enabled.
        /// </summary>
        protected virtual void Reset()
        {
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            enabled = false;
        }
#endif

        /************************************************************************************************************************/

        /// <summary>
        /// Forces the <see cref="Creature"/> to return to the <see cref="CreatureStateType.Idle"/> state.
        /// </summary>
        public void ReturnToIdle()
        {
            _Creature.StateMachine.ForceSetState(CreatureStateType.Idle);
        }

        /************************************************************************************************************************/
    }
}
