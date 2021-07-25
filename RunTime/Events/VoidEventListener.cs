using UnityEngine;
using UnityEngine.Events;

namespace Games.GrumpyBear.Core.Events
{
    public class VoidEventListener : MonoBehaviour
    {
        [SerializeField] private VoidEvent _event;
        [SerializeField] private UnityEvent _onTrigger;

        private void OnEnable() => _event.Listeners += OnTrigger;
        private void OnDisable() => _event.Listeners -= OnTrigger;
        private void OnTrigger() => _onTrigger.Invoke();
    }
}
