using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games.GrumpyBear.Core.LevelManagement
{
    public class SceneGroup : ScriptableObject
    {
        [HideInInspector][SerializeField] private SceneManager _manager;
        [SerializeField] private List<SceneReference> _scenes = new();
        public IReadOnlyList<SceneReference> Scenes => _scenes;
        public SceneReference ActiveScene => _scenes[0];
        public SceneManager Manager => _manager;

        public void Load() => _manager.Load(this);
        public IEnumerator Load_CO() => _manager.Load_CO(this);
        
        #if UNITY_EDITOR
        public static SceneGroup CreateInstance(SceneManager sceneManager)
        {
            var sceneGroup = ScriptableObject.CreateInstance<SceneGroup>();
            sceneGroup._manager = sceneManager;
            sceneGroup._scenes = new List<SceneReference>();
            return sceneGroup;
        }
        #endif
    }
}
