// Animancer // Copyright 2018 Kybernetik //

using UnityEngine;

namespace Animancer.Demo
{
    /// <summary>
    /// A brain for creatures controlled by local input (keyboard and mouse).
    /// </summary>
    [AddComponentMenu("Animancer/Demo/Local Player Brain")]
    [HelpURL(AnimancerController.APIDocumentationURL + ".Demo/LocalPlayerBrain")]
    public sealed class LocalPlayerBrain : CreatureBrain
    {
        /************************************************************************************************************************/

        private void Update()
        {
            // Actions.

            // Left click to attack.
            if (Input.GetKeyDown(KeyCode.Mouse0))
                Creature.StateMachine.TrySetState(CreatureStateType.Attack);

            // Hold space to crouch.
            if (Input.GetKey(KeyCode.Space))
                Creature.StateMachine.TrySetState(CreatureStateType.Crouch);

            // Release space to jump.
            if (Input.GetKeyUp(KeyCode.Space))
                Creature.StateMachine.TrySetState(CreatureStateType.Jump);

            // Movement.
            // A/D or Arrow Keys to move.

            Movement = 0;

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                Movement--;

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                Movement++;

            if (Movement != 0 && Creature.StateMachine.CurrentKey == CreatureStateType.Idle)
                Creature.StateMachine.TrySetState(CreatureStateType.Move);
        }

        /************************************************************************************************************************/
    }
}
