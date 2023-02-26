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
        public static class Fields
        {
            public const string ScenePath = nameof(SceneReference._scenePath);
        }
        #endif
    }
}
