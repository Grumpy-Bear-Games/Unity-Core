using System;
using Games.GrumpyBear.Core.Settings;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Games.GrumpyBear.Core.Editor.Settings
{
    [CustomEditor(typeof(VideoSettings))]
    public class VideoSettingsEditor : UnityEditor.Editor
    {
        [SerializeField] private StyleSheet _styleSheet;

        private static string UssClassBase = "video-settings-editor";
        private string UssCurrentSettingsClass = $"{UssClassBase}_current-settings";
        
        
        private VideoSettings _videoSettings;

        private void OnEnable() => _videoSettings = target as VideoSettings;

        private VisualElement RowWrap(VisualElement field)
        {
            var row = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    justifyContent = Justify.SpaceBetween
                }
            };
            row.Add(field);

            var clearButton = new Button();
            clearButton.Add(new Image
            {
                image = EditorGUIUtility.IconContent("TreeEditor.Trash").image,
                scaleMode = ScaleMode.ScaleToFit,
            });

            row.Add(clearButton);
            return row;
        }

        private bool HasFullscreenSettings => PlayerPrefs.HasKey(_videoSettings.FullscreenSettingsKey);
        private bool HasQualitySettings => PlayerPrefs.HasKey(_videoSettings.QualityIndexSettingsKey);
        private bool HasResolutionSettings => PlayerPrefs.HasKey(_videoSettings.ResolutionIndexSettingsKey);
        private bool HasAnySettings => HasFullscreenSettings || HasQualitySettings || HasResolutionSettings;
        
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            root.styleSheets.Add(_styleSheet);
            
            InspectorElement.FillDefaultInspector(root, serializedObject, this);
            
            var box = new Box();
            box.AddToClassList(UssCurrentSettingsClass);

            var label = new Label("Current Settings");
            label.AddToClassList($"{UssCurrentSettingsClass}_header");
            box.Add(label);

            var noticeBox = new HelpBox("Remember: These values are saved in PlayerPrefs; not in this object", HelpBoxMessageType.Info);
            noticeBox.AddToClassList($"{UssCurrentSettingsClass}_notice-box");
            box.Add(noticeBox);

            var fullscreenToggle = new Toggle("Fullscreen")
            {
                value = _videoSettings.Fullscreen,
            };
            box.Add(RowWrap(fullscreenToggle));
            var clearFullscreenButton = fullscreenToggle.parent.Q<Button>();

            var qualityDropdown = new DropdownField("Quality")
            {
                choices = VideoSettings.QualityNames,
                index = HasQualitySettings ? _videoSettings.QualityIndex : -1,
            };
            box.Add(RowWrap(qualityDropdown));
            var clearQualityButton = qualityDropdown.parent.Q<Button>();
            
            var resolutionDropdown = new PopupField<VideoSettings.ResolutionEntry>("Resolution")
            {
                choices = VideoSettings.Resolutions,
            };
            box.Add(RowWrap(resolutionDropdown));
            var clearResolutionButton = resolutionDropdown.parent.Q<Button>();
            
            var clearAllButton = new Button
            {
                text = "Clear All Settings"
            };
            clearAllButton.AddToClassList($"{UssCurrentSettingsClass}_clear-all-button");
            box.Add(clearAllButton);
            
            root.Add(box);
            
            ///////////////////////////////////////////////////////////////////////////

            void UpdateButtonsAndNoticeBox()
            {
                noticeBox.style.display = HasAnySettings ? DisplayStyle.Flex : DisplayStyle.None;
                clearFullscreenButton.SetEnabled(HasFullscreenSettings);
                clearQualityButton.SetEnabled(HasQualitySettings);
                clearResolutionButton.SetEnabled(HasResolutionSettings);
                clearAllButton.SetEnabled(HasAnySettings);
            }

            clearFullscreenButton.clicked += () => {
                PlayerPrefs.DeleteKey(_videoSettings.FullscreenSettingsKey);
                fullscreenToggle.SetValueWithoutNotify(false);
                UpdateButtonsAndNoticeBox();
            };
            fullscreenToggle.RegisterValueChangedCallback(evt =>
            {
                if (evt.previousValue == evt.newValue) return;
                PlayerPrefs.SetInt(_videoSettings.FullscreenSettingsKey, evt.newValue ? 1 : 0);
                UpdateButtonsAndNoticeBox();
            });
            
            clearQualityButton.clicked += () => {
                PlayerPrefs.DeleteKey(_videoSettings.QualityIndexSettingsKey);
                qualityDropdown.SetValueWithoutNotify(null);
                UpdateButtonsAndNoticeBox();
            };
            qualityDropdown.RegisterValueChangedCallback(evt =>
            {
                if (evt.previousValue == evt.newValue) return;
                PlayerPrefs.SetInt(_videoSettings.QualityIndexSettingsKey, qualityDropdown.index);
                UpdateButtonsAndNoticeBox();
            });

            clearResolutionButton.clicked += () => {
                PlayerPrefs.DeleteKey(_videoSettings.ResolutionIndexSettingsKey);
                resolutionDropdown.SetValueWithoutNotify(null);
                UpdateButtonsAndNoticeBox();
            };
            resolutionDropdown.RegisterValueChangedCallback(evt =>
            {
                if (evt.previousValue == evt.newValue) return;
                PlayerPrefs.SetString(_videoSettings.ResolutionIndexSettingsKey, evt.newValue.ToString());
                UpdateButtonsAndNoticeBox();
            });

            clearAllButton.clicked += () =>
            {
                _videoSettings.ClearPlayerPrefs();
                UpdateButtonsAndNoticeBox();
            };

            UpdateButtonsAndNoticeBox();

            return root;
        }
    }
}
