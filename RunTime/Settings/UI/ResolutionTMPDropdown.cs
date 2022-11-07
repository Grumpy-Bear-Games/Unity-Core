#if USE_TEXTMESHPRO
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Games.GrumpyBear.Core.Settings.UI
{
    [AddComponentMenu("Grumpy Bear Games/Core/Settings/Resolution TMP_Dropdown")]
    [RequireComponent(typeof(TMP_Dropdown))]
    public class ResolutionTMPDropdown : MonoBehaviour
    {
        [SerializeField] private VideoSettings _videoSettings;

        private TMP_Dropdown _dropdown;
        private List<VideoSettings.ResolutionEntry> _resolutionEntries;

        private void Awake() => _dropdown = GetComponent<TMP_Dropdown>();

        private void OnEnable()
        {
            _resolutionEntries = VideoSettings.Resolutions;
            
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
#endif
