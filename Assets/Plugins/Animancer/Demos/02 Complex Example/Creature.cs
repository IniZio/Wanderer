// Animancer // Copyright 2018 Kybernetik //

using System.Collections.Generic;
using UnityEngine;

namespace Animancer.Demo
{
    /// <summary>
    /// A centralised group of references to the common parts of a creature and a state machine for their actions.
    /// </summary>
    [AddComponentMenu("Animancer/Demo/Creature")]
    [HelpURL(AnimancerController.APIDocumentationURL + ".Demo/Creature")]
    public sealed class Creature : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField]
        private AnimancerController _Anim;
        public AnimancerController Anim { get { return _Anim; } }

        [SerializeField]
        private CreatureBrain _Brain;
        public CreatureBrain Brain { get { return _Brain; } }

        [SerializeField]
        private Rigidbody _Body;
        public Rigidbody Body { get { return _Body; } }

        [SerializeField]
        private CreatureFeet _Feet;
        public CreatureFeet Feet { get { return _Feet; } }

        // Stats.
        // Health and Mana pools.
        // Pathfinding.
        // Etc.
        // Anything common to most creatures.

        [SerializeField]
        private CreatureState[] _States;

        /************************************************************************************************************************/

        public readonly StateMachine<CreatureStateType, CreatureState>
            StateMachine = new StateMachine<CreatureStateType, CreatureState>();

        /************************************************************************************************************************/

        private void Awake()
        {
            for (int i = 0; i < _States.Length; i++)
            {
                AddState(_States[i]);
            }

            StateMachine.TrySetState(_States[0].Type);
        }

        /************************************************************************************************************************/

        public void AddState(CreatureState state)
        {
            StateMachine.Add(state.Type, state);
            state.Creature = this;
        }

        /************************************************************************************************************************/
    }
}
