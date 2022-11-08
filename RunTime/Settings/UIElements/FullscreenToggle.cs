#if UNITY_2022_1_OR_NEWER
using UnityEngine;
using UnityEngine.UIElements;

namespace Games.GrumpyBear.Core.Settings.UIElements
{
    public sealed class FullscreenToggle : VideoSettingsControl
    {
        public new class UxmlFactory : UxmlFactory<FullscreenToggle, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private readonly UxmlStringAttributeDescription labelAttr = new()
            {
                name = "label", defaultValue = "Fullscreen"
            };

            private readonly UxmlStringAttributeDescription textAttr = new()
            {
                name = "text", defaultValue = ""
            };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                var fullscreenToggle = (ve as FullscreenToggle); 
                
                fullscreenToggle.label = labelAttr.GetValueFromBag(bag, cc);
                fullscreenToggle.text = textAttr.GetValueFromBag(bag, cc);
            }
        };

        public string label
        {
            get => _toggle.label;
            set => _toggle.label = value;
        }

        public string text
        {
            get => _toggle.text;
            set => _toggle.text = value;
        }

        private readonly Toggle _toggle;

        public override void UpdateUI() => _toggle.SetValueWithoutNotify(VideoSettings != null && VideoSettings.Fullscreen);

        public FullscreenToggle()
        {
            _toggle = new Toggle();
            _toggle.RegisterValueChangedCallback(SetFullscreen);
            _toggle.RegisterCallback<GeometryChangedEvent>(UpdateOnShow);
            hierarchy.Add(_toggle);
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
    }
}
#endif
