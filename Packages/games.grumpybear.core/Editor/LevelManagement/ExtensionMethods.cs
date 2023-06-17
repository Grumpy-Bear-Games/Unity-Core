// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

using System;
using System.Linq;
using Games.GrumpyBear.Core.LevelManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Games.GrumpyBear.Core.Editor.LevelManagement
{
    public static class ExtensionMethods
    {
        #region SceneReference
        public static bool IsEmpty(this SceneReference sceneReference) => string.IsNullOrEmpty(sceneReference.ScenePath);

        public static bool IsValidScene(this SceneReference sceneReference)
        {
            if (sceneReference.IsEmpty()) return false;
            var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(sceneReference.ScenePath);
            return sceneAsset != null;
        }

        public static bool IsInBuild(this SceneReference sceneReference)
        {
            if (!sceneReference.IsValidScene()) return false;
            return sceneReference.BuildIndex != -1;
        }

        public static SceneReferenceStatus Validate(this SceneReference sceneReference)
        {
            if (sceneReference.IsEmpty()) return SceneReferenceStatus.NoSceneSelected;
            var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(sceneReference.ScenePath);
            if (sceneAsset == null) return SceneReferenceStatus.InvalidScenePath;
            return sceneReference.BuildIndex == -1 ? SceneReferenceStatus.SceneMissingFromBuild : SceneReferenceStatus.Valid;
        }

        public enum SceneReferenceStatus {
            Valid,
            NoSceneSelected,
            InvalidScenePath,
            SceneMissingFromBuild
        }

        public static void AddToBuild(this SceneReference sceneReference)
        {
            if (sceneReference.IsInBuild()) return;
            var editorBuildSettingsScenes = EditorBuildSettings.scenes.ToList();

            try
            {
                var scene = editorBuildSettingsScenes.First(scene => scene.path == sceneReference.ScenePath);
                scene.enabled = true;
            }
            catch (InvalidOperationException) {
                editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(sceneReference.ScenePath, true));
            }
            
            EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
        }
        #endregion
        
        
        #region SceneGroup
        public static void LoadInEditor(this SceneGroup sceneGroup)
        {
            var sceneManager = sceneGroup.Manager;
            var openScenes = Enumerable.Range(0, UnityEngine.SceneManagement.SceneManager.loadedSceneCount)
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

        public static bool AllScenesInBuild(this SceneGroup sceneGroup) =>
            sceneGroup.Scenes.All(sceneReference => sceneReference.IsInBuild());

        #endregion

        
        #region SceneManager
        public static bool AllScenesInBuild(this SceneManager sceneManager) =>
            sceneManager.GlobalScenes.All(sceneReference => sceneReference.IsInBuild()) &&
            sceneManager.SceneGroups.All(group => group.AllScenesInBuild());

        #endregion
    }
}
