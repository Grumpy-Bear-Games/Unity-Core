using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Games.GrumpyBear.Core.Settings.UI
{
    [AddComponentMenu("Grumpy Bear Games/Core/Settings/Resolution Dropdown")]
    [RequireComponent(typeof(Dropdown))]
    public class ResolutionDropdown : MonoBehaviour
    {
        [SerializeField] private VideoSettings _videoSettings;

        private Dropdown _dropdown;
        private List<VideoSettings.ResolutionEntry> _resolutionEntries;

        private void Awake() => _dropdown = GetComponent<Dropdown>();

        private void OnEnable()
        {
            _resolutionEntries = _videoSettings.Resolutions;
            
            _dropdown.ClearOptions();
            _dropdown.AddOptions(_resolutionEntries.Select(x=>x.ToString()).ToList());
            _dropdown.value = _resolutionEntries.IndexOf(_videoSettings.Resolution);
            _dropdown.RefreshShownValue();
            
            _dropdown.onValueChanged.AddListener(OnValueChange);
        }

        private void OnDisable() => _dropdown.onValueChanged.RemoveListener(OnValueChange);

        private void OnValueChange(int index) => _videoSettings.Resolution = _resolutionEntries[index];
    }
}
