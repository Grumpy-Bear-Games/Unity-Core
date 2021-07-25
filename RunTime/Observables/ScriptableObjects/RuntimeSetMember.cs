using UnityEngine;

namespace Games.GrumpyBear.Core.Observables.ScriptableObjects
{
    [AddComponentMenu("Grumpy Bear Games/Core/Observables/Runtime Set Member")]
    public class RuntimeSetMember : MonoBehaviour
    {
        [SerializeField] private RuntimeSet _runtimeSet;

        private void OnEnable() => _runtimeSet.Register(gameObject);
        private void OnDisable() => _runtimeSet.Unregister(gameObject);

    }
}
