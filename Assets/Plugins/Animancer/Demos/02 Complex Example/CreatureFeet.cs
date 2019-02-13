// Animancer // Copyright 2018 Kybernetik //

using System.Collections.Generic;
using UnityEngine;

namespace Animancer.Demo
{
    /// <summary>
    /// Keeps track of whether or not an object is touching the ground.
    /// </summary>
    [AddComponentMenu("Animancer/Demo/Creature Feet")]
    [HelpURL(AnimancerController.APIDocumentationURL + ".Demo/CreatureFeet")]
    public sealed class CreatureFeet : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField]
        private Vector3[] _CastPoints;

        [SerializeField]
        private float _CastRange;

        /************************************************************************************************************************/

        public bool IsGrounded { get; private set; }

        /************************************************************************************************************************/

        private void FixedUpdate()
        {
            IsGrounded = false;

            for (int i = 0; i < _CastPoints.Length; i++)
            {
                var start = transform.TransformPoint(_CastPoints[i]);

                RaycastHit hit;
                if (Physics.Raycast(start, Vector3.down, out hit, _CastRange))
                {
                    // Normally you would check the angle of the hit.normal to make sure it isn't too steep.
                    IsGrounded = true;
                    return;
                }
            }
        }

        /************************************************************************************************************************/

        private void OnDrawGizmosSelected()
        {
            if (_CastPoints == null)
                return;

            Gizmos.color = Color.blue;

            for (int i = 0; i < _CastPoints.Length; i++)
            {
                var start = transform.TransformPoint(_CastPoints[i]);
                Gizmos.DrawLine(start, start + Vector3.down * _CastRange);
            }
        }

        /************************************************************************************************************************/
    }
}
