using UnityEngine;

namespace Games.GrumpyBear.Core.Observables.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Grumpy Bear Games/Core/Observables/Int Observable", fileName = "Int Observable")]
    public class IntObservable : Observable<int>
    {
        public void Add(int x) => Set(Value + x);
        public void Increment() => Add(1);
        public void Decrement() => Add(-1);
    }
}
