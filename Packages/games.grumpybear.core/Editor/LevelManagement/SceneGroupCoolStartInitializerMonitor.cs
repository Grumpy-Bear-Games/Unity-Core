#if UNITY_EDITOR
using Games.GrumpyBear.Core.LevelManagement;
using UnityEditor;
using UnityEngine;

namespace Games.GrumpyBear.Core.Editor.LevelManagement
{
    [InitializeOnLoad]
    public static class SceneGroupCoolStartInitializerMonitor
    {
        // Can it really be called a constructor if it's a static class?
        // Initialize any callbacks.
        //
        static SceneGroupCoolStartInitializerMonitor()
        {
            UnityEditor.SceneManagement.EditorSceneManager.sceneOpened +=
                _SceneOpenedCallback;
        }

        private static void _SceneOpenedCallback(
            UnityEngine.SceneManagement.Scene scene,
            UnityEditor.SceneManagement.OpenSceneMode mode)
        {
            // Don't do anything if scene loaded in any additive mode.
            if (mode != UnityEditor.SceneManagement.OpenSceneMode.Single)
                return;

            foreach (var rootGameObject in scene.GetRootGameObjects())
            {
                var initializer = rootGameObject.GetComponentInChildren<SceneGroupColdStartInitializer>();
                if (initializer == null) continue;
                var sceneGroup = initializer.SceneGroup;
                if (!sceneGroup.ContainsScene(initializer.gameObject.scene))
                {
                    Debug.LogError("Scene Group does not contain current scene", initializer);
                    return;
                }

                initializer.SceneGroup.LoadInEditor();
                return;
            }
        }
    }
}
#endif
