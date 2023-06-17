using UnityEngine;
using UnityEngine.UIElements;

namespace Games.GrumpyBear.Core.Settings.UIElements
{
    public sealed class QualityDropdown : PopupField<string>, IVideoSettingsControl
    {
        private VideoSettings _videoSettings;
        
        public VideoSettings VideoSettings
        {
            get => _videoSettings;
            set
            {
                _videoSettings = value;
                UpdateUI();
            }
        }        
        
        public new class UxmlFactory : UxmlFactory<QualityDropdown, UxmlTraits> { }

        public QualityDropdown()
        {
            formatListItemCallback = FormatQualityLevel;
            formatSelectedValueCallback = FormatQualityLevel;
            choices = VideoSettings.QualityNames;
            value = QualitySettings.names[QualitySettings.GetQualityLevel()];
            this.RegisterValueChangedCallback(QualityLevel);
            RegisterCallback<GeometryChangedEvent>(UpdateOnShow);
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

        private void UpdateUI() => SetValueWithoutNotify(VideoSettings != null ? VideoSettings.QualityNames[VideoSettings.QualityIndex] : "");
    }
}
