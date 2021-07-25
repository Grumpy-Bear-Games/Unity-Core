using UnityEngine;
using UnityEngine.Events;

namespace Games.GrumpyBear.Core.Triggers
{
    [AddComponentMenu("Grumpy Bear Games/Core/Triggers/Trigger3D")]
    [RequireComponent(typeof(Collider))]
    public class Trigger3D : MonoBehaviour
    {
        [SerializeField] private UnityEvent<Collider> _onEnter;
        [SerializeField] private UnityEvent<Collider> _onExit;
        
        private void OnTriggerEnter(Collider other) => _onEnter.Invoke(other);
        private void OnTriggerExit(Collider other) => _onExit.Invoke(other);
    }
}
