// ReSharper disable UnusedMember.Global
using System;
using UnityEngine;

namespace Games.GrumpyBear.Core.Events
{
    /// <summary>
    /// A generic reference-able event with a event payload used to propagate data among components in a
    /// Pub-Sub fashion without neither producers nor consumers knowing about each other. This is similar
    /// to <see cref="VoidEvent"/> but with an event payload.
    ///
    /// Because instances of <see cref="EventT{T}"/> are <c>ScriptableObjects</c>
    /// they can be referenced in serialized fields and <c>UnityEvents</c>.
    /// </summary>
    ///
    /// <remarks>
    /// Because Unity can't instantiate generics directly, you must override the class type. 
    /// <code>
    /// [CreateAssetMenu(menuName = "Integer Event", fileName = "Integer Event")]
    /// public class IntEvent: EventT&lt;t&gt; {}
    /// </code>
    /// </remarks>
    ///
    /// <example>
    /// Imagine you have a player health component and a UI components, which spawns a small
    /// floating popup every time the player loses health. Using <see cref="EventT{T}"/> we
    /// can have the health component broadcast the damage amount to the UI components without
    /// any of them knowing about or depending on each other.
    /// 
    /// <code>
    /// public class PlayerHealth: MonoBehaviour {
    ///   [SerializeField] private EventT&lt;int&gt; _onDamage;
    ///
    ///   public void DamagePlayer(int damage) {
    ///     /* Update player health */
    ///     _onDamage.Invoke(damage);
    ///   }
    /// }
    ///
    /// 
    /// public class DamagePopupUI: MonoBehaviour {
    ///   [SerializeField] private EventT&lt;int&gt; _onDamage;
    /// 
    ///   private void OnEnable() => _onDamage.Listeners += OnDamage;
    /// 
    ///   private void OnDisable() => _onDamage.Listeners -= OnDamage;
    /// 
    ///   private void OnDamage(int damage) {
    ///     /* Spawn floating popup showing the player damage for a few seconds */
    ///   }
    /// }
    /// </code>
    /// </example>
    /// <seealso cref="VoidEvent"/>
    /// <seealso cref="EventTListener{T}"/>
    /// <typeparam name="T">Type of the payload being passed from producers to consumers</typeparam>
    public class EventT<T> : ScriptableObject
    {
        /// <summary>
        /// Subscribers to the event should add themselves to this delegate.
        /// </summary>
        /// <remarks>
        /// Remember to unsubscribe for the event before destroying your listener. In a
        /// <c>MonoBehaviour</c> component, this is often done in <c>OnDisable</c>
        /// or <c>OnDestroy</c>
        /// </remarks>
        public event Action<T> Listeners;
        
        /// <summary>
        /// Producers can trigger the event by calling this method and providing the event payload.
        /// </summary>
        /// <param name="value">The event payload</param>
        public void Invoke(T value) => Listeners?.Invoke(value);
    }
}
