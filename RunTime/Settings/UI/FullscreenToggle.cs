using UnityEngine;
using UnityEngine.UI;

namespace Games.GrumpyBear.Core.Settings.UI
{
    [AddComponentMenu("Grumpy Bear Games/Core/Settings/Fullscreen Toggle")]
    [RequireComponent(typeof(Toggle))]
    public class FullscreenToggle : MonoBehaviour
    {
        [SerializeField] private VideoSettings _videoSettings;
        private Toggle _toggle;

        private void Awake() => _toggle = GetComponent<Toggle>();

        private void OnEnable()
        {
            _toggle.isOn = _videoSettings.Fullscreen;
            _toggle.onValueChanged.AddListener(OnToggle);
        }

        private void OnDisable() => _toggle.onValueChanged.RemoveListener(OnToggle);

        private void OnToggle(bool fullscreen) => _videoSettings.Fullscreen = fullscreen;
    }
}
