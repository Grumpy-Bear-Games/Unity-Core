// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

using UnityEngine.InputSystem;

namespace Games.GrumpyBear.Core.InputSystem
{
    public static class ExtensionMethods
    { 
        #region UnityEngine.InputSystem
        public static InputBinding? GetEffectiveBindingMask(this InputAction action) =>
            (action.bindingMask ?? action.actionMap.bindingMask) ?? action.actionMap.asset.bindingMask;

        public static InputBinding? GetEffectiveBinding(this InputAction action)
        {
            var bindingMask = action.GetEffectiveBindingMask();
            if (!bindingMask.HasValue) return null;
            var index = action.GetBindingIndex(bindingMask.Value);
            return action.bindings[index];
        }

        public static string GetKeyName(this InputAction action)
        {
            var binding = action.GetEffectiveBinding();
            return binding.HasValue
                ? InputControlPath.ToHumanReadableString(
                    binding.Value.effectivePath,
                    InputControlPath.HumanReadableStringOptions.OmitDevice,
                    Gamepad.current
                )
                : "";
        }

        public static string GetKeyName(this PlayerInput playerInput, string action) =>
            playerInput.actions.FindAction(action).GetKeyName();

        public static string GetKeyName(this InputActionReference inputActionReference) =>
            inputActionReference.action.GetKeyName();
        #endregion
    }
}
