using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Games.GrumpyBear.Core.UI
{
    // Acknowledgement: Adapted from https://forum.unity.com/threads/dropdown-autoscroller.441424/#post-7133597
    
    [AddComponentMenu("Grumpy Bear Games/Core/UI/Dropdown - Auto Scroll To Selected")]
    [RequireComponent(typeof(Dropdown))]
    public class DropdownAutoScroll : MonoBehaviour, IPointerClickHandler
    {
        private Dropdown _dropdown;

        private void Awake() => _dropdown = GetComponent<Dropdown>();

        public void OnPointerClick(PointerEventData _) {
            if (!_dropdown.GetComponentInChildren<Scrollbar>() || !_dropdown.IsActive() || !_dropdown.IsInteractable()) {
                return;
            }
 
            var verticalScrollbar = GetComponentInChildren<ScrollRect>()?.verticalScrollbar;
            if (_dropdown.options.Count <= 1 || verticalScrollbar == null) return;
            
            var valuePosition = (float)_dropdown.value / (_dropdown.options.Count - 1);
            var value = verticalScrollbar.direction == Scrollbar.Direction.TopToBottom ? valuePosition : 1f - valuePosition;
            verticalScrollbar.value = Mathf.Max(.001f, value);
        }
    }
}
