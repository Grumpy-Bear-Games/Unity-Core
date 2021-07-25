using UnityEngine;
using UnityEngine.Events;

namespace Games.GrumpyBear.Core.Triggers
{
    [RequireComponent(typeof(Collider2D))]
    public class Trigger2D : MonoBehaviour
    {
        [SerializeField] private UnityEvent<Collider2D> _onEnter;
        [SerializeField] private UnityEvent<Collider2D> _onExit;
        
        private void OnTriggerEnter2D(Collider2D other) => _onEnter.Invoke(other);
        private void OnTriggerExit2D(Collider2D other) => _onExit.Invoke(other);
    }
}
