// Animancer // Copyright 2018 Kybernetik //

using UnityEngine;

namespace Animancer.Demo
{
    /// <summary>
    /// Base class for any kind of <see cref="Demo.Creature"/> controller - local, network, AI, replay, etc.
    /// </summary>
    [HelpURL(AnimancerController.APIDocumentationURL + ".Demo/CreatureBrain")]
    public abstract class CreatureBrain : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField]
        private Creature _Creature;
        public Creature Creature { get { return _Creature; } }

        /************************************************************************************************************************/

        public float Movement { get; protected set; }

        /************************************************************************************************************************/
    }
}
