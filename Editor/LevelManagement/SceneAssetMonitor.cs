#if CORE_GAME_UTILITIES_EXPERIMENTAL
using System.Linq;
using Games.GrumpyBear.Core.LevelManagement;
using UnityEditor;
using UnityEngine;

namespace Games.GrumpyBear.Core.Editor.LevelManagement
{
    public class SceneAssetMonitor : AssetModificationProcessor
    {
        private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
        {
            var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(sourcePath);
            if (sceneAsset == null) return AssetMoveResult.DidNotMove;
            
            foreach (var guid in AssetDatabase.FindAssets($"t:{nameof(SceneManager)}"))
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var sceneManager = AssetDatabase.LoadAssetAtPath<SceneManager>(assetPath);
                if (!sceneManager.OnSceneMove(sourcePath, destinationPath)) continue;
                EditorUtility.SetDirty(sceneManager);
                AssetDatabase.SaveAssetIfDirty(sceneManager);
            }

            foreach (var guid in AssetDatabase.FindAssets($"t:{nameof(SceneGroup)}"))
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var sceneGroup = AssetDatabase.LoadAssetAtPath<SceneGroup>(assetPath);
                if (!sceneGroup.OnSceneMove(sourcePath, destinationPath)) continue;
                EditorUtility.SetDirty(sceneGroup);
                AssetDatabase.SaveAssetIfDirty(sceneGroup);
            }
            
            return AssetMoveResult.DidNotMove;
        }

        private static AssetDeleteResult OnWillDeleteAsset(string sourcePath, RemoveAssetOptions options)
        {
            var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(sourcePath);
            if (sceneAsset == null) return AssetDeleteResult.DidNotDelete;
            
            foreach (var guid in AssetDatabase.FindAssets($"t:{nameof(SceneManager)}"))
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var sceneManager = AssetDatabase.LoadAssetAtPath<SceneManager>(assetPath);
                if (sceneManager.GlobalScenes.All(scene => scene.ScenePath != sourcePath)) continue;
                Debug.LogError($"Can't delete Scene '{sceneAsset.name}' while it is in use in Scene Manager '{sceneManager.name}'");
                return AssetDeleteResult.FailedDelete;

            }

            foreach (var guid in AssetDatabase.FindAssets($"t:{nameof(SceneGroup)}"))
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var sceneGroup = AssetDatabase.LoadAssetAtPath<SceneGroup>(assetPath);
                if (sceneGroup.Scenes.All(scene => scene.ScenePath != sourcePath)) continue;
                Debug.LogError($"Can't delete Scene '{sceneAsset.name}' while it is in use in Scene Group '{sceneGroup.name}'");
                return AssetDeleteResult.FailedDelete;
            }
            return AssetDeleteResult.DidNotDelete;
        }
    }
}
#endif
