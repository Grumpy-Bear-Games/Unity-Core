#if ENABLE_INPUT_SYSTEM
using UnityEngine;
using UnityEngine.InputSystem;

namespace Games.GrumpyBear.Core.Observables.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Grumpy Bear Games/Core/Observables/Player Input", fileName = "Player Input")]
    public class PlayerInputObservable: Observable<PlayerInput> { }
}
#endif
