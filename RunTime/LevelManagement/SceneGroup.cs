using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Games.GrumpyBear.Core.SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Games.GrumpyBear.Core.LevelManagement
{
    public class SceneGroup : SerializableScriptableObject<SceneGroup>
    {
        [HideInInspector][SerializeField] private SceneManager _manager;
        [SerializeField] private List<SceneReference> _scenes = new();
        public IReadOnlyList<SceneReference> Scenes => _scenes;
        public SceneReference ActiveScene => _scenes[0];
        public SceneManager Manager => _manager;

        public bool ContainsScene(Scene scene) => _scenes.Any(sceneReference => sceneReference.ScenePath == scene.path);

        public void Load() => _manager.Load(this);
        public IEnumerator Load_CO() => _manager.Load_CO(this);
        
        #if UNITY_EDITOR
        #if CORE_GAME_UTILITIES_EXPERIMENTAL
        public bool OnSceneMove(string source, string destination)
        {
            var hasChanged = false;
            foreach (var sceneReference in _scenes)
            {
                if (sceneReference.OnSceneMove(source, destination)) hasChanged = true;
            }
            return hasChanged;
        }
        #endif

        public static SceneGroup CreateInstance(SceneManager sceneManager)
        {
            var sceneGroup = ScriptableObject.CreateInstance<SceneGroup>();
            sceneGroup._manager = sceneManager;
            sceneGroup._scenes = new List<SceneReference>();
            return sceneGroup;
        }

        public static class Fields
        {
            public const string Scenes = nameof(_scenes);
        }
        #endif
    }
}
