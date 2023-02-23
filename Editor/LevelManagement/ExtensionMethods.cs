// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

using System.Linq;
using Games.GrumpyBear.Core.LevelManagement;
using UnityEditor.SceneManagement;

namespace Games.GrumpyBear.Core.Editor.LevelManagement
{
    public static class ExtensionMethods
    {
        #region SceneGroup
        public static void LoadInEditor(this SceneGroup sceneGroup)
        {
            var sceneManager = sceneGroup.Manager;
            var openScenes = Enumerable.Range(0, EditorSceneManager.loadedSceneCount)
                .Select(UnityEngine.SceneManagement.SceneManager.GetSceneAt)
                .Where(scene => sceneGroup.Scenes.All(x => scene.path != x.ScenePath))
                .Where(scene => sceneManager.GlobalScenes.All(x => scene.path != x.ScenePath))
                .ToArray();
            if (!EditorSceneManager.SaveModifiedScenesIfUserWantsTo(openScenes)) return;
            foreach (var sceneAsset in sceneGroup.Scenes)
            {
                EditorSceneManager.OpenScene(sceneAsset.ScenePath, OpenSceneMode.Additive);
            }
            foreach (var sceneAsset in sceneManager.GlobalScenes)
            {
                EditorSceneManager.OpenScene(sceneAsset.ScenePath, OpenSceneMode.Additive);
            }
            UnityEngine.SceneManagement.SceneManager.SetActiveScene(UnityEngine.SceneManagement.SceneManager.GetSceneByPath(sceneGroup.ActiveScene.ScenePath));
            foreach (var openScene in openScenes)
            {
                EditorSceneManager.CloseScene(openScene, true);
            }
        }
        #endregion
    }
}
