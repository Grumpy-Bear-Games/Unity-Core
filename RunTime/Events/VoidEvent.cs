// ReSharper disable UnusedMember.Global
using System;
using UnityEngine;

namespace Games.GrumpyBear.Core.Events
{
    /// <summary>
    /// A generic reference-able event used to send signals between components in a Pub-Sub fashion without
    /// neither invoker nor listener knowing about each other. This is similar to <see cref="EventT{T}"/>
    /// but without an event payload.
    ///
    /// Because instances of <see cref="VoidEvent"/> are <c>ScriptableObjects</c>
    /// they can be referenced in serialized fields and <c>UnityEvents</c>.
    /// </summary>
    ///
    /// <example>
    /// Imagine you have a player health component and a UI components, which shows a Game Over screen
    /// when the player dies. Using <see cref="VoidEvent"/> we can have the health component broadcast
    /// when the player dies to the UI components without any of them knowing about or depending on each
    /// other.
    ///
    /// <code>
    /// public class PlayerHealth: MonoBehaviour {
    ///   [SerializeField] private VoidEvent _onPlayerDied;
    ///   private int _health;
    ///
    ///   public void DamagePlayer(int damage) {
    ///     /* Update player health */
    ///     if (_health &lt;= 0) {
    ///       _onPlayerDies.Invoke();
    ///     }
    ///   }
    /// }
    ///
    /// 
    /// public class GameOverUI: MonoBehaviour {
    ///   [SerializeField] private VoidEvent _onPlayerDied;
    /// 
    ///   private void OnEnable() => _onPlayerDied.Listeners += OnPlayerDied;
    /// 
    ///   private void OnDisable() => _onPlayerDied.Listeners -= OnPlayerDied;
    /// 
    ///   private void OnPlayerDied() {
    ///     /* Show Game Over screen */
    ///   }
    /// }
    /// </code>
    /// </example>
    /// <seealso cref="EventT{T}"/>
    /// <seealso cref="VoidEventListener"/>
    [CreateAssetMenu(menuName = "Grumpy Bear Games/Core/Events/Void Event", fileName = "Void Event")]
    public class VoidEvent : ScriptableObject
    {
        /// <summary>
        /// Subscribers to the event should add themselves to this delegate.
        /// </summary>
        /// <remarks>
        /// Remember to unsubscribe for the event before destroying your listener. In a
        /// <c>MonoBehaviour</c> component, this is often done in <c>OnDisable</c>
        /// or <c>OnDestroy</c>
        /// </remarks>
        public event Action Listeners;

        /// <summary>
        /// Invokers can trigger the event by calling this method.
        /// </summary>
        /// <param name="value">The event payload</param>
        public void Invoke() => Listeners?.Invoke();
    }
}
