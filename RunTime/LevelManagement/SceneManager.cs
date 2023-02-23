using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Games.GrumpyBear.Core.LevelManagement
{
    [CreateAssetMenu(menuName = "Grumpy Bear Games/Core/Level Management/Scene Manager")]
    public class SceneManager: ScriptableObject
    {
        public static event Action<SceneGroup> OnSceneGroupChanged;
        public static SceneGroup SceneGroupCurrentlyLoading;
        public static bool IsLoadingSceneGroup => SceneGroupCurrentlyLoading != null;

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
            OnSceneGroupChanged?.Invoke(sceneGroup);
            SceneGroupCurrentlyLoading = null;
        }

        #if UNITY_EDITOR
        public const string GlobalScenesPropertyName = nameof(_globalScenes);
        public const string SceneGroupsPropertyName = nameof(_sceneGroups);
        #endif
    }
}
