// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
using UnityEngine;
using UnityEngine.InputSystem;

namespace Games.GrumpyBear.Core
{
    public static class ExtensionMethods
    {
        #region Input System
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
        
        #region Bezier support for LineRenderer
        private static Vector2 Lerp(Vector2 a, Vector2 b, float t) => a + (b - a) * t;

        private static Vector2 QuadraticCurve(Vector2 a, Vector2 b, Vector2 c, float t)
        {
            var p0 = Lerp(a, b, t);
            var p1 = Lerp(b, c, t);
            return Lerp(p0, p1, t);
        }

        private static Vector2 CubicCurve(Vector2 a, Vector2 b, Vector2 c, Vector2 d, float t)
        {
            var p0 = QuadraticCurve(a, b, c, t);
            var p1 = QuadraticCurve(b, c, d, t);
            return Lerp(p0, p1, t);
        }

        public static void BezierCurve(this LineRenderer lineRenderer, Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            var positions = new Vector3[lineRenderer.positionCount];
            for (var i = 0; i < lineRenderer.positionCount; i++)
            {
                var t = i / (float) (lineRenderer.positionCount - 1); 
                positions[i] = CubicCurve(a, b, c, d, t);
            }
            lineRenderer.SetPositions(positions);
   
        }
        #endregion
    }
}
