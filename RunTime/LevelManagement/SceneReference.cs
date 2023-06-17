using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Games.GrumpyBear.Core.LevelManagement
{
    [Serializable]
    public class SceneReference
    {
        [SerializeField] private string _scenePath;
        public string ScenePath => _scenePath;

        public int BuildIndex => SceneUtility.GetBuildIndexByScenePath(ScenePath);
        public Scene Scene => UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(BuildIndex);
        
        #if UNITY_EDITOR
        #if CORE_GAME_UTILITIES_EXPERIMENTAL
        public bool OnSceneMove(string source, string destination)
        {
            if (_scenePath != source) return false;
            Debug.Log($"Updating {source} to {destination}");
            _scenePath = destination;
            return true;
        }
        #endif

        public static class Fields
        {
            public const string ScenePath = nameof(_scenePath);
        }
        #endif
    }
}
