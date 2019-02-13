// Animancer // Copyright 2018 Kybernetik //

using System;
using UnityEngine;

namespace Animancer
{
    /// <summary>
    /// An <see cref="AnimancerController"/> which uses a C# delegate to react to animation events called "Event".
    /// </summary>
    [AddComponentMenu("Animancer/Simple Event Animancer Controller")]
    [HelpURL(APIDocumentationURL + "/SimpleEventAnimancerController")]
    public class SimpleEventAnimancerController : EventfulAnimancerController
    {
        /************************************************************************************************************************/

        private Action _OnEvent;

        /// <summary>
        /// A callback triggered by any animation events called "Event".
        /// Registered delegates are automatically cleared whenever any of the Play or CrossFade overloads are used.
        /// <para></para>
        /// A warning will be logged if you try to register multiple callbacks to this property without clearing it or
        /// if you register an event when there is no animation currently playing that will actually trigger it.
        /// </summary>
        public Action OnEvent
        {
            get { return _OnEvent; }
            set
            {
                EditorAssertRegisterEvent(_OnEvent, value, "Event");
                _OnEvent = value;
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Called by Animation Events.
        /// </summary>
        private void Event()
        {
            if (OnEvent != null)
                OnEvent();
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Called by all of the Play and CrossFade overloads. Resets <see cref="OnEvent"/> to null.
        /// </summary>
        protected override void OnBeforePlay()
        {
            OnEvent = null;
        }

        /************************************************************************************************************************/
    }
}
