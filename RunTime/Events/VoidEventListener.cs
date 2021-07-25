using UnityEngine;
using UnityEngine.Events;

namespace Games.GrumpyBear.Core.Events
{
    [AddComponentMenu("Grumpy Bear Games/Core/Events/Void Event Listener")]
    public class VoidEventListener : MonoBehaviour
    {
        [SerializeField] protected VoidEvent _event;
        [SerializeField] protected UnityEvent _onTrigger;

        private void OnEnable() => _event.Listeners += OnTrigger;
        private void OnDisable() => _event.Listeners -= OnTrigger;
        private void OnTrigger() => _onTrigger.Invoke();
    }
}
