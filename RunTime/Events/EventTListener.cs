using UnityEngine;
using UnityEngine.Events;

namespace Games.GrumpyBear.Core.Events
{
    public class EventTListener<T> : MonoBehaviour
    {
        [SerializeField] private EventT<T> _event;
        [SerializeField] private UnityEvent<T> _onTrigger;

        private void OnEnable() => _event.Listeners += OnTrigger;
        private void OnDisable() => _event.Listeners -= OnTrigger;
        private void OnTrigger(T value) => _onTrigger.Invoke(value);
        
    }
}
