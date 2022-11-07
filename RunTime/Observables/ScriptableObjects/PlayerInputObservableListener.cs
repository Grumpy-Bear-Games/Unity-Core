#if ENABLE_INPUT_SYSTEM
using UnityEngine;
using UnityEngine.InputSystem;

namespace Games.GrumpyBear.Core.Observables.ScriptableObjects
{
    [AddComponentMenu("Grumpy Bear Games/Core/Observables/PlayerInput Observable Listener")]
    public class PlayerInputObservableListener: ObservableListener<PlayerInput> { }
}
#endif
