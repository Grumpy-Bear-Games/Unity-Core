using UnityEngine;
using UnityEngine.UIElements;

namespace Games.GrumpyBear.Core.Settings.UIElements
{
    public sealed class ResolutionDropdown : PopupField<VideoSettings.ResolutionEntry>, IVideoSettingsControl
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
        
        public new class UxmlFactory : UxmlFactory<ResolutionDropdown, UxmlTraits> { }

        public ResolutionDropdown()
        {
            formatListItemCallback = FormatResolution;
            formatSelectedValueCallback = FormatResolution;
            this.RegisterValueChangedCallback(SetResolution);
            RegisterCallback<GeometryChangedEvent>(UpdateOnShow);
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
        
        private void UpdateUI()
        {
            choices = VideoSettings.Resolutions;
            SetValueWithoutNotify(VideoSettings != null ? VideoSettings.Resolution : null);
        }
    }
}
