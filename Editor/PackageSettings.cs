using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.UIElements;

namespace Games.GrumpyBear.Core.Editor
{
    public class PackageSettings : EditorWindow
    {
        private Toggle _experimentalCheckBox;

        [MenuItem("Tools/Core Game Utilities/Package Settings")]
        private static void ShowWindow()
        {
            var window = GetWindow<PackageSettings>();
            window.titleContent = new GUIContent("Core Game Utilities");
            window.Show();
        }

        private const string ExperimentalDefine = "CORE_GAME_UTILITIES_EXPERIMENTAL";
        
        private static bool Experimental
        {
            get => PlayerSettings.GetScriptingDefineSymbols(NamedBuildTarget.Standalone).Contains(ExperimentalDefine);
            set
            {
                var defines = PlayerSettings.GetScriptingDefineSymbols(NamedBuildTarget.Standalone);
                if (value)
                {
                    if (defines.Contains(ExperimentalDefine)) return;
                    defines += $";{ExperimentalDefine}";
                    
                }
                else
                {
                    defines = defines.Replace(ExperimentalDefine, "").Replace(";;", ";");
                }
                PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.Standalone, defines);
            }
        }

        private void OnEnable()
        {
            var label = new Label("Optional features:")
            {
                style =
                {
                    unityFontStyleAndWeight = FontStyle.Bold
                }
            };
            rootVisualElement.Add(label);


            _experimentalCheckBox = new Toggle
            {
                label = "Enable experimental features"
            };
            _experimentalCheckBox.SetValueWithoutNotify(Experimental);
            _experimentalCheckBox.RegisterValueChangedCallback(evt => Experimental = evt.newValue);
            rootVisualElement.Add(_experimentalCheckBox);
        }

        private void OnInspectorUpdate()
        {
            if (EditorApplication.isCompiling) return;
            _experimentalCheckBox.SetValueWithoutNotify(Experimental);
        }
    }
}
