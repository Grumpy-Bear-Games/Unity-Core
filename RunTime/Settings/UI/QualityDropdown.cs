using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Games.GrumpyBear.Core.Settings.UI
{
    [AddComponentMenu("Grumpy Bear Games/Core/Settings/Quality Dropdown")]
    [RequireComponent(typeof(Dropdown))]
    public class QualityDropdown : MonoBehaviour
    {
        [SerializeField] private VideoSettings _videoSettings;

        private Dropdown _dropdown;

        private void Awake() => _dropdown = GetComponent<Dropdown>();

        private void OnEnable()
        {
            _dropdown.ClearOptions();
            _dropdown.AddOptions(QualityNames);
            _dropdown.value = _videoSettings.QualityIndex;
            _dropdown.RefreshShownValue();
            
            _dropdown.onValueChanged.AddListener(OnValueChange);
        }

        private void OnDisable() => _dropdown.onValueChanged.RemoveListener(OnValueChange);

        private void OnValueChange(int qualityIndex) => _videoSettings.QualityIndex = qualityIndex;

        public static List<string> QualityNames => QualitySettings.names.ToList();
    }
}
