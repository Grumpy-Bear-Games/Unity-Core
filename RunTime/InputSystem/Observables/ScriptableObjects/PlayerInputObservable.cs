using Games.GrumpyBear.Core.Observables.ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Games.GrumpyBear.Core.InputSystem.Observables.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Grumpy Bear Games/Core/Observables/Player Input", fileName = "Player Input")]
    public class PlayerInputObservable: Observable<PlayerInput> { }
}
