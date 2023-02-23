using System.Linq;
using Games.GrumpyBear.Core.LevelManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Games.GrumpyBear.Core.Editor.LevelManagement
{
    [CustomEditor(typeof(SceneGroup))]
    public class SceneGroupEditor: UnityEditor.Editor
    {
        private SceneGroup _sceneGroup;
        private SerializedProperty _scenesProperty;
        
        private void OnEnable()
        {
            _sceneGroup = target as SceneGroup;
            _scenesProperty = serializedObject.FindProperty("_scenes");
        }

        public override VisualElement CreateInspectorGUI()
        {
            // Create a new VisualElement to be the root of our inspector UI
            var myInspector = new VisualElement();
            var nameProperty = new PropertyField
            {
                bindingPath = "m_Name"
            };
            var scenesProperty = new PropertyField
            {
                bindingPath = "_scenes"
            };
            myInspector.Add(nameProperty);
            myInspector.Add(scenesProperty);
            myInspector.Bind(serializedObject);
            nameProperty.RegisterValueChangeCallback(UpdateAssetDatabase);
            
            // Return the finished inspector UI
            return myInspector;
        }

        private void UpdateAssetDatabase(SerializedPropertyChangeEvent evt)
        {
            Debug.Log(target.name);
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssetIfDirty(target);
            AssetDatabase.Refresh();
        }

        /*
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("The first scene in the list will be the active one.", MessageType.Info, true);

            base.OnInspectorGUI();

            var hasProblems = false;
            foreach (var scene in _sceneGroup.Scenes)
            {
                if (string.IsNullOrEmpty(scene.ScenePath)) continue;
                if (scene.BuildIndex != -1) continue;
                EditorGUILayout.HelpBox($"{scene.ScenePath} is missing from the build", MessageType.Warning);
                hasProblems = true;
            }

            if (hasProblems && GUILayout.Button("Fix all problems")) FixAllProblems(); 

            GUI.enabled = _sceneGroup.Scenes.Count > 0;
            if (GUILayout.Button("Load location")) LoadScene();
            GUI.enabled = true;
        }
        */
        private void FixAllProblems()
        {
            var editorBuildSettingsScenes = EditorBuildSettings.scenes.ToList();
            editorBuildSettingsScenes.AddRange(
                from scene in _sceneGroup.Scenes
                where scene.BuildIndex == -1
                select new EditorBuildSettingsScene(scene.ScenePath, true)
            );
            EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
        }

        private void LoadScene()
        {
            var openScenes = Enumerable.Range(0, EditorSceneManager.loadedSceneCount)
                .Select(EditorSceneManager.GetSceneAt)
                .Where(scene => _sceneGroup.Scenes.All(x => scene.buildIndex != x.BuildIndex))
                .ToArray();
            if (!EditorSceneManager.SaveModifiedScenesIfUserWantsTo(openScenes)) return;
            foreach (var sceneAsset in _sceneGroup.Scenes)
            {
                EditorSceneManager.OpenScene(sceneAsset.ScenePath, OpenSceneMode.Additive);
            }
            EditorSceneManager.SetActiveScene(EditorSceneManager.GetSceneByPath(_sceneGroup.ActiveScene.ScenePath));
            foreach (var openScene in openScenes)
            {
                EditorSceneManager.CloseScene(openScene, true);
            }
        }        
    }
}
