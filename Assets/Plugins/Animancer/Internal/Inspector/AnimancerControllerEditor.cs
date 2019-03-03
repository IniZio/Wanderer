// Animancer // Copyright 2018 Kybernetik //

#if UNITY_EDITOR

using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Animancer.Editor
{
    /// <summary>[Editor-Only]
    /// A custom inspector for <see cref="AnimancerController"/> components.
    /// </summary>
    [CustomEditor(typeof(AnimancerController), true), CanEditMultipleObjects]
    public class AnimancerControllerEditor : AnimancerPlayableEditor
    {
        /************************************************************************************************************************/

        /// <summary>The priority of all context menu items added by this class.</summary>
        protected const int MenuItemPriority = 2000;

        /************************************************************************************************************************/

        /// <summary>Returns <see cref="EditorApplication.isPlaying"/> to disable certain menu items in Edit Mode.</summary>
        [MenuItem("CONTEXT/AnimancerController/Pause All", validate = true)]
        [MenuItem("CONTEXT/AnimancerController/Resume All", validate = true)]
        [MenuItem("CONTEXT/AnimancerController/Stop All", validate = true)]
        [MenuItem("CONTEXT/AnimancerControllerAuto/Play Animations in Sequence", validate = true)]
        [MenuItem("CONTEXT/AnimancerControllerAuto/Cross Fade Animations in Sequence", validate = true)]
        [MenuItem("CONTEXT/AnimancerController/Evaluate", validate = true)]
        [MenuItem("CONTEXT/AnimancerController/Log Description of States", validate = true)]
        private static bool TrueInPlayMode()
        {
            return EditorApplication.isPlaying;
        }

        /************************************************************************************************************************/

        /// <summary>Calls <see cref="AnimancerController.PauseAll()"/>.</summary>
        [MenuItem("CONTEXT/AnimancerController/Pause All", priority = MenuItemPriority)]
        private static void PauseAll(MenuCommand command)
        {
            var animancer = command.context as AnimancerController;
            animancer.PauseAll();
        }

        /// <summary>Calls <see cref="AnimancerController.ResumeAll()"/>.</summary>
        [MenuItem("CONTEXT/AnimancerController/Resume All", priority = MenuItemPriority)]
        private static void ResumeAll(MenuCommand command)
        {
            var animancer = command.context as AnimancerController;
            animancer.ResumeAll();
            animancer.Evaluate();
        }

        /// <summary>Calls <see cref="AnimancerController.StopAll()"/>.</summary>
        [MenuItem("CONTEXT/AnimancerController/Stop All", priority = MenuItemPriority)]
        private static void StopAll(MenuCommand command)
        {
            var animancer = command.context as AnimancerController;
            animancer.StopAll();
            animancer.Evaluate();
        }

        /************************************************************************************************************************/

        /// <summary>Starts <see cref="AnimancerController.PlayAnimationsInSequence"/> as a coroutine.</summary>
        [MenuItem("CONTEXT/AnimancerControllerAuto/Play Animations in Sequence", priority = MenuItemPriority)]
        private static void PlayAnimationsInSequence(MenuCommand command)
        {
            var animancer = command.context as LegacyAnimancerController;
            animancer.StartCoroutine(animancer.PlayAnimationsInSequence());
        }

        /// <summary>Starts <see cref="AnimancerController.CrossFadeAnimationsInSequence"/> as a coroutine.</summary>
        [MenuItem("CONTEXT/AnimancerControllerAuto/Cross Fade Animations in Sequence", priority = MenuItemPriority)]
        private static void CrossFadeAnimationsInSequence(MenuCommand command)
        {
            var animancer = command.context as LegacyAnimancerController;
            animancer.StartCoroutine(animancer.CrossFadeAnimationsInSequence());
        }

        /************************************************************************************************************************/

        /// <summary>Calls <see cref="AnimancerController.Evaluate()"/>.</summary>
        [MenuItem("CONTEXT/AnimancerController/Evaluate", priority = MenuItemPriority)]
        private static void Evaluate(MenuCommand command)
        {
            var animancer = command.context as AnimancerController;
            animancer.Evaluate();
        }

        /************************************************************************************************************************/

        /// <summary>Logs a description of all states currently in the <see cref="AnimancerController.Playable"/>.</summary>
        [MenuItem("CONTEXT/AnimancerController/Log Description of States", priority = MenuItemPriority)]
        private static void LogDescriptionOfStates(MenuCommand command)
        {
            var animancer = command.context as AnimancerController;
            if (animancer.IsPlayableInitialised)
                Debug.Log(animancer + ":\n" + animancer.Playable.GetDescription(), animancer);
            else
                Debug.Log("AnimancerController.Playable is not initialised.", animancer);
        }

        /************************************************************************************************************************/

        /// <summary>Opens the Animancer Documentation website (animancer.github.io).</summary>
        [MenuItem("CONTEXT/AnimancerController/Animancer Documentation (animancer.github.io)", priority = MenuItemPriority)]
        private static void OpenUserManual(MenuCommand command)
        {
            EditorUtility.OpenWithDefaultApp(AnimancerController.DocumentationURL);
        }

        /************************************************************************************************************************/

        /// <summary>The email address which handles support for Animancer.</summary>
        public const string DeveloperEmail = "AnimancerUnityPlugin@gmail.com";

        /// <summary>Opens an email to "AnimancerUnityPlugin@gmail.com" and copies that string to the clipboard.</summary>
        [MenuItem("CONTEXT/AnimancerController/Email Support (" + DeveloperEmail + ")", priority = MenuItemPriority)]
        public static void EmailTheDeveloper()
        {
            EditorGUIUtility.systemCopyBuffer = DeveloperEmail;
            Debug.Log("Copied '" + DeveloperEmail + "' to clipboard.");
            Application.OpenURL("mailto:" + DeveloperEmail + "? subject=Animancer");
        }

        /************************************************************************************************************************/
    }
}

#endif
