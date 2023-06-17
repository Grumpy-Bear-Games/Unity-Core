using UnityEngine;

namespace Games.GrumpyBear.Core.Observables.ScriptableObjects
{
    [AddComponentMenu("Grumpy Bear Games/Core/Observables/Camera Observable Notifier")]
    [RequireComponent(typeof(Camera))]
    public class CameraObservableNotifier : MonoBehaviour
    {
        [SerializeField] private CameraObservable _cameraObservable;

        private Camera _camera;

        private void Awake() => _camera = GetComponent<Camera>();

        private void OnEnable() => _cameraObservable.Set(_camera);

        private void OnDisable()
        {
            if (_cameraObservable.Value == _camera) _cameraObservable.Set(null);
        }
    }
}