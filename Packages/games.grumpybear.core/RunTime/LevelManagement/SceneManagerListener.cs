using UnityEngine;
using UnityEngine.Events;

namespace Games.GrumpyBear.Core.LevelManagement
{
    public class SceneManagerListener : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onSceneGroupChanged;
        
        private void Awake() => SceneManager.OnSceneGroupChanged += OnSceneGroupChanged;

        private void OnDestroy() => SceneManager.OnSceneGroupChanged -= OnSceneGroupChanged;

        private void OnSceneGroupChanged(SceneGroup sceneGroup) => _onSceneGroupChanged?.Invoke();
    }
}
