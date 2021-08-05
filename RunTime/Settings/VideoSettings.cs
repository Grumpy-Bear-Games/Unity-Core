using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Games.GrumpyBear.Core.Settings
{
    [CreateAssetMenu(menuName = "Grumpy Bear Games/Core/Settings/Video Settings", fileName = "Video Settings")]
    public class VideoSettings: ScriptableObject
    {
        [SerializeField] private string _fullscreenSettingsKey = "Settings/Video/Fullscreen";
        [SerializeField] private string _qualityIndexSettingsKey = "Settings/Video/QualityIndex";
        [SerializeField] private string _resolutionIndexSettingsKey = "Settings/Video/ResolutionIndex";

        public string FullscreenSettingsKey => _fullscreenSettingsKey;
        public string QualityIndexSettingsKey => _qualityIndexSettingsKey;
        public string ResolutionIndexSettingsKey => _resolutionIndexSettingsKey;
        
        public bool Fullscreen
        {
            get => PlayerPrefs.GetInt(_fullscreenSettingsKey, Screen.fullScreen ? 1 : 0) != 0;
            set
            {
                Screen.fullScreen = value;
                PlayerPrefs.SetInt(_fullscreenSettingsKey, value ? 1 : 0);
            }
        }

        public int QualityIndex
        {
            get => PlayerPrefs.GetInt(_qualityIndexSettingsKey, QualitySettings.GetQualityLevel());
            set
            {
                QualitySettings.SetQualityLevel(value);
                PlayerPrefs.SetInt(_qualityIndexSettingsKey, value);
            }
        }

        public ResolutionEntry Resolution
        {
            get => ResolutionEntry.FromString(PlayerPrefs.GetString(_resolutionIndexSettingsKey));
            set
            {
                Screen.SetResolution(value.Width, value.Height, Fullscreen);
                PlayerPrefs.SetString(_resolutionIndexSettingsKey, value.ToString());
            }
        }

        public static List<ResolutionEntry> Resolutions
        {
            get
            {
                var result =
                    new HashSet<ResolutionEntry>(Screen.resolutions.Select(ResolutionEntry.FromResolution))
                        .ToList();
                result.Sort();
                return result;
            }
        }
        
        public static List<string> QualityNames => QualitySettings.names.ToList();

        private void ApplyAll()
        {
            Screen.fullScreen = Fullscreen;
            QualitySettings.SetQualityLevel(QualityIndex);
            var res = Resolution;
            Screen.SetResolution(res.Width, res.Height, Fullscreen);
        }

        public class ResolutionEntry : IComparable<ResolutionEntry>
        {
            public readonly int Height;

            public readonly int Width;

            private ResolutionEntry(int width, int height)
            {
                Height = height;
                Width = width;
            }
            
            internal static ResolutionEntry FromResolution(Resolution resolution) => new ResolutionEntry(resolution.width, resolution.height);

            internal static ResolutionEntry FromString(string resolution)
            {
                try
                {
                    var entries = resolution.Split(new[] {'x'}, 2);
                    return new ResolutionEntry(int.Parse(entries[0]), int.Parse(entries[1]));
                }
                catch (Exception)
                {
                    return new ResolutionEntry(Screen.currentResolution.width, Screen.currentResolution.height);
                }
            }

            public override string ToString() => $"{Width}x{Height}";

            public int CompareTo(ResolutionEntry other)
            {
                var result = Width.CompareTo(other.Width);
                return result != 0 ? result : Height.CompareTo(other.Height);
            }

            public override bool Equals(object other)
            {
                if (!(other is ResolutionEntry entry)) return false; 
                return Height == entry.Height && Width == entry.Width;
            }

            public override int GetHashCode() => unchecked((Height * 397) ^ Width);
        }

        #if UNITY_EDITOR
        private void OnEnable()
        {
            UnityEditor.EditorApplication.playModeStateChanged += change =>
            {
                if (change != UnityEditor.PlayModeStateChange.EnteredPlayMode) return;
                ApplyAll();
            };
        }
        #else
        private void OnEnable() => ApplyAll();
        #endif
        
        public void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteKey(_fullscreenSettingsKey);
            PlayerPrefs.DeleteKey(_qualityIndexSettingsKey);
            PlayerPrefs.DeleteKey(_resolutionIndexSettingsKey);
        }
    }
}
