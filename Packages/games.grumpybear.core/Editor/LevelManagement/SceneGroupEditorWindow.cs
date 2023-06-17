using System;
using Games.GrumpyBear.Core.LevelManagement;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Games.GrumpyBear.Core.Editor.LevelManagement
{
    public class SceneGroupEditorWindow : EditorWindow
    {
        [SerializeField] private StyleSheet _styleSheet;
        
        private SceneManager[] _sceneManagers;

        private const string UssBaseClass = "scene-group-editor-window";
        private static readonly string UssSceneManagerClass = $"{UssBaseClass}__scene-manager";
        private static readonly string UssSceneGroupEntryClass = $"{UssBaseClass}__scene-group-entry";
        
        
        [MenuItem("Tools/Core Game Utilities/Scene Groups", false, 0)]
        private static void ShowWindow()
        {
            var window = GetWindow<SceneGroupEditorWindow>();
            window.titleContent = new GUIContent("Scene Groups");
            window.Show();
        }


        private void RebuildEditorWindow()
        {
            rootVisualElement.Clear();

            _sceneManagers = Resources.FindObjectsOfTypeAll<SceneManager>();

            foreach (var sceneManager in _sceneManagers)
            {
                var foldout = new Foldout()
                {
                    text = sceneManager.name,
                    viewDataKey = sceneManager.GetHashCode().ToString(),
                };
                foldout.AddToClassList(UssSceneManagerClass);
                rootVisualElement.Add(foldout);

                foreach (var sceneGroup in sceneManager.SceneGroups)
                {
                    var sceneGroupEntry = new VisualElement();
                    sceneGroupEntry.AddToClassList(UssSceneGroupEntryClass);
                    sceneGroupEntry.Add(new Label(sceneGroup.name));
                    sceneGroupEntry.Add(new Button(sceneGroup.LoadInEditor)
                    {
                        text = "Load in editor",
                    });
                    foldout.Add(sceneGroupEntry);
                }
            }
        }

        private void CreateGUI()
        {
            if (_styleSheet != null) rootVisualElement.styleSheets.Add(_styleSheet);
            RebuildEditorWindow();
        }

        private void OnEnable()
        {
            AssetPostprocessor.OnAssetsChanged += RebuildEditorWindow;
            AssetModificationProcessor.OnAssetCreated += RebuildEditorWindow;
        }

        private void OnDisable()
        {
            AssetPostprocessor.OnAssetsChanged -= RebuildEditorWindow;
            AssetModificationProcessor.OnAssetCreated -= RebuildEditorWindow;
        }

        private class AssetPostprocessor: UnityEditor.AssetPostprocessor
        {
            public static event Action OnAssetsChanged;
            
            private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
            {
                EditorApplication.delayCall += () => OnAssetsChanged?.Invoke();
            }        
        }

        private class AssetModificationProcessor : UnityEditor.AssetModificationProcessor
        {
            public static event Action OnAssetCreated;
            private static void OnWillCreateAsset(string assetName)
            {
                EditorApplication.delayCall += () => OnAssetCreated?.Invoke();
            }
        }
    }
}
