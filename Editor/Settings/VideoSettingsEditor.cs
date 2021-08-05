using System.Collections.Generic;
using System.Linq;
using Games.GrumpyBear.Core.Settings;
using UnityEditor;
using UnityEngine;

namespace Games.GrumpyBear.Core.Editor.Settings
{
    [CustomEditor(typeof(VideoSettings))]
    public class VideoSettingsEditor : UnityEditor.Editor
    {
        private VideoSettings _videoSettings;
        private List<VideoSettings.ResolutionEntry> _resolutionEntries;
        private string[] _resolutionNames;
        private string[] _qualityNames;

        private void OnEnable()
        {
            _videoSettings = target as VideoSettings;

            _resolutionEntries = VideoSettings.Resolutions;
            _resolutionNames = _resolutionEntries.Select(x => x.ToString()).ToArray();
            _qualityNames = VideoSettings.QualityNames.ToArray();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var hasPrefs = (PlayerPrefs.HasKey(_videoSettings.FullscreenSettingsKey) ||
                            PlayerPrefs.HasKey(_videoSettings.QualityIndexSettingsKey) ||
                            PlayerPrefs.HasKey(_videoSettings.ResolutionIndexSettingsKey));

            GUILayout.Space(20);
            if (hasPrefs)
                EditorGUILayout.HelpBox("Remember: These values are saved in PlayerPrefs; not in this object",
                    MessageType.Info);

            {
                var oldValue = _videoSettings.Fullscreen;
                var newValue = EditorGUILayout.Toggle("Fullscreen", oldValue);
                if (oldValue != newValue) PlayerPrefs.SetInt(_videoSettings.FullscreenSettingsKey, newValue ? 1 : 0);
            }

            {
                var oldValue = _videoSettings.QualityIndex;
                var newValue = EditorGUILayout.Popup("Quality", oldValue, _qualityNames);
                if (oldValue != newValue) PlayerPrefs.SetInt(_videoSettings.QualityIndexSettingsKey, newValue);
            }

            {
                var oldValue = _resolutionEntries.IndexOf(_videoSettings.Resolution);
                var newValue = EditorGUILayout.Popup("Resolution", oldValue, _resolutionNames);
                if (oldValue != newValue) PlayerPrefs.SetString(_videoSettings.ResolutionIndexSettingsKey, _resolutionEntries[newValue].ToString());
            }

            if (!hasPrefs) return;
            if (GUILayout.Button("Delete PlayerPrefs")) _videoSettings.ClearPlayerPrefs();
        }
    }
}
