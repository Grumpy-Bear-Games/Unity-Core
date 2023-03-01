using UnityEngine.UIElements;

namespace Games.GrumpyBear.Core.Settings.UIElements
{
    public abstract class VideoSettingsControl : VisualElement
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

        protected abstract void UpdateUI();
    }
}
