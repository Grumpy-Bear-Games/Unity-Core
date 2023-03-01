using UnityEngine;

namespace Games.GrumpyBear.Core.Observables.ScriptableObjects
{
    [AddComponentMenu("Grumpy Bear Games/Core/Observables/GameObject Observable Notifier")]
    public class GameObjectObservableNotifier : MonoBehaviour
    {
        [SerializeField] private GameObjectObservable _gameObjectObservable;
        [SerializeField] private bool _registerOnAwake;

        private void Awake()
        {
            if (_registerOnAwake) _gameObjectObservable.Set(gameObject);
        }

        private void OnEnable()
        {
            if (!_registerOnAwake) _gameObjectObservable.Set(gameObject);
        }

        private void OnDisable()
        {
            if (_registerOnAwake || _gameObjectObservable.Value != gameObject) return;
            _gameObjectObservable.Set(null);
        }

        private void OnDestroy()
        {
            if (!_registerOnAwake || _gameObjectObservable.Value != gameObject) return;
            _gameObjectObservable.Set(null);
        }
    }
}
