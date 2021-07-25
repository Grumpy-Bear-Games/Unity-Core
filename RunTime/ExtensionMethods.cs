// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Games.GrumpyBear.Core
{
    public static class ExtensionMethods
    {
        #region <Any type>
        public static void Swap<T>(ref T a, ref T b) {
            var tmp = a;
            a = b;
            b = tmp;
        }
        #endregion

        #region float
        public static bool Approximate(this float value, float compareTo) => Mathf.Approximately(value, compareTo);
        public static bool IsZero(this float value) => Mathf.Approximately(value, 0f);
        #endregion

        #region Vector math
        public static Vector2 Lerp(Vector2 a, Vector2 b, float t) => a + (b - a) * t;
        
        public static Vector3 Lerp(Vector3 a, Vector3 b, float t) => a + (b - a) * t;

        public static Vector2 QuadraticCurve(Vector2 a, Vector2 b, Vector2 c, float t)
        {
            var p0 = Lerp(a, b, t);
            var p1 = Lerp(b, c, t);
            return Lerp(p0, p1, t);
        }
        
        public static Vector3 QuadraticCurve(Vector3 a, Vector3 b, Vector3 c, float t)
        {
            var p0 = Lerp(a, b, t);
            var p1 = Lerp(b, c, t);
            return Lerp(p0, p1, t);
        }

        public static Vector2 CubicCurve(Vector2 a, Vector2 b, Vector2 c, Vector2 d, float t)
        {
            var p0 = QuadraticCurve(a, b, c, t);
            var p1 = QuadraticCurve(b, c, d, t);
            return Lerp(p0, p1, t);
        }
        
        public static Vector3 CubicCurve(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
        {
            var p0 = QuadraticCurve(a, b, c, t);
            var p1 = QuadraticCurve(b, c, d, t);
            return Lerp(p0, p1, t);
        }
        #endregion
        
        #region Rect
        public static Rect Copy(this Rect rect) => new Rect(rect.position, rect.size);

        public static Rect SetWidth(this Rect rect, float width)
        {
            rect.width = width;
            return rect;
        }

        public static Rect SetMargin(this Rect rect, float top=0, float bottom=0, float left=0, float right=0)
        {
            rect.yMin += top;
            rect.yMax -= bottom;
            rect.xMin += left;
            rect.xMax -= right;
            return rect;
        }

        public static Rect WithWidth(this Rect rect, float width) => rect.Copy().SetWidth(width);

        public static Rect WithMargin(this Rect rect, float top=0, float bottom=0, float left=0, float right=0) =>
            rect.Copy().SetMargin(top, bottom, left, right);
        #endregion
        
        #region IReadOnlyList<T>
        public static T PickRandom<T>(this IReadOnlyList<T> objects) => objects[Random.Range(0, objects.Count)];
        #endregion

        #region UnityEngine.Transform
        public static void DestroyAllChildren(this Transform transform) {
            for (var i = transform.childCount-1; i >= 0; i--)
            {
                Object.Destroy(transform.GetChild(i).gameObject);    
            }
        }
        
        public static void DestroyAllChildrenImmediate(this Transform transform) {
            for (var i = transform.childCount-1; i >= 0; i--)
            {
                Object.DestroyImmediate(transform.GetChild(i).gameObject);    
            }
        }

        public static void DestroyAllChildren<T>(this Transform transform) where T: MonoBehaviour
        {
            var childrenToDestroy = transform.GetComponentsInChildren<T>(); 
            for (var i = childrenToDestroy.Length-1; i >= 0; i--)
            {
                Object.Destroy(childrenToDestroy[i].gameObject);    
            }
        }

        public static void DestroyAllChildrenImmediate<T>(this Transform transform) where T: MonoBehaviour
        {
            var childrenToDestroy = transform.GetComponentsInChildren<T>(); 
            for (var i = childrenToDestroy.Length-1; i >= 0; i--)
            {
                Object.DestroyImmediate(childrenToDestroy[i].gameObject);    
            }
        }
        #endregion
        
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
        
        #region UnityEngine.LineRenderer
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
        
        public static void BezierCurve(this LineRenderer lineRenderer, Vector3 a, Vector3 b, Vector3 c, Vector3 d)
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
