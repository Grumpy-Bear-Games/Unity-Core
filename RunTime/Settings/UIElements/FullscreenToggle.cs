using UnityEngine;
using UnityEngine.UIElements;

namespace Games.GrumpyBear.Core.Settings.UIElements
{
    public sealed class FullscreenToggle : Toggle, IVideoSettingsControl
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
        
        public new class UxmlFactory : UxmlFactory<FullscreenToggle, UxmlTraits> { }

        public FullscreenToggle()
        {
            this.RegisterValueChangedCallback(SetFullscreen);
            RegisterCallback<GeometryChangedEvent>(UpdateOnShow);
        }
        
        private void UpdateOnShow(GeometryChangedEvent evt)
        {
            if (evt.oldRect == Rect.zero && evt.newRect != Rect.zero) UpdateUI();
        }

        private void SetFullscreen(ChangeEvent<bool> evt)
        {
            if (VideoSettings == null) return;
            VideoSettings.Fullscreen = evt.newValue;
        }
        
        private void UpdateUI() => SetValueWithoutNotify(VideoSettings != null && VideoSettings.Fullscreen);
    }
}
