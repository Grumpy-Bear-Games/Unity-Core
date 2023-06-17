using UnityEngine;
using UnityEngine.Events;

namespace Games.GrumpyBear.Core.Observables.ScriptableObjects
{
    public class ObservableListener<T> : MonoBehaviour
    {
        [SerializeField] protected Observable<T> _observable;
        [SerializeField] protected UnityEvent<T> _onChange;

        private void OnEnable() => _observable.Subscribe(_onChange.Invoke);

        private void OnDisable() => _observable.Unsubscribe(_onChange.Invoke);
    }
}