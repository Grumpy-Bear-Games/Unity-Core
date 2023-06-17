// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Games.GrumpyBear.Core.LevelManagement
{
    public static class SceneLoader
    {
        /// <summary>
        /// Make sure a specific set of scenes by loading and unloading scenes.
        /// </summary>
        public static IEnumerator LoadExactlyByBuildIndex(IEnumerable<int> buildIndices, int activeBuildIndex)
        {
            var buildIndicesSet = new HashSet<int>(buildIndices);
            var scenesToUnload = Enumerable.Range(0, UnityEngine.SceneManagement.SceneManager.sceneCount)
                .Select(UnityEngine.SceneManagement.SceneManager.GetSceneAt)
                .Where(scene => scene.isLoaded && !buildIndicesSet.Contains(scene.buildIndex))
                .ToList();
            yield return LoadScenesByBuildIndex(buildIndicesSet, activeBuildIndex);
            yield return UnloadScenes(scenesToUnload);
        }

        /// <summary>
        /// Make sure a specific set of scenes by loading and unloading scenes.
        /// </summary>
        public static IEnumerator LoadExactlyByScenePath(IEnumerable<string> scenePaths, string activeScenePath)
        {
            var scenePathsSet = new HashSet<string>(scenePaths);
            var scenesToUnload = Enumerable.Range(0, UnityEngine.SceneManagement.SceneManager.sceneCount)
                .Select(UnityEngine.SceneManagement.SceneManager.GetSceneAt)
                .Where(scene => scene.isLoaded && !scenePathsSet.Contains(scene.path))
                .ToList();
            yield return LoadScenesByPath(scenePathsSet, activeScenePath);
            yield return UnloadScenes(scenesToUnload);
        }

        /// <summary>
        /// Load scenes by path and set active scene as a coroutine.
        /// </summary>
        public static IEnumerator LoadScenesByPath(IEnumerable<string> scenePaths, string activeScenePath)
        {
            yield return LoadScenesByPath(scenePaths);
            UnityEngine.SceneManagement.SceneManager.SetActiveScene(UnityEngine.SceneManagement.SceneManager.GetSceneByPath(activeScenePath));
        }

        /// <summary>
        /// Load scenes by path as a coroutine.
        /// </summary>
        public static IEnumerator LoadScenesByPath(IEnumerable<string> scenePaths) =>
            LoadScenesByBuildIndex(scenePaths.Select(SceneUtility.GetBuildIndexByScenePath));

        /// <summary>
        /// Load scenes by build index and set active scene as a coroutine.
        /// </summary>
        public static IEnumerator LoadScenesByBuildIndex(IEnumerable<int> buildIndices, int activeBuildIndex)
        {
            yield return LoadScenesByBuildIndex(buildIndices);
            UnityEngine.SceneManagement.SceneManager.SetActiveScene(UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(activeBuildIndex));
        }

        /// <summary>
        /// Load scenes by build index as a coroutine.
        /// </summary>
        public static IEnumerator LoadScenesByBuildIndex(IEnumerable<int> buildIndices)
        {
            var ops = buildIndices
                .Where(buildIndex => !UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(buildIndex).isLoaded)
                .Select(buildIndex => UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive));
            foreach (var op in ops)
            {
                while (!op.isDone) yield return null;
            }
        }

        /// <summary>
        /// Unload scenes as a coroutine.
        /// </summary>
        public static IEnumerator UnloadScenes(IEnumerable<Scene> scenes)
        {
            var ops = scenes
                .Select(scene => UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene.buildIndex));
            
            foreach (var op in ops)
            {
                while (!op.isDone) yield return null;
            }
        }
    }
}
