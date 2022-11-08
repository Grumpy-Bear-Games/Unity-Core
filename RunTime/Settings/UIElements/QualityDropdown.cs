#if UNITY_2022_1_OR_NEWER
using UnityEngine;
using UnityEngine.UIElements;

namespace Games.GrumpyBear.Core.Settings.UIElements
{
    public sealed class QualityDropdown : VideoSettingsControl
    {
        private readonly PopupField<string> _dropdown;

        public new class UxmlFactory : UxmlFactory<QualityDropdown, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private readonly UxmlStringAttributeDescription labelAttr = new()
            {
                name = "label", defaultValue = "Quality"
            };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                var resolutionDropdown = (ve as QualityDropdown); 
                
                resolutionDropdown.label = labelAttr.GetValueFromBag(bag, cc);
            }
        };

        public string label
        {
            get => _dropdown.label;
            set => _dropdown.label = value;
        }

        public override void UpdateUI()
        {
            _dropdown.SetValueWithoutNotify(VideoSettings != null ? VideoSettings.QualityNames[VideoSettings.QualityIndex] : "");
        }

        public QualityDropdown()
        {
            _dropdown = new PopupField<string>
            {
                formatListItemCallback = FormatQualityLevel,
                formatSelectedValueCallback = FormatQualityLevel,
                choices = VideoSettings.QualityNames,
                value = QualitySettings.names[QualitySettings.GetQualityLevel()]
            };
            _dropdown.RegisterValueChangedCallback(QualityLevel);
            _dropdown.RegisterCallback<GeometryChangedEvent>(UpdateOnShow);
            hierarchy.Add(_dropdown);
        }

        private void UpdateOnShow(GeometryChangedEvent evt)
        {
            if (evt.oldRect == Rect.zero && evt.newRect != Rect.zero) UpdateUI();
        }

        private static string FormatQualityLevel(string entry) => entry ?? "";

        private void QualityLevel(ChangeEvent<string> evt)
        {
            if (VideoSettings == null) return;
            VideoSettings.QualityIndex = VideoSettings.QualityNames.IndexOf(evt.newValue);
        }
    }
}
#endif
