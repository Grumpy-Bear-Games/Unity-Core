using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Games.GrumpyBear.Core.InputSystem
{
    [AddComponentMenu("Grumpy Bear Games/Core/Input System/Input Action Performed Listener")]
    public class InputActionPerformedListener : MonoBehaviour
    {
        [SerializeField] private InputActionReference _actionReference;
        [SerializeField] private UnityEvent _onTriggered;

        private void Awake() => _actionReference.action.Enable();
        private void OnEnable() => _actionReference.action.performed += OnTriggered;
        private void OnDisable() => _actionReference.action.performed -= OnTriggered;

        private void OnTriggered(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) _onTriggered.Invoke();
        }
    }
}
