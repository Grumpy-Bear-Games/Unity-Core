using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Games.GrumpyBear.Core.LevelManagement
{
    public class SceneGroupColdStartInitializer: MonoBehaviour
    {
        private const string DEFAULT_NAME = "[Scene Group Coldstart Initializer]";
        
        [SerializeField] private SceneGroup _sceneGroup;
        [SerializeField] private UnityEvent<SceneGroup> _afterLoad;
        public SceneGroup SceneGroup => _sceneGroup;

        private static bool _initialized;

        private IEnumerator Start()
        {
            if (_initialized) yield break;
            if (!_sceneGroup.ContainsScene(gameObject.scene))
            {
                Debug.LogError("Scene Group does not contain this scene!");
                yield break;
            }

            yield return _sceneGroup.Load_CO();
            _afterLoad.Invoke(_sceneGroup);
            _initialized = true;
        }

        #if UNITY_EDITOR
        private void Reset()
        {
            var go = gameObject;
            go.name = DEFAULT_NAME;
            go.tag = "EditorOnly";
        }
        
        [UnityEditor.MenuItem("GameObject/Grumpy Bear Games/Core/Level Management/Scene Group ColdStart Initializer", false, 10)]
        private static void CreateSceneGroupColdStartInitializer(UnityEditor.MenuCommand menuCommand)
        {
            var go = new GameObject(DEFAULT_NAME, typeof(SceneGroupColdStartInitializer));
            UnityEditor.GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            UnityEditor.Selection.activeObject = go;
        }
        
        public static class Fields
        {
            public const string SceneGroup = nameof(_sceneGroup);
            public const string AfterLoad = nameof(_afterLoad);
        }
        #endif
    }
}
