using UnityEngine;
using UnityEngine.UIElements;
using Games.GrumpyBear.Core.Settings;
using Games.GrumpyBear.Core.Settings.UIElements;

namespace Games.GrumpyBear.Core.Examples
{
    [RequireComponent(typeof(UIDocument))]
    public class OptionsMenu : MonoBehaviour
    {
        [SerializeField] private VideoSettings _videoSettings;
        
        private void Awake()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;

            root.Q<FullscreenToggle>().VideoSettings = _videoSettings;
            root.Q<QualityDropdown>().VideoSettings = _videoSettings;
            root.Q<ResolutionDropdown>().VideoSettings = _videoSettings;
        }
    }
}
