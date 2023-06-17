using UnityEngine;
using UnityEngine.Events;

namespace Games.GrumpyBear.Core.Events
{
    /// <summary>
    /// Companion helper class to <see cref="VoidEvent"/> to call methods on other component,
    /// when the event trigger. This is similar to <see cref="EventTListener{T}"/> but without
    /// an event payload.
    /// 
    /// </summary>
    /// <seealso cref="VoidEvent"/>
    [AddComponentMenu("Grumpy Bear Games/Core/Events/Void Event Listener")]
    public class VoidEventListener : MonoBehaviour
    {
        /// <summary>
        /// The <see cref="VoidEvent"/> which this component should subscribe to.
        /// </summary>
        [SerializeField] protected VoidEvent _event;
        
        /// <summary>
        /// The <c>UnityEvent</c> which gets invoked,
        /// when this component receives an event.
        /// </summary>
        [SerializeField] protected UnityEvent _onTrigger;

        private void OnEnable() => _event.Listeners += OnTrigger;
        private void OnDisable() => _event.Listeners -= OnTrigger;
        private void OnTrigger() => _onTrigger.Invoke();
    }
}
