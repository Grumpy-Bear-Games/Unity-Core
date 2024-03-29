using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Games.GrumpyBear.Core.Observables;
using UnityEngine;

namespace Games.GrumpyBear.Core.LevelManagement
{
    [CreateAssetMenu(menuName = "Grumpy Bear Games/Core/Level Management/Scene Manager")]
    public class SceneManager: ScriptableObject
    {
        [Obsolete("OnSceneGroupChanged is deprecated. Please use CurrentSceneGroup instead")]
        public static event Action<SceneGroup> OnSceneGroupChanged;
        public static IReadonlyObservable<SceneGroup> CurrentSceneGroup => _currentSceneGroup;
        
        public static SceneGroup SceneGroupCurrentlyLoading { get; private set; }
        public static bool IsLoadingSceneGroup => SceneGroupCurrentlyLoading != null;

        private static readonly Observable<SceneGroup> _currentSceneGroup = new();

        public static IEnumerator WaitForLoadingCompleted()
        {
            while (IsLoadingSceneGroup) yield return null;
        }


        [SerializeField] private List<SceneReference> _globalScenes = new();
        [SerializeField] private List<SceneGroup> _sceneGroups = new();
        public IReadOnlyList<SceneReference> GlobalScenes => _globalScenes;
        public IReadOnlyList<SceneGroup> SceneGroups => _sceneGroups;

        public void Load(SceneGroup sceneGroup) => SceneManagerHelper.Instance.StartCoroutine(Load_CO(sceneGroup));

        public IEnumerator Load_CO(SceneGroup sceneGroup)
        {
            if (IsLoadingSceneGroup)
            {
                if (SceneGroupCurrentlyLoading != sceneGroup)
                {
                    Debug.LogError($"Failed to load SceneGroup {sceneGroup.name} while SceneGroup {SceneGroupCurrentlyLoading.name} is already being loaded");
                }
                yield break;
            }

            SceneGroupCurrentlyLoading = sceneGroup;
            var sceneReferencesToLoad = sceneGroup.Scenes.Concat(_globalScenes).Select(scene => scene.ScenePath);
            yield return SceneLoader.LoadExactlyByScenePath(sceneReferencesToLoad, sceneGroup.ActiveScene.ScenePath);
            _currentSceneGroup.Set(sceneGroup);
            OnSceneGroupChanged?.Invoke(sceneGroup);
            SceneGroupCurrentlyLoading = null;
        }

        #if UNITY_EDITOR
        #if CORE_GAME_UTILITIES_EXPERIMENTAL
        public bool OnSceneMove(string source, string destination)
        {
            var hasChanged = false;
            foreach (var sceneReference in GlobalScenes)
            {
                if (sceneReference.OnSceneMove(source, destination)) hasChanged = true;
            }
            return hasChanged;
        }
        #endif
        
        public static class Fields
        {
            public const string GlobalScenes = nameof(_globalScenes);
            public const string SceneGroups = nameof(_sceneGroups);
        }
        #endif
    }
}
