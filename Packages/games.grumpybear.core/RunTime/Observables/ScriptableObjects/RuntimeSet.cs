using System.Collections.Generic;
using UnityEngine;

namespace Games.GrumpyBear.Core.Observables.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Grumpy Bear Games/Core/Observables/Runtime Set", fileName = "Runtime Set")]
    public class RuntimeSet: ReadOnlyObservable<int>
    {
        private readonly HashSet<GameObject> _runtimeSet = new();
        
        public void Register(GameObject instance)
        {
            _runtimeSet.Add(instance);
            Set(_runtimeSet.Count);
        }

        public void Unregister(GameObject instance)
        {
            _runtimeSet.Remove(instance);
            Set(_runtimeSet.Count);
        }
    }
}
