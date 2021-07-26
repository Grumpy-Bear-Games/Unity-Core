using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Games.GrumpyBear.Core.Settings.UI
{
    [AddComponentMenu("Grumpy Bear Games/Core/Settings/Quality TMP_Dropdown")]
    [RequireComponent(typeof(TMP_Dropdown))]
    public class QualityTMPDropdown : MonoBehaviour
    {
        [SerializeField] private VideoSettings _videoSettings;

        private TMP_Dropdown _dropdown;

        private void Awake() => _dropdown = GetComponent<TMP_Dropdown>();

        private void OnEnable()
        {
            _dropdown.ClearOptions();
            _dropdown.AddOptions(VideoSettings.QualityNames);
            _dropdown.value = _videoSettings.QualityIndex;
            _dropdown.RefreshShownValue();
            
            _dropdown.onValueChanged.AddListener(OnValueChange);
        }

        private void OnDisable() => _dropdown.onValueChanged.RemoveListener(OnValueChange);

        private void OnValueChange(int qualityIndex) => _videoSettings.QualityIndex = qualityIndex;
    }
}
