using UnityEngine;
using UnityEngine.Events;

namespace Games.GrumpyBear.Core.Events
{
    /// <summary>
    /// Companion helper class to <see cref="EventT{T}"/> to call methods on other component,
    /// when the event trigger. The event payload can be provided to these methods as a parameter.
    /// This is similar to <see cref="VoidEventListener"/> but with an event payload.
    /// 
    /// </summary>
    /// <remarks>
    /// Because Unity can't instantiate generics directly, you must override the class type. 
    /// <code>
    /// public class IntEventListener: EventTListener&lt;t&gt; {}
    /// </code>
    /// </remarks>
    /// <typeparam name="T">The event payload type</typeparam>
    /// <seealso cref="EventT{T}"/>
    public class EventTListener<T> : MonoBehaviour
    {
        /// <summary>
        /// The <see cref="EventT{T}"/> which this component should subscribe to.
        /// </summary>
        [SerializeField] protected EventT<T> _event;
        
        /// <summary>
        /// The <c>UnityEvent&lt;T&gt;</c> which gets invoked,
        /// when this component receives an event. The event payload is passed to
        /// <c>UnityEvent&lt;T&gt;.Invoke()</c>.
        /// </summary>
        [SerializeField] protected UnityEvent<T> _onTrigger;

        private void OnEnable() => _event.Listeners += OnTrigger;
        private void OnDisable() => _event.Listeners -= OnTrigger;
        private void OnTrigger(T value) => _onTrigger.Invoke(value);
        
    }
}
