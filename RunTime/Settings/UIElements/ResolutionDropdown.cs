#if UNITY_2022_1_OR_NEWER
using UnityEngine;
using UnityEngine.UIElements;

namespace Games.GrumpyBear.Core.Settings.UIElements
{
    public sealed class ResolutionDropdown : VideoSettingsControl
    {
        private readonly PopupField<VideoSettings.ResolutionEntry> _dropdown;

        public new class UxmlFactory : UxmlFactory<ResolutionDropdown, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private readonly UxmlStringAttributeDescription labelAttr = new()
            {
                name = "label", defaultValue = "Resolution"
            };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                if (ve is not ResolutionDropdown resolutionDropdown) return;
                resolutionDropdown.label = labelAttr.GetValueFromBag(bag, cc);
            }
        }

        public string label
        {
            get => _dropdown.label;
            set => _dropdown.label = value;
        }

        protected override void UpdateUI()
        {
            _dropdown.choices = VideoSettings.Resolutions;
            _dropdown.SetValueWithoutNotify(VideoSettings != null ? VideoSettings.Resolution : null);
        }

        public ResolutionDropdown()
        {
            _dropdown = new PopupField<VideoSettings.ResolutionEntry>
            {
                formatListItemCallback = FormatResolution,
                formatSelectedValueCallback = FormatResolution
            };
            _dropdown.RegisterValueChangedCallback(SetResolution);
            _dropdown.RegisterCallback<GeometryChangedEvent>(UpdateOnShow);
            hierarchy.Add(_dropdown);
        }
        
        private void UpdateOnShow(GeometryChangedEvent evt)
        {
            if (evt.oldRect == Rect.zero && evt.newRect != Rect.zero) UpdateUI();
        }

        private static string FormatResolution(VideoSettings.ResolutionEntry entry) => entry is null ? "" : entry.ToString();

        private void SetResolution(ChangeEvent<VideoSettings.ResolutionEntry> evt)
        {
            if (VideoSettings == null) return;
            VideoSettings.Resolution = evt.newValue;
        }
    }
}
#endif
