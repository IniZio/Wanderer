// Animancer // Copyright 2018 Kybernetik //

#if UNITY_EDITOR

using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Animancer.Editor
{
    /// <summary>[Editor-Only] [Internal]
    /// An introductory <see cref="EditorWindow"/> for when you first import a new version of Animancer.
    /// </summary>
    internal sealed class WelcomeWindow : EditorWindow
    {
        /************************************************************************************************************************/

        /// <summary>The index of this Animancer release.</summary>
        private const int ReleaseNumber = 4;

        /// <summary>The key used to save the release number.</summary>
        private const string PrefKey = "Animancer.ReleaseNumber";

        /// <summary>
        /// A temporary file to ensure that this window only gets shown once instead of every time scripts are
        /// recompiled, even if the user hasn't selected "Don't show this again" yet.
        /// </summary>
        private const string LockFile = "Temp/AnimancerWelcome";

        private const string DemoScenePath = "Assets/Plugins/Animancer/Demos/01 Simple Example/01 Simple Example.unity";

        /************************************************************************************************************************/

        /// <summary>
        /// Called after Unity reloads assemblies (such as on startup and when a script is modified).
        /// Checks if the Animancer version has changed and hasn't yet been shown. If so, this method opens it.
        /// </summary>
        [InitializeOnLoadMethod]
        private static void Initialise()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode ||
                EditorPrefs.GetInt(PrefKey, -1) >= ReleaseNumber ||
                File.Exists(LockFile))
                return;

            EditorApplication.delayCall += () =>
            {
                File.WriteAllText(LockFile, "");

                GetWindow<WelcomeWindow>();
            };
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Called by Unity when this component becomes enabled and active.
        /// Configures the window title and size.
        /// </summary>
        private void OnEnable()
        {
            titleContent = new GUIContent("Animancer");
            minSize = new Vector2(300, 235);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Called by Unity to draw the GUI for this window and respond to input events.
        /// </summary>
        private void OnGUI()
        {
            GUILayout.Label("Welcome to", Styles.CenteredLabel);
            GUILayout.Label("Animancer v2.0", Styles.Headding);
            GUILayout.Label("If you just upgraded from an earlier version of Animancer" +
                " you will need to restart Unity before you can use it." +
                " It is also recommended that you fully delete and reimport it since it has been reorganised a lot.",
                EditorStyles.wordWrappedLabel);

            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            if (GUILayout.Button("Go to first Demo Scene"))
            {
                EditorSceneManager.OpenScene(DemoScenePath);

                var scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(DemoScenePath);

                Selection.activeObject = null;
                EditorApplication.delayCall += () =>
                {
                    EditorApplication.delayCall += () =>
                        Selection.activeObject = scene;
                };
            }

            if (GUILayout.Button("Documentation: " + AnimancerController.DocumentationURL))
                EditorUtility.OpenWithDefaultApp(AnimancerController.DocumentationURL);

            if (GUILayout.Button("Support: " + AnimancerControllerEditor.DeveloperEmail))
                AnimancerControllerEditor.EmailTheDeveloper();

            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            if (GUILayout.Button("Close and don't show this again"))
            {
                EditorPrefs.SetInt(PrefKey, ReleaseNumber);
                Close();
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// <see cref="GUIStyle"/>s used by this window. They are located in a nested class so they don't get
        /// initialised before they are referenced (because they can't be until the first <see cref="OnGUI"/> call).
        /// </summary>
        private static class Styles
        {
            /// <summary>
            /// The standard <see cref="GUISkin.label"/> with the alignment centered.
            /// </summary>
            public static readonly GUIStyle CenteredLabel = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
            };

            /// <summary>
            /// The standard <see cref="GUISkin.label"/> with the alignment centered and a larger size.
            /// </summary>
            public static readonly GUIStyle Headding = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 26,
            };
        }

        /************************************************************************************************************************/
    }
}

#endif
